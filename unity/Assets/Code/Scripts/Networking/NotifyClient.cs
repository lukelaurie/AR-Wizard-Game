using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class NotifyClient : NetworkBehaviour
{
    [ClientRpc]
    public void JoinGameClientRpc()
    {
        if (!IsOwner)
            return;

        ScreenToggle.ToggleGameObjectWithTag(true, "GameBackground");
        EnableSpellShootingScript();

        // try to get the Game object from the scene
        try
        {
            GameObject joinRoomUI = GameObject.FindWithTag("JoinRoomUI");
            joinRoomUI.SetActive(false);
        }
        catch
        {
            Debug.Log("Look! No error is thrown because it was caught and this printed instead!!!!");
        }
        StartGameAr.StartNewGame();
    }

    [ClientRpc]
    public void PartyLoseGameClientRpc()
    {
        if (!IsOwner)
            return;

        ScreenToggle.ToggleGameObjectWithTag(false, "DeathBackground");
        ScreenToggle.ToggleGameObjectWithTag(false, "GameBackground");
        ScreenToggle.ToggleGameObjectWithTag(false, "SpellPanel");        

        var loseBackground = ScreenToggle.FindGameObjectWithTag("LoseBackground");
        loseBackground.SetActive(true);

        TMPro.TMP_Text childText = loseBackground.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
        Transform tryAgainButton = loseBackground.transform.Find("TryAgain");
        if (IsHost)
        {
            childText.text = "Try Again...";
            tryAgainButton.gameObject.SetActive(true);
        }
        else
        {
            childText.text = "Waiting For Host...";
            tryAgainButton.gameObject.SetActive(false);
        }
    }

    [ClientRpc]
    public void PartyWinGameClientRpc(string rewardsDictJson)
    {
        if (!IsOwner)
            return;
        ScreenToggle.ToggleGameObjectWithTag(false, "GameBackground");
        ScreenToggle.ToggleGameObjectWithTag(true, "WinBackground");
        var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

        Dictionary<string, int> rewards = JsonConvert.DeserializeObject<Dictionary<string, int>>(rewardsDictJson);

        var reward = rewards[clientData.GetUsername()];
        GameObject winBackground = GameObject.FindWithTag("WinBackground");
        TMPro.TMP_Text childText = winBackground.transform.GetChild(3).GetComponent<TMPro.TMP_Text>();
        childText.text = reward.ToString();
    }

    [ClientRpc]
    public void PlayerRestartGameClientRpc()
    {
        if (!IsOwner || IsHost)
            return;

        ScreenToggle.ToggleGameObjectWithTag(false, "LoseBackground");
        ScreenToggle.ToggleGameObjectWithTag(true, "JoinRoomUI");
    }

    [ClientRpc]
    public void PlayerGameStartedClientRpc()
    {
        GameObject gameBackground = GameObject.FindWithTag("GameBackground");
        TMPro.TMP_Text placeBossText = gameBackground.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();

        placeBossText.text = "";
        ScreenToggle.ToggleGameObjectWithTag(true, "SpellPanel");
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

}
