
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseButtonController : MonoBehaviour, IInteractiveButton
{
    private bool isPaused = false;
    public Sprite pauseIcon;
    public Sprite playIcon;
    private Image image;
    public AudioSource backgroundMusic; // Assign in Inspector OR find in Static Environment

    void Start()
    {
        image = GetComponent<Image>();

        // If backgroundMusic is not assigned manually, find it inside Static Environment
        if (backgroundMusic == null)
        {
            GameObject staticEnv = GameObject.Find("Static Environment");
            if (staticEnv != null)
            {
                backgroundMusic = staticEnv.GetComponent<AudioSource>();
            }
        }

        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background music AudioSource not found in Static Environment!");
        }
    }

    public void ButtonClick()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;

        if (isPaused)
        {
            Debug.Log("Game Paused");
            image.sprite = playIcon;
            if (backgroundMusic != null) backgroundMusic.Pause();
        }
        else
        {
            image.sprite = pauseIcon;
            if (backgroundMusic != null) backgroundMusic.UnPause();
        }
    }
}
