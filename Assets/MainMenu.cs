// Samuel Rouillard, Tristan Caetano, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject mainMenuUI;

    // Enabling start menu on game start.
    void Start() {
        Time.timeScale = 0f;
        mainMenuUI.SetActive(true);
    }

    // Function to enable start from a button.
    public void startAppear() {
        Time.timeScale = 0f;
        mainMenuUI.SetActive(true);
    }

    // "Unpausing" the game to play.
    public void StartGame() {
        Time.timeScale = 1f;
        mainMenuUI.SetActive(false);
    }

    // Close start menu and open options.
    public void LoadOptionsMenu() {
        Debug.Log("Loading options...");
        mainMenuUI.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Click");
    }

    // Quitting the game all together.
    public void QuitGame() {
        Debug.Log("Quitting game...");
        FindObjectOfType<AudioManager>().Play("Click");
        Application.Quit();
    }
}