using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CanvasFunctions : MonoBehaviour
{
    public int mainMenuNum;
    public void DisconnectPlayer()
    {
        SceneManager.LoadScene(mainMenuNum);
        PhotonNetwork.Disconnect();
    }
}
