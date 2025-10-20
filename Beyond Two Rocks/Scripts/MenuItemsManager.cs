using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemsManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject winMenu;

    private bool _paused;
    private bool _gameOver;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _paused = false;
        _gameOver = false;
    }

    void Start()
    {
        OEventManager.Instance.Subscribe("OnPause", OnPause);
        OEventManager.Instance.Subscribe("OnPlayerDeath", OnPlayerDeath);
        OEventManager.Instance.Subscribe("OnVictory", OnVictory);
    }

    void OnPause(params object[] parameters)
    {
        _paused = !_paused;

        if (!_gameOver)
        {
            if (_paused)
            {
                Cursor.lockState = CursorLockMode.None;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }       
    }

    void OnPlayerDeath(params object[] parameters)
    {
        _gameOver = true;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnVictory(params object[] parameters)
    {
        _gameOver = true;
        winMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
}
