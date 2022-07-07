using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    static HUD Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
