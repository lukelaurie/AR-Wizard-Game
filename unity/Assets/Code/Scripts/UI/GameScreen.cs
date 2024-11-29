using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameScreen : NetworkBehaviour
{
    [SerializeField] private TMPro.TMP_Text placeBossText;
    private PlaceCharacter placeCharacter; 
    private PlayerData playerData; 


    // Start is called before the first frame update
    void Awake()
    {
        placeCharacter = gameObject.GetComponent<PlaceCharacter>();
        playerData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
    }

    void OnEnable()
    {
        placeCharacter.ResetIsPlaced();

        if (playerData.IsPlayerHost())
        {
            placeBossText.text = "Tap the screen to place the boss";
        }
        else
        {
            placeBossText.text = "";
            // placeBossText.text = "Waiting for host to place the boss...";
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            placeCharacter.HandlePlaceObjectUnity();
        }
#endif
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && Input.touchCount < 2 &&
            Input.GetTouch(0).phase == TouchPhase.Began) {
            placeCharacter.HandlePlaceObjectIOS();
        }
#endif
    }
}
