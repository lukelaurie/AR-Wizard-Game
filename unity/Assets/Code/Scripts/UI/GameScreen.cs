using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameScreen : NetworkBehaviour
{
    [SerializeField] private TMPro.TMP_Text placeBossText;
    private PlaceCharacter placeCharacter;
    private PlayerData playerData;


    void Awake()
    {
        placeCharacter = GameObject.FindWithTag(TagManager.GameLogic).GetComponent<PlaceCharacter>();
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
    }

    void OnEnable()
    {
        if (playerData.IsPlayerHost())
        {
            placeCharacter.enabled = true;
            placeCharacter.ResetIsPlaced();
        }

        if (playerData.IsPlayerHost())
        {
            placeBossText.text = "Tap the screen to place the boss";
        }
        else
        {
            placeBossText.text = "Waiting for host to place the boss...";
        }
    }
}
