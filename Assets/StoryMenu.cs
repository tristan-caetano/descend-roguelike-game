using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMenu : MonoBehaviour
{
    public GameObject storyMenuUI;

    // Enabling the controls menu from a button.
    public void storyAppear() {
        Time.timeScale = 0f;
        storyMenuUI.SetActive(true);
    }

    // Disabling the controls menu.
    public void CloseStory() {
        Debug.Log("Closing story menu...");
        storyMenuUI.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Click");
    }
}
