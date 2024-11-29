using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SpellManager : MonoBehaviour
{

    public static SpellManager Instance { get; private set; }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<string> PurchaseSpell(string spellName)
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/purchase-spell";

        using UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{ \"spellName\": \"" + spellName + "\" }");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
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
        string response = request.downloadHandler.text.Trim();
        Debug.Log($"Response {response}");

        return response;
    }
}
