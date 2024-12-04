using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        playerData.SetIsGameOver();
        continueButton.onClick.AddListener(SwapScreens.Instance.QuitGame);
    }
}
