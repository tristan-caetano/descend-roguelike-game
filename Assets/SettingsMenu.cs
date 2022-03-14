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

    public void settingsAppear() {
        Time.timeScale = 0f;
        settingsMenuUI.SetActive(true);
    }

    public void SetResolution (int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMaster (float master) {
        audioMixer.SetFloat("masterVolume", master);
    }
    public void SetSound (float sound) {
        audioMixer.SetFloat("soundVolume", sound);
    }

    public void SetMusic (float music) {
        audioMixer.SetFloat("musicVolume", music);
    }

    public void SetFullScreen (bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void CloseOptions() {
        Time.timeScale = 0f;
        Debug.Log("Closing options menu...");
        settingsMenuUI.SetActive(false);
    }

    public void OpenControls() {
        Time.timeScale = 0f;
        Debug.Log("Opening controls menu...");
        settingsMenuUI.SetActive(false);
    }
}
