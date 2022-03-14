using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    public void CloseControls() {
        Debug.Log("Closing controls menu...");

        // Loading the title screen scene
        SceneManager.LoadScene("Options");
    }
}
