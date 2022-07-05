using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    string gameVersion = "1";
    public bool isConnected = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError($"Multiple {GetType().Name} is not allow");
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinGameRoom(string roomName)
    {

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 6
        };
        
        PhotonNetwork.JoinOrCreateRoom(roomName,options,null);
    }

 #region Callback
    public override void OnConnected()
    {
        base.OnConnected();
        isConnected = true;
        Debug.Log("PUN is connected");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("PUN is connected to master");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected via {cause.ToString()}");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Successfully create a new room");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Created Room");
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            Debug.Log("Joined Room");
        }
    }

    #endregion Callback
}
