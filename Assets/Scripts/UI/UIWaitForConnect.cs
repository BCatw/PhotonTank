using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWaitForConnect : MonoBehaviour
{
    public Button joinRoomBtn;
    GameManager manager;

    private void Awake()
    {
        manager = GameManager.instance;
    }

    private void Update()
    {
        joinRoomBtn.interactable = manager.isConnected;
    }
}
