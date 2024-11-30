using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            Debug.Log($"Error buying spell: {request.error}");
            return "Error buying spell";
        }

        // parse the response 
        string response = request.downloadHandler.text.Trim();
        Debug.Log($"Response {response}");

        return "Spell purchased";
    }

    public async Task<string> UpgradeSpell(string spellName)
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/upgrade-spell";

        using UnityWebRequest request = new UnityWebRequest(url, "PUT");
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
            Debug.Log($"Error buying spell: {request.error}");
            return "Error buying spell";
        }

        // parse the response 
        string response = request.downloadHandler.text.Trim();
        Debug.Log($"Response {response}");

        return "Spell upgraded";
    }

    public async Task<Dictionary<string, int>> GetSpells()
    {
        string url = $"{GlobalConfig.baseURL}/api/protected/get-spells";

        using UnityWebRequest request = new UnityWebRequest(url, "GET");

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
        string json = request.downloadHandler.text;
        Debug.Log($"JSON {json}");
        var spellMap = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        Debug.Log($"MAP {spellMap}");
        return spellMap;
    }
}
