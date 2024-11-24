using Unity.Netcode;
using UnityEngine;

public class NotifyClient : NetworkBehaviour
{
    [ClientRpc]
    public void JoinGameClientRpc()
    {
        if (!IsOwner)
            return;

        // If they are the host allow them to click to place a boss object 
        if (IsHost)
        {
            EnablePlacementScript();
            return;
        }

        // try to get the Game object from the scene 
        GameObject joinRoomUI = GameObject.FindWithTag("JoinRoomUI");

        if (joinRoomUI == null)
        {
            Debug.Log("Join room UI was not found");
            return;
        }

        joinRoomUI.SetActive(false);
        StartGameAr.StartNewGame();
    }

    private void EnablePlacementScript()
    {
        PlaceCharacter placeBoss = GetComponent<PlaceCharacter>();

        if (placeBoss == null)
        {
            Debug.Log("Unable to find boss script");
            return;
        }

        placeBoss.enabled = true;
        return;
    }
}
