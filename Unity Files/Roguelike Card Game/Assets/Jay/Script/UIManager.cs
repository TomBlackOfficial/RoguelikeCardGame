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

    private void Start()
    {
        PauseMenuUI.SetActive(false);
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
        StartCoroutine(LoadLevel(2));
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
        StartCoroutine(LoadLevel(1));
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

    public void PauseMenu()
    {
        
    }
}
