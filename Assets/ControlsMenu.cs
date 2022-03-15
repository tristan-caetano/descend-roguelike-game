// Samuel Rouillard, Tristan Caetano, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour {
    public GameObject controlsMenuUI;

    // Enabling the controls menu from a button.
    public void controlsAppear() {
        Time.timeScale = 0f;
        controlsMenuUI.SetActive(true);
    }

    // Disabling the controls menu.
    public void CloseControls() {
        Debug.Log("Closing controls menu...");
        controlsMenuUI.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Click");
    }
}