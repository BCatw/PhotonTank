using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{
    static MainMenu Instance;
    private GameObject UIObj;
    private Button joinGameBtn;

    string UI_s = "UI";
    string joinGameBtn_s = "Btn_JoinRoom";


    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        UIObj = transform.FindAnyChild<Transform>(UI_s).gameObject;
        joinGameBtn = transform.FindAnyChild<Button>(joinGameBtn_s);

        UIObj.SetActive(true);
        joinGameBtn.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        joinGameBtn.interactable = true;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        UIObj.SetActive(!PhotonNetwork.InRoom);
    }
}