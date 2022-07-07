using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static T FindAnyChild<T>(this Transform trans, string name) where T : Component
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            if (trans.childCount > 0)
            {
                Transform child = trans.GetChild(i).FindAnyChild<Transform>(name);
                if (child != null) return child.GetComponent<T>();
            }

            if (trans.GetChild(i).name == name) return trans.GetChild(i).GetComponent<T>();
        }

        return default;
    }
}
