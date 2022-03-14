using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    PauseMenu pauseInstance;

    void Start() {
        Time.timeScale = 0f;
        pauseInstance = pauseMenuUI.GetComponent<PauseMenu>();
        pauseInstance.GameIsPaused = true;
    }

    public void StartGame() {
        Time.timeScale = 1f;
        pauseInstance.GameIsPaused = false;
        mainMenuUI.SetActive(false);
    }

    public void LoadOptionsMenu() {
        Debug.Log("Loading options...");

        // Loading the options scene
        SceneManager.LoadScene("Options");
    }

    public void QuitGame() {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
