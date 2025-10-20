using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool levelOne, levelTwo;
    [SerializeField]
    GameObject endOfTheLevel;

    public int itemsNeeded;
    public float pupUpTime;
    public Text objectiveText, popUp;
    public string objectiveOne, objectiveTwo;
    public string[] objectivesText;
    int _itemsCollected;
    bool _weightsObj, _turretObj, _towerObj;

    public static GameManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (levelOne)
        {
            EventManager.Instance.Subscribe("GotItem", CheckItemsObtainedLevelOne);
            objectiveText.text = objectiveOne + _itemsCollected.ToString() + "/" + itemsNeeded.ToString();
        }

        if (levelTwo)
        {
            objectiveText.text = "Activate each Tower";
            EventManager.Instance.Subscribe("PressedPlate", AddCountToCollected);
            EventManager.Instance.Subscribe("LeftPlate", RemoveCountToCollected);
        }
    }

    public void AddCountToNeeded()
    {
        itemsNeeded++;
    }

    public void TurretObjective()
    {
        _turretObj = true;
        WaitCoroutine(objectivesText[1]);
        CheckTowers();
    }

    public void TowerObjective()
    {
        _towerObj = true;
        WaitCoroutine(objectivesText[2]);
        CheckTowers();
    }

    void CheckTowers()
    {
        if (_weightsObj && _turretObj && _towerObj)
        {
            objectiveText.text = "Go to the city plaza";
            endOfTheLevel.SetActive(true);
        }
    }

    private void AddCountToCollected(params object[] objects)
    {
        if (!_weightsObj)
        {
            _itemsCollected++;
            if (_itemsCollected >= itemsNeeded)
            {
                _weightsObj = true;
                CheckTowers();
                StartCoroutine(WaitCoroutine(objectivesText[0]));
            }
        }
    }

    private void  RemoveCountToCollected(params object[] objects)
    {
        if (!_weightsObj)
        {
            _itemsCollected--;
        }
    }

    void CheckItemsObtainedLevelOne(params object[] parameters)
    {
        _itemsCollected++;
        objectiveText.text = objectiveOne + _itemsCollected.ToString() + "/" + itemsNeeded.ToString();
        if (_itemsCollected >= itemsNeeded)
        {
            EventManager.Instance.Trigger("Open");
            objectiveText.text = objectiveTwo;
        }
    }

    IEnumerator WaitCoroutine(string text)
    {
        popUp.text = text;
        yield return new WaitForSeconds(pupUpTime);
        popUp.text = "";
    }
}
