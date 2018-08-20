using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    string pressButtonSound = "ButtonPress";

    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audio");
        }
    }

    public void StartGame()
    {
        audioManager.PlaySound(pressButtonSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        audioManager.PlaySound(pressButtonSound);
        Debug.Log("We Quit");
        Application.Quit();
    }

    public void optionMenu()
    {
        audioManager.PlaySound(pressButtonSound);
    }
	
}
