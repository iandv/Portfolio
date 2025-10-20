using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerData : MonoBehaviour
{
    public static GameManagerData instance;
    public Transform spawnPositionOne, spawnPositionTwo;
    public GameObject goalPlayerOne, goalPlayerTwo, waitForPlayerScreen, readyButton;
    public TextMeshProUGUI playerOneScore, playerTwoScore, goalScore, playerTwoLobbyTxt;
    public Image playerOneLobbyKnob, playerTwoLobbyKnob;
    public int pointsNeededToWin;

    private void Awake()
    {
        instance = this;
        goalScore.text = "" + pointsNeededToWin;
    }
}
