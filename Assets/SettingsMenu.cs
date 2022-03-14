// Samuel Rouillard, Tristan Caetano, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    public GameObject settingsMenuUI;

    Resolution[] resolutions;

    // Passing found resolutions to an array and saving selected value.
    void Start() {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Enabling the options menu.
    public void settingsAppear() {
        Time.timeScale = 0f;
        settingsMenuUI.SetActive(true);
    }

    // Finding compatible resolutions for monitor.
    public void SetResolution (int resolutionIndex) {
        FindObjectOfType<AudioManager>().Play("Click");
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

//    public void SetMaster (float master) {
//        audioMixer.SetFloat("masterVolume", master);
//    }

    // Float for sound effects slider value.
    public void SetSound (float sound) {
        audioMixer.SetFloat("soundVolume", sound);
    }

    // Float for music slider value.
    public void SetMusic (float music) {
        audioMixer.SetFloat("musicVolume", music);
    }

    // Boolean for full screen activation.
    public void SetFullScreen (bool isFullscreen) {
        FindObjectOfType<AudioManager>().Play("Click");
        Screen.fullScreen = isFullscreen;
    }

    // Closing options menu.
    public void CloseOptions() {
        Time.timeScale = 0f;
        Debug.Log("Closing options menu...");
        settingsMenuUI.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Click");
    }

    // Opening the controls menu.
    public void OpenControls() {
        Time.timeScale = 0f;
        Debug.Log("Opening controls menu...");
        settingsMenuUI.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Click");
    }
}