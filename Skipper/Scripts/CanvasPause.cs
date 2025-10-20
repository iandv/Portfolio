using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPause : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private List<GameObject> hideWhenClosed, showWhenClosed;

    private bool _paused;

    public static CanvasPause Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"Duplicated CanvasPause found in GameObject: {gameObject.name}");
            Destroy(this);
        }
        _paused = false;
    }

    public void PauseGame()
    {
        _paused =! _paused;

        if (_paused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
        }

        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            foreach (var item in hideWhenClosed)
                item.SetActive(false);
            foreach (var item in showWhenClosed)
                item.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }
}
