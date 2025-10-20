using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPun
{
    public static GameManager instance;
    private Image _myLobbyKnob, _enemyLobbyKnob;
    private TextMeshProUGUI _myScoreText, _enemyScoreText;
    public int _myScoreNum, _enemyScoreNum, _playersReady;
    public bool _isWaiting = false;

    private Player _player;

    private void Awake()
    {
        instance = this;
    }

    public void JoinGame(Player p)
    {
        int lenght = PhotonNetwork.PlayerList.Length;

        if (lenght == 1)
        {
            _isWaiting = true;
            _player = p;
            _player.endGame = true;
            p.ChangePosition(GameManagerData.instance.spawnPositionOne);
            p.ChangeSkin(lenght);
            GameManagerData.instance.goalPlayerTwo.SetActive(false);
            _myScoreText = GameManagerData.instance.playerOneScore;
            _enemyScoreText = GameManagerData.instance.playerTwoScore;
            _myLobbyKnob = GameManagerData.instance.playerOneLobbyKnob;
            _enemyLobbyKnob = GameManagerData.instance.playerTwoLobbyKnob;
        }

        if (lenght == 2)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                GameManagerData.instance.goalPlayerOne.SetActive(false);
                _myScoreText = GameManagerData.instance.playerTwoScore;
                _enemyScoreText = GameManagerData.instance.playerOneScore;
                _myLobbyKnob = GameManagerData.instance.playerTwoLobbyKnob;
                _enemyLobbyKnob = GameManagerData.instance.playerOneLobbyKnob;
            }
            p.ChangePosition(GameManagerData.instance.spawnPositionTwo);
            p.ChangeSkin(lenght);
            GameManagerData.instance.playerTwoLobbyTxt.gameObject.SetActive(true);
            GameManagerData.instance.playerTwoLobbyKnob.gameObject.SetActive(true);
        }
    }

    public void ReadyGame()
    {
        int lenght = PhotonNetwork.PlayerList.Length;
        if (lenght < 2) return;

        _myLobbyKnob.color = Color.green;
        GameManagerData.instance.readyButton.SetActive(false);
        photonView.RPC("RPC_PlayerReady", RpcTarget.OthersBuffered);
        _playersReady++;

        if (_playersReady >= 2)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        if (_player != null) _player.endGame = false;
        GameManagerData.instance.waitForPlayerScreen.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            ItemSpawner.instance.StartSpawner();
        }
    }

    public void CubeInsideGoal()
    {
        photonView.RPC("RPC_PlayerScored", RpcTarget.OthersBuffered);
        _myScoreNum++;
        _myScoreText.text = "" + _myScoreNum;
        if (_myScoreNum >= GameManagerData.instance.pointsNeededToWin)
        {
            EventManager.Instance.Trigger("Win");
            photonView.RPC("RPC_PlayerWon", RpcTarget.OthersBuffered);
        }
    }

    public void CubeOutSideGoal()
    {
        photonView.RPC("RPC_PlayerLostPoints", RpcTarget.OthersBuffered);
        _myScoreNum--;
        _myScoreText.text = "" + _myScoreNum;
    }

    [PunRPC]
    void RPC_PlayerScored()
    {
        _enemyScoreNum++;
        _enemyScoreText.text = "" + _enemyScoreNum;
    }

    [PunRPC]
    void RPC_PlayerLostPoints()
    {
        _enemyScoreNum--;
        _enemyScoreText.text = "" + _enemyScoreNum;
    }

    [PunRPC]
    void RPC_PlayerWon()
    {
        EventManager.Instance.Trigger("Lose");
    }

    [PunRPC]
    void RPC_PlayerReady()
    {
        _enemyLobbyKnob.color = Color.green;
        _playersReady++;

        if (_playersReady >= 2)
        {
            StartGame();
        }
    }
}
