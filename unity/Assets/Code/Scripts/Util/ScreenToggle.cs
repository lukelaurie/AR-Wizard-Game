using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenToggle : MonoBehaviour
{
    public static void ToggleGameObjectWithTag(bool on, string tag)
    {
        var obj = FindGameObjectWithTag(tag);
        obj.SetActive(on);
    }

    public static GameObject FindGameObjectWithTag(string tag)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }
        return null;
    }
}
