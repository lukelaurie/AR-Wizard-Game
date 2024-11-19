using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    // ensure that this is a singleton object 
    public static RoomManager Instance { get; private set;} // can only set/modify in this class

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // create a new room on the go server 
    public string CreateRoom()
    {
        Debug.Log(GlobalConfig.baseURL);
        int x = 1000;
        return x.ToString();
    }
}
