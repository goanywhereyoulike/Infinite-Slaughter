using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayhud : MonoBehaviour
{
    [SerializeField]
    private Text scoresText=null;

    [SerializeField]
    private Text timeText=null;

    [SerializeField]
    private Text MessageText=null;


    [SerializeField]
    private Slider healthbar = null;

    [SerializeField]
    private Image Panel = null;  

    [SerializeField]
    private Button Resumebutton = null;

    [SerializeField]
    private Button Quitbutton = null;


    bool GamePause = false;
    public void Initialize()
    {
        Panel.gameObject.SetActive(false);
        MessageText.text = string.Empty;
        scoresText.text = "Score: " + "0";
        timeText.text = "Time: " + "0";
        healthbar.maxValue = 1;
        healthbar.value = 1;
        Resumebutton.onClick.AddListener(Resume);
        Quitbutton.onClick.AddListener(QuitGame);
    }

    public void SetGameplayHUDActive(bool shouldbeActive)
    {
        gameObject.SetActive(shouldbeActive);

    }

    public void UpdateScores(int currentScore)
    {

        scoresText.text = "Score: " + currentScore.ToString();

    }

    public void UpdateMessageText(string text)
    {
        MessageText.gameObject.SetActive(true);
        MessageText.text = text;

    }

    public void UpdateTimeText(float time)
    {
        timeText.text = "Time: " + time;

    }
    public void SetHealthBar(float maxHealth)
    {
        healthbar.maxValue = maxHealth;

    }
    public void UpdateHealthBar(float health)
    {

        healthbar.value = health;
    }
    public void Resume()
    {
        Panel.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GamePause = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

   public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Panel.gameObject.SetActive(true);
        Time.timeScale = 0f;
        GamePause = true;
    }

    public void QuitGame()
    {
        Application.Quit();

    }
}
