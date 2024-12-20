using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class RoomManager : MonoBehaviour
{

    // ensure that this is a singleton object 
    public static RoomManager Instance { get; private set; } // can only set/modify in this class

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

        using (UnityWebRequest request = new UnityWebRequest(url, "POST")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");

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

        using (UnityWebRequest request = new UnityWebRequest(url, "GET")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");
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
                Debug.LogError($"Error joining room: {request.downloadHandler.text}");
                return false;
            }

            return true;
        }
    }

    // get all the players in the current room 
    public async Task<string[]> GetPlayersInRoom()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/get-room";

        using (UnityWebRequest request = new UnityWebRequest(url, "GET")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");

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
                Debug.LogError($"Error getting players in room: {request.error}");
                return null;
            }

            string response = request.downloadHandler.text.Trim();

            // try to deserialize the object into a list of strings
            try
            {
                string[] players = JsonConvert.DeserializeObject<string[]>(response);
                return players;
            }
            catch (JsonException)
            {
                Debug.LogError("Error parsing the json");
                return null;
            }
        }
    }

    public async Task<bool> StartGameInRoom()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/start-game";

        using (UnityWebRequest request = new UnityWebRequest(url, "GET")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");

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
                Debug.LogError($"Error starting room: {request.error}");
                return false;
            }
            return true;

        }
    }

    public async Task<bool> IsGameStarted()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/is-game-started";

        using (UnityWebRequest request = new UnityWebRequest(url, "GET")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");

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
                return false;
            }

            return true;
        }
    }

    public async Task<string> EndGame(bool winStatus, int level)
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/end-game";

        using UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{ \"winStatus\": " + winStatus.ToString().ToLower() + ", \"level\": " + level + " }");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the web request 
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        // check for a response of 200 
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error ending game, status code: {request.result}");
            return null;
        }
        return request.downloadHandler.text;

    }

    public async Task<bool> LeaveGame()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/leave-game";

        using (UnityWebRequest request = new UnityWebRequest(url, "GET")) // using ensures removed after 
        {
            request.SetRequestHeader("Content-Type", "application/json");

            // allows the response to be stored

            // Send the web request 
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            // check for a response of 200 
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error deleting the user from the game");
                return false;
            }

            return true;
        }
    }
}