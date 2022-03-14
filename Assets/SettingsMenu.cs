using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

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
        Debug.Log("Closing options menu...");

        // Loading the title screen scene
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenControls() {
        Debug.Log("Opening controls menu...");

        // Loading the title screen scene
        SceneManager.LoadScene("ControlsMenu");
    }
}
