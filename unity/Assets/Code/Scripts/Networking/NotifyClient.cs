using Unity.Netcode;
using UnityEngine;

public class NotifyClient : NetworkBehaviour
{
    [ClientRpc]
    public void JoinGameClientRpc()
    {
        if (!IsOwner)
            return;

        ToggleSpellBarOn();
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

    private void ToggleSpellBarOn()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("InitiateSpellUi"))
            {
                obj.SetActive(true);
            }
        }
    }
}
