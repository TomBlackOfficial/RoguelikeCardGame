using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public bool isMainMenu = false;
    public bool isGamePaused = false;
    public GameObject PauseMenuUI;
    public GameObject mainMenuPanel; // Reference the main menu panel with buttons in Inspector
    public GameObject creditsPanel; // Reference the disabled credits panel in Inspector

    private void Start()
    {
        if (PauseMenuUI != null)
            PauseMenuUI.SetActive(false);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true); // Show main menu on start
        if (creditsPanel != null) // Ensure credits panel reference is valid
            creditsPanel.SetActive(false); // Hide credits panel on start
    }

    private void Update()
    {
        if (PauseMenuUI != null)
        {
            HandlePauseMenu();
        }
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }

    public void HandlePauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isMainMenu)
        {
            if (!isGamePaused)
            {
                Debug.Log("Paused");
                PauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
                isGamePaused = true;
            }
            else
            {
                Debug.Log("UnPaused");
                PauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                isGamePaused = false;
            }
        }
    }

    public void PlayButton()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void ResumeButton()
    {
        Debug.Log("UnPaused");
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditButton()
    {
        ShowCreditsPanel();
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        if (isGamePaused)
        {
            PauseMenuUI.SetActive(false);
            isGamePaused = false;
        }
        StartCoroutine(LoadLevel(0));
    }

    public void ShowCreditsPanel()
    {
        if (creditsPanel != null) // Check if credits panel reference is valid
        {
            creditsPanel.SetActive(!creditsPanel.activeSelf); // Toggle credits visibility
            if (creditsPanel.activeSelf) // If credits are active
            {
                mainMenuPanel.SetActive(false); // Disable main menu panel
            }
            else // If credits are not active (hidden)
            {
                mainMenuPanel.SetActive(true); // Enable main menu panel
            }
        }
    }
}
