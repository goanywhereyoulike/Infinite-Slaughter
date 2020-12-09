using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject hudPrefab = null;
    private GamePlayhud _hud = null;
    private void Awake()
    {
        if (hudPrefab == null)
        {
            Debug.Log("UIManager has no hud prefab assigned");
        }
        
        GameLoader.CallOnComplete(Initialize);

    }

    public void Initialize()
    {
        var hudProject = Instantiate(hudPrefab);

        hudProject.transform.SetParent(transform);

        _hud = hudProject.GetComponent<GamePlayhud>();
        if (_hud == null)
        {
            Debug.Log("Gameplay hud is null");
            return;
        }
        _hud.Initialize();
        StartCoroutine(TimeAdd());
    }

    public void UpdateScoreDisplay(int currentStore)
    {
        _hud.UpdateScores(currentStore);

    }

    public void DisplayMessageText(string message)
    {
        _hud.UpdateMessageText(message);
    }
    public void SetHealthBar(float maxHealth)
    {
        _hud.SetHealthBar(maxHealth);

    }
    public void UpdateHealthBar(float health)
    {

        _hud.UpdateHealthBar(health);
    }

    private float time = 0.0f;
    public float Time
    {
        get
        {
            return time;
        }
        set
        {
            time = value;
            UPdateTimeDisplay();
        }
    }
    public void UPdateTimeDisplay()
    {
        _hud.UpdateTimeText(Time);
    }


    public void Pause()
    {
        _hud.Pause();
    }

   
    private IEnumerator TimeAdd()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            Time++;
        }
    }
}
