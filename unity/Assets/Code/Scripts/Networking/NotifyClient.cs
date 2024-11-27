using System;
using Unity.Netcode;
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
        ToggleGameObjectWithTag(false, "InitiateSpellUi");
        ToggleGameObjectWithTag(true, "LoseBackground");
    }

    [ClientRpc]
    public void PartyWinGameClientRpc()
    {
        ToggleGameObjectWithTag(false, "InitiateSpellUi");
        ToggleGameObjectWithTag(true, "WinBackground");
    }

    [ClientRpc]
    public void PlayerDieClientRpc()
    {
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
