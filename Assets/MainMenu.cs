using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject mainMenuUI;

    void Start() {
        Time.timeScale = 0f;
        mainMenuUI.SetActive(true);
    }

    public void startAppear() {
        Time.timeScale = 0f;
        mainMenuUI.SetActive(true);
    }

    public void StartGame() {
        Time.timeScale = 1f;
        mainMenuUI.SetActive(false);
    }

    public void LoadOptionsMenu() {
        Debug.Log("Loading options...");
        mainMenuUI.SetActive(false);
    }

    public void QuitGame() {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
