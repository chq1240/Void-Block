using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    public Text txtConnect;
    public Text txtLogMsg;
    private PhotonView pv;
    private bool talking = false;
    public InputField userwords;
    public GameObject wordsblock;
    public Transform boreposA;
    public Transform boreposB;
    public bool gamestart = false;

    // Use this for initialization
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pv = GetComponent<PhotonView>();
        CreateRobot();
        PhotonNetwork.isMessageQueueRunning = true;
        GetConnectPlayerCount();
        Cursor.visible = false;
        wordsblock.SetActive(false);
        userwords.enabled = false;
    }
    IEnumerator Start()
    {
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.name + "] Connected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
        yield return new WaitForSeconds(1.0f);
        SetConnectPlayerScore();
    }
    
    void SetConnectPlayerScore()
    {
        PhotonPlayer[] players = PhotonNetwork.playerList;
        foreach (PhotonPlayer _player in players)
        {
            Debug.Log("[" + _player.ID + "]" + _player.name + " " + _player.GetScore() + "KILL");
        }
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");
        foreach (GameObject tank in tanks)
        {
            int currKillCount = tank.GetComponent<PhotonView>().owner.GetScore();
            tank.GetComponent<TankDamage>().txtKillCount.text = currKillCount.ToString();
        }
    }

    void GetConnectPlayerCount()
    {
        Room currRoom = PhotonNetwork.room;
        txtConnect.text = currRoom.playerCount.ToString() + "/" + currRoom.maxPlayers.ToString();
    }
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        GetConnectPlayerCount();
    }
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {
        GetConnectPlayerCount();
    }
    void CreateRobot()
    {
        Room currRoom = PhotonNetwork.room;
        if(currRoom.playerCount < 3)
            PhotonNetwork.Instantiate("PlayerA", boreposA.position, Quaternion.identity, 0);
        else
            PhotonNetwork.Instantiate("PlayerB", boreposB.position, Quaternion.identity, 0);
        if (currRoom.playerCount == 4)
        {
            currRoom.IsOpen = false;
            gamestart = true;
        }
    }

    
    public void OnClickExitRoom()
    {
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.name + "] DisConnected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
        PhotonNetwork.LeaveRoom();
    }

    public void OnSendWords()
    {
        if (userwords.text != "请输入....")
        {
            string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.name + "] </color> : " + userwords.text;
            pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
            userwords.text = "请输入....";
        }
    }
    [PunRPC]
    void LogMsg(string msg)
    {
        txtLogMsg.text = txtLogMsg.text + msg;
    }
    void OnLeftRoom()
    {
        Application.LoadLevel("blockLobby");
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Cursor.visible)
            {
                Debug.Log("Cursor.visible: ture");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                wordsblock.SetActive(true);
                userwords.enabled = true;
                OnSendWords();
            }
            else
            {
                Cursor.visible = false;
                Debug.Log("Cursor.visible: false");
                Cursor.lockState = CursorLockMode.Locked;
                wordsblock.SetActive(false);
                userwords.enabled = false;
                OnSendWords();
            }
        }
    }
}
