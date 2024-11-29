using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; } // can only set/modify in this class

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

    public async Task<string> IsLoggedIn()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/is-logged-in";

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
                return null;
            }

            string response = request.downloadHandler.text.Trim();
            response = response.Trim('"');

            return response;
        }
    }

    public async Task<bool> LoginUser(string username, string password)
    {
        string url = $"{GlobalConfig.baseURL}/api/login?username={username}&password={password}";

        using (UnityWebRequest request = new UnityWebRequest(url, "GET")) // using ensures removed after 
        {
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
                Debug.Log($"Error logging in user: {username}");
                return false;
            }

            return true;
        }
    }

    public async Task<bool> RegisterUser(string username, string password)
    {
        string url = $"{GlobalConfig.baseURL}/api/register-user";

        using UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{ \"username\": \"" + username + "\", \"password\": \"" + password + "\" }");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        request.SetRequestHeader("Content-Type", "application/json");

        // Send the web request 
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        // check for a response of 200 
        Debug.Log(request.error);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Error registering user: {username}");
            return false;
        }

        return true;
    }

    public async Task<string> GetUserCoins()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/get-coins";

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
                Debug.Log($"Error getting coins for the user");
                return null;
            }

            string response = request.downloadHandler.text.Trim();
            response = response.Trim('"');

            return response;
        }
    }

}
