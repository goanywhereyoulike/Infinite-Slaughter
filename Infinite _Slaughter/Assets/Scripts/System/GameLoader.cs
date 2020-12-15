using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : AsynCloader
{
    public int sceneIndexToLoad = 1;
    public List<Component> GameModules;
    public GameObject UIManagerPrefab = null;
    public LoadingScreen loadingScreen = null;
    private static int _sceneIndex = 1;
    private static GameLoader _instance;
    protected override void Awake()
    {
        Debug.Log("GameLoader Starting");
        if (_instance != null && _instance != this)
        {
            Debug.Log("A Duplicated instance of the GameLoader was found, and will be ignored. Only one instance is permitted.");
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        if (sceneIndexToLoad < 0 || sceneIndexToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Invalid Scene Index {sceneIndexToLoad} ... using default value of {_sceneIndex}");
        }
        else
        {
            _sceneIndex = sceneIndexToLoad;
        }

        GameObject systemGO = new GameObject("[Services]");
        systemGO.tag = "Services";
        Transform systemParent = systemGO.transform;
        DontDestroyOnLoad(systemGO);

        loadingScreen.UpdateLoadingStep("Loading Game Systems");

        Enqueue(InitializeCoreSystems(systemParent), 50, UpdateCoreSystemsProgress);
        Enqueue(InitializeModularSystems(systemParent), 50, UpdateModularSystemsProgress);

       
        CallOnComplete(OnComplete);
    }


    private float _coreLoadTotalSteps = 20.0f;
    private float _coreLoadCurrentStep = 0.0f;

    private float UpdateCoreSystemsProgress()
    {
        return _coreLoadCurrentStep / _coreLoadTotalSteps;

    }

    private float _modularLoadTotalSteps = 300.0f;
    private float _modularLoadCurrentStep = 0.0f;


    private float UpdateModularSystemsProgress()
    {
        return _modularLoadCurrentStep / _modularLoadTotalSteps;
    }


    protected override void ProgressUpdated(float percentComplete)
    {
        base.ProgressUpdated(percentComplete);
        loadingScreen.UpdateLoadingBar(percentComplete);
        loadingScreen.UpdateLoadingStep("Progress: " + Mathf.Ceil(percentComplete * 100.0f) + "%");
    }

    private IEnumerator InitializeCoreSystems(Transform systemsParent)
    {
        Debug.Log("Loading core system");

        _coreLoadCurrentStep = 1.0f;

        GameObject uiGO = GameObject.Instantiate(UIManagerPrefab);
        uiGO.transform.SetParent(systemsParent);
        var uiMngr = uiGO.GetComponent<UIManager>();
        ServiceLocator.Register<UIManager>(uiMngr);
        yield return null;

        _coreLoadCurrentStep += 1.0f;
        GameObject gmGO = new GameObject("GameManager");
        gmGO.transform.SetParent(systemsParent);
        var gm = gmGO.AddComponent<GameManager>();
        ServiceLocator.Register<GameManager>(gm.Initialize(sceneIndexToLoad));
        yield return null;


        for (int i = 0; i < _coreLoadTotalSteps - 2.0f; ++i)
        {
            _coreLoadCurrentStep += 1.0f;
            yield return null;
        }
    }

    private IEnumerator InitializeModularSystems(Transform systemsParent)
    {
        Debug.Log("Loading modular systems...");
       // yield return null;

        foreach (Component c in GameModules)
        {
            if (c is IGameModule)
            {
                IGameModule module = (IGameModule)c;
                yield return module.LoadModule();
            }



        }
        yield return null;
    }

    private void OnComplete()
    {
        Debug.Log("GameLoader Completed");
        StartCoroutine(LoadInitialScene(_sceneIndex));
    }

    private IEnumerator LoadInitialScene(int index)
    {
        Debug.Log("GameLoader Starting Scene load");
        var loadOp = SceneManager.LoadSceneAsync(index);

        loadingScreen.UpdateLoadingStep("Loading Scene: " + index.ToString());

        while (!loadOp.isDone)
        {
            loadingScreen.UpdateLoadingBar(loadOp.progress);
            yield return loadOp;
        }
    }
}


