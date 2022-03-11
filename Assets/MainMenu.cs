using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

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
