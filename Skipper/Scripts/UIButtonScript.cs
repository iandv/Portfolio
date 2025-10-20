using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex;
    [SerializeField]
    GameObject nextMenu, currentMenu;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1;
    }

    public void NextMenu()
    {
        nextMenu.SetActive(true);
        currentMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
