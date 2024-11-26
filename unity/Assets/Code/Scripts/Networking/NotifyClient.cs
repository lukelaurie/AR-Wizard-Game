using Unity.Netcode;
using UnityEngine;

public class NotifyClient : NetworkBehaviour
{
    [SerializeField] private GameObject initiateSpellUi;

    [ClientRpc]
    public void JoinGameClientRpc()
    {
        if (!IsOwner)
            return;

        ToggleSpellBarOn();

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
        PlaceCharacter placeBoss = GetComponent<PlaceCharacter>();

        if (placeBoss == null)
        {
            Debug.Log("Unable to find boss script");
            return;
        }

        placeBoss.enabled = true;
        return;
    }

    private void ToggleSpellBarOn()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in allObjects)
        {
            Debug.Log(obj.tag);
            if (obj.CompareTag("InitiateSpellUi"))
            {
                Debug.Log("here");
                obj.SetActive(true);
            }
        }
    }
}
