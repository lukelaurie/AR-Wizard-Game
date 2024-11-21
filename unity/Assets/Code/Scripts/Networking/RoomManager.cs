using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

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
    public async Task<string> CreateRoom()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/create-room";
        string cookieToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MzIzMjExMDcsInVzZXJuYW1lIjoidXNlcjQifQ.Vh6BDKjrJ9dhAdvsUkaNRU6mZDEqXDJQ3ilmbvYXiek";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Cookie", $"token={cookieToken}");

            // allows the response to be stored
            request.downloadHandler = new DownloadHandlerBuffer();

            // Send the web request 
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            // check for a response of 200 
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error creating room: {request.error}");
                return null;
            }

            // parse the response 
            string roomID = request.downloadHandler.text.Trim();
            Debug.Log($"Room with ID: {roomID} created successfully");

            return roomID;
        }
    }

    // try joining a room from the id from the user 
    public async Task<bool> JoinRoom(string roomId)
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/join-room?roomNumber={roomId}";
        string cookieToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MzIzMjE5OTQsInVzZXJuYW1lIjoidXNlcjMifQ.s0zyOnT4T1q5592MKPxhEcwphazkSN8W1y1c0-MBCWQ";

        using (UnityWebRequest request = new UnityWebRequest(url, "GET")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Cookie", $"token={cookieToken}");

            // Send the web request 
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            // check for a response of 200 
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error joining room: {request.error}");
                return false;
            }

            return true;
        }
    }

    // get all the players in the current room 
    public async Task<string[]> GetPlayersInRoom()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/get-room";
        string cookieToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MzIzMjExMDcsInVzZXJuYW1lIjoidXNlcjQifQ.Vh6BDKjrJ9dhAdvsUkaNRU6mZDEqXDJQ3ilmbvYXiek";

        using (UnityWebRequest request = new UnityWebRequest(url, "GET")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Cookie", $"token={cookieToken}");

            // allows the response to be stored
            request.downloadHandler = new DownloadHandlerBuffer();

            // Send the web request 
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            // check for a response of 200 
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error creating room: {request.error}");
                return null;
            }

            string response = request.downloadHandler.text.Trim();
            Debug.Log($"Players in room: {response}");

            // try to deserialize the object into a list of strings
            try 
            {
                string[] players = JsonConvert.DeserializeObject<string[]>(response); 
                return players;
            }
            catch (JsonException ex)
            {
                Debug.LogError("Error parsing the json");
                return null;
            }
        }
    }
}