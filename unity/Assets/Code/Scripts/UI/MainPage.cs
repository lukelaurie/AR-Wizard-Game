using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    [SerializeField] private Button openStoreButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private Canvas storeCanvas;
    [SerializeField] private Canvas homeCanvas;
    [SerializeField] private TMPro.TMP_Text coinText;

    private PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        int userCoins = playerData.GetCoinTotal();
        coinText.text = $"Coins:\n{userCoins}";

        openStoreButton.onClick.AddListener(() =>
        {
            homeCanvas.gameObject.SetActive(false);
            storeCanvas.gameObject.SetActive(true);
        });

        returnButton.onClick.AddListener(() =>
        {
            storeCanvas.gameObject.SetActive(false);
            homeCanvas.gameObject.SetActive(true);
        });
    }
}
