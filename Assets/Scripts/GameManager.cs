using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public static GameObject localPlayer;
    private GameObject defaultSpawnPoint;

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

        //j創建預設生存點
        defaultSpawnPoint = new GameObject("Default Spanw Point");
        defaultSpawnPoint.transform.position = new Vector3(0, 0, 0);
        defaultSpawnPoint.transform.SetParent(transform, false);
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        //PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.AutomaticallySyncScene = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public bool ConnectToServer(string account)
    {
        PhotonNetwork.NickName = account;
        return PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinGameRoom(string roomName)
    {

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 6
        };
        
        PhotonNetwork.JoinOrCreateRoom(roomName,options,null);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //判斷是否正在房間，
        if (!PhotonNetwork.InRoom) return;

        ////生成玩家物件
        //localPlayer = PhotonNetwork.Instantiate("TankPlayer", new Vector3(0, 0, 0), Quaternion.identity);
        //Debug.Log($"Plyaer Instance ID: {localPlayer.GetInstanceID()}");

        //將生成玩家物件，加入隨機生成點功能
        var spawnPosit = GetRandomSpawnPoint();
        localPlayer = PhotonNetwork.Instantiate("TankPlayer", spawnPosit.position, Quaternion.identity);

    }

    public static List<GameObject> GetAllGameObjOfTypeInScene<T>()
    {
        List<GameObject> objsInScene = new List<GameObject>();
        foreach (GameObject obj in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject))) 
        {
            if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave) continue;
            if (obj.GetComponent<T>() != null) objsInScene.Add(obj);
        }
        return objsInScene;
    } 

    private Transform GetRandomSpawnPoint()
    {
        //抓取所有含有SpawnPint Component物件
        List<GameObject> spawnpoints = GetAllGameObjOfTypeInScene<SpawnPoint>();
        //依照是否有抓取成功，回傳預設復活點或是隨機取一復活點
        return spawnpoints.Count == 0 ? defaultSpawnPoint.transform : spawnpoints[Random.Range(0, spawnpoints.Count)].transform;
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
        }
        else
        {
            Debug.Log("Joined Room");
        }
        PhotonNetwork.LoadLevel("GameScene");
    }

    #endregion Callback
}
