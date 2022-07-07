using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public static MainMenu Instance;
    private GameObject UIObj;
    private Button joinGameBtn;

    private InputField accountInput;
    private Button loginBtn;
    private Text text;

    string UI_s = "UI";
    string joinGameBtn_s = "Btn_JoinRoom";
    string loginBtn_s = "Btn_Login";
    string accountInput_s = "InputField_Account";
    string text_s = "Info";

    string loginSuccess = "<color=green> Login Success! Welcome {0}! </color>";
    string pleaseEnterID = "<color=yellow> Please enter ID </color>";
    string loginFailed = "<color=red> Login Failed </color>";

    private void Awake()
    {
        //Singletone實作
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        UIObj = transform.FindAnyChild<Transform>(UI_s).gameObject;
        joinGameBtn = transform.FindAnyChild<Button>(joinGameBtn_s);
        loginBtn = transform.FindAnyChild<Button>(loginBtn_s);
        accountInput = transform.FindAnyChild<InputField>(accountInput_s);
        text = transform.FindAnyChild<Text>(text_s);


        ResetUI();
    }

    void ResetUI()
    {
        UIObj.SetActive(true);
        accountInput.gameObject.SetActive(true);
        loginBtn.gameObject.SetActive(true);
        joinGameBtn.gameObject.SetActive(true);

        text.text = "";

        accountInput.interactable = true;
        loginBtn.interactable = true;
        joinGameBtn.interactable = false;
    }

    //處理登入流程
    public void Login()
    {
        if (string.IsNullOrEmpty(accountInput.text))
        {
            Debug.LogWarning("Please enter ID");
            text.text = pleaseEnterID;
            return;
        }

        accountInput.interactable = false;
        loginBtn.interactable = false;

        if (!GameManager.instance.ConnectToServer(accountInput.text))
        {
            Debug.LogError("Connect Failed");
            text.text = loginFailed;
        }
    }

    public override void OnConnectedToMaster()
    {
        accountInput.interactable = false;
        loginBtn.interactable = false;
        text.text = string.Format(loginSuccess, accountInput.text);
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