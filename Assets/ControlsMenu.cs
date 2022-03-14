using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour {
    public GameObject controlsMenuUI;
    public void controlsAppear() {
        Time.timeScale = 0f;
        controlsMenuUI.SetActive(true);
    }
    public void CloseControls() {
        Debug.Log("Closing controls menu...");
        controlsMenuUI.SetActive(false);
    }
}
