using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;

public class NotifyClient : NetworkBehaviour
{
    [ClientRpc]
    public void JoinGameClientRpc()
    {
        if (!IsOwner)
            return;

        ToggleGameObjectWithTag(true, "InitiateSpellUi");
        EnableSpellShootingScript();

        // If they are the host allow them to click to place a boss object 
        if (IsHost)
        {
            EnablePlacementScript();
            return;
        }

        // try to get the Game object from the scene 
        GameObject joinRoomUI = GameObject.FindWithTag("JoinRoomUI");
        joinRoomUI.SetActive(false);

        StartGameAr.StartNewGame();
    }

    [ClientRpc]
    public void PartyLoseGameClientRpc()
    {
        if (!IsOwner)
            return;
        ToggleGameObjectWithTag(false, "InitiateSpellUi");
        ToggleGameObjectWithTag(true, "LoseBackground");
    }

    [ClientRpc]
    public void PartyWinGameClientRpc(string rewardsDictJson)
    {
        if (!IsOwner)
            return;
        ToggleGameObjectWithTag(false, "InitiateSpellUi");
        ToggleGameObjectWithTag(true, "WinBackground");
        var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

        Dictionary<string, int> rewards = JsonConvert.DeserializeObject<Dictionary<string, int>>(rewardsDictJson);

        var reward = rewards[clientData.GetUsername()];
        Debug.Log($"Rewards: ");
        Debug.Log($"My reward {reward}");
        GameObject winBackground = GameObject.FindWithTag("WinBackground");
        TMPro.TMP_Text childText = winBackground.transform.GetChild(3).GetComponent<TMPro.TMP_Text>(); //TODO get of the name not the index
        childText.text = reward.ToString();
    }

    [ClientRpc]
    public void PlayerDieClientRpc()
    {
        if (!IsOwner)
            return;
        ToggleGameObjectWithTag(false, "InitiateSpellUi");
        ToggleGameObjectWithTag(true, "DeathBackground");
    }

    private void EnablePlacementScript()
    {
        var placeBoss = GameObject.FindWithTag("GameLogic").GetComponent<PlaceCharacter>();

        if (placeBoss == null)
        {
            Debug.Log("Unable to find boss script");
            return;
        }

        placeBoss.enabled = true;
        return;
    }

    private void EnableSpellShootingScript()
    {
        var playerShoot = GameObject.FindWithTag("GameLogic").GetComponent<PlayerShoot>();

        if (playerShoot == null)
        {
            Debug.Log("Unable to find shoot script");
            return;
        }

        playerShoot.enabled = true;
        return;
    }

    private void ToggleGameObjectWithTag(bool on, string tag)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag))
            {
                obj.SetActive(on);
            }
        }
    }


}
