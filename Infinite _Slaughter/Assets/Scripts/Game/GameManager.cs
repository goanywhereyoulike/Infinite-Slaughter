using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static readonly Dictionary<int, int> _killEnemiesByLevel = new Dictionary<int, int>()
    {
        {1,4},
        {2,3}
     
       
    };
    private int _topScore = 0;
    private int _numEnemyKilled = 0;
    private int _currentScore = 0;
    private int _currentCoin = 0;
    public int CurrentScore { get { return _currentScore; } }

    private int _currentLevevl = 0;
    public int CurrentLevel { get { return _currentLevevl; } }

    public int CurrentCoin { get { return _currentCoin; } }
    private UIManager _uiManager = null;
 
    public GameManager Initialize(int startLevel)
    {
        _topScore= ServiceLocator.Get<SaveSystem>().LoadBinary<int>("topScore");
        GameLoader.CallOnComplete(OnGameLoaderComplete);
        SetLevel(startLevel);
        return this;

    }

    private void OnGameLoaderComplete()
    {
        _uiManager = ServiceLocator.Get<UIManager>();

    }

    private void SetLevel(int level)
    {

        _currentLevevl = level;

    }

    private void LoadNextLevel()
    {
        int nextLevel = ++_currentLevevl;
        if (nextLevel > _killEnemiesByLevel.Count)
        {
            _uiManager.DisplayMessageText("YOU WIN");
            Time.timeScale = 0;
            return;
        }
        SceneManager.LoadScene(nextLevel);
        SetLevel(nextLevel);
        _numEnemyKilled = 0;
        _uiManager.DisplayMessageText("");
    }

    public void UpdateScore(int score)
    {
        _currentScore += score;
        _numEnemyKilled++;
        _uiManager.UpdateScoreDisplay(_currentScore);
       if(_currentScore>=_topScore)
        {
            ServiceLocator.Get<SaveSystem>().SaveBinary<int>(_currentScore,"topScore");
        }
        CheckWinCondition();
    }

    public void GainCoin()
    {
        _currentCoin++;
    }

    public void UpdateHealthBar(float health)
    {
        _uiManager.UpdateHealthBar(health);

    }
    public void SetHealthBar(float maxhealth)
    {

        _uiManager.SetHealthBar(maxhealth);
    }


    public void UpdateTime()
    {
        _uiManager.UPdateTimeDisplay();
    }

    public void UpdateMessageText(string text)
    {
        _uiManager.DisplayMessageText(text);
    }

    private void CheckWinCondition()
    {

        int numberRequiredToWin = _killEnemiesByLevel[_currentLevevl];
        if (_numEnemyKilled >= numberRequiredToWin)
        {
            _uiManager.DisplayMessageText("Level Completed");
            LoadNextLevel();
        }
    }

    public void Pause()
    {
        _uiManager.Pause();
    }

   
}
