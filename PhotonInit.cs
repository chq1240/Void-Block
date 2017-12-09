using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInit : MonoBehaviour {
    public string version = "v1.0";
    public InputField userId;
    public InputField roomName;
    public GameObject scrollContents;
    public GameObject roomItem;
    public string scenename;

    void OnReceivedRoomListUpdate()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("ROOM_ITEM"))
        {
            Destroy(obj);
        }

        int rowCount = 0;
        scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContents.transform, false);
            Debug.Log(_room.name);

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.name;
            roomData.connectPlayer = _room.playerCount;
            roomData.maxPlayers = _room.maxPlayers;

            roomData.DispRoomData();

            roomData.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });

            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);
        }
    }
    void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.player.name = userId.text;
        PlayerPrefs.SetString("USER_ID",userId.text);
        PhotonNetwork.JoinRoom(roomName);
    }
    // Use this for initialization
    void Awake()
    {
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(version);
        }
        userId.text = GetUserId();
        roomName.text = "ROOM_" + Random.Range(0, 999).ToString("000");
    }
	void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby !");
        userId.text = GetUserId();
        //PhotonNetwork.JoinRandomRoom();
    }
    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID");
        if (string.IsNullOrEmpty(userId))
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        return userId;
    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No rooms !");
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 4;
        PhotonNetwork.CreateRoom("MyRoom", ro, TypedLobby.Default);
    }
    void OnJoinedRoom()
    {
        Debug.Log("Enter Room");
        //CreateTank();
        StartCoroutine(this.LoadBattleField());

    }
    IEnumerator LoadBattleField()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        AsyncOperation ao = Application.LoadLevelAsync(scenename);
        yield return ao;
    }
    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.player.name = userId.text;
        PlayerPrefs.SetString("USER_ID",userId.text);
        PhotonNetwork.JoinRandomRoom();
    }
    /*void CreateTank()
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }*/
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
    void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
    public void OnClickCreatRoom()
    {
        string _roomName = roomName.text;
        if(string.IsNullOrEmpty(roomName.text))
        {
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");
        }
        PhotonNetwork.player.name = userId.text;

        PlayerPrefs.SetString("USER_ID", userId.text);


        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(_roomName, ro, TypedLobby.Default);

    }

    void OnPhotonCreatRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Creat Room Failed = " + codeAndMsg[1]);
    }
}
