using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public GameObject currentMenu;
    public GameObject nextMenu;
    public string eventTrigger;

    public void ChangeToTheNextLevelByNumber(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeToNextMenu()
    {
        currentMenu.SetActive(false);
        nextMenu.SetActive(true);
    }

    public void Trigger()
    {
        OEventManager.Instance.Trigger(eventTrigger);
    }
}
