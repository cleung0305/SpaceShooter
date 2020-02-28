using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] string menuScene;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameManager gameManager;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        UpdateSliders();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ShowOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void HideOptions()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Destroy(gameManager);
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeMusicVolume(Slider slider)
    {
        gameManager.SetMusicVolume(slider.value);
    }

    public void ChangeSFXVolume(Slider slider)
    {
        gameManager.SetSFXVolume(slider.value);
    }

    void UpdateSliders()
    {
        musicSlider.value = gameManager.GetMusicVolume();
        sfxSlider.value = gameManager.GetSFXVolume();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void SetMouseSensitivity(TMP_InputField slider)
    {
        gameManager.SetMouseSensitivity(slider);
    }

    public void SetScrollSpeed(TMP_InputField slider)
    {
        gameManager.SetScrollSpeed(slider);
    }
}
