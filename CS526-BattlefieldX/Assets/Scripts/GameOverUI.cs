using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    private ScoreManager theScoreManager;
    AudioManager audioManager;
    string buttonPressSound = "ButtonPress";
    void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("Sound Error");
        }
        theScoreManager = FindObjectOfType<ScoreManager>();
    }

    public void Quit()
    {
        audioManager.PlaySound(buttonPressSound);
        Debug.Log("APPLICATION QUIT");
        Application.Quit();
    }

    public void Retry()
    {
        theScoreManager.scoreCount = 0;
        theScoreManager.scoreIncreasing = true;
        audioManager.PlaySound(buttonPressSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
