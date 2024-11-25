using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenStore : MonoBehaviour
{
    [SerializeField] private Button openStoreButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private Canvas storeCanvas;
    [SerializeField] private Canvas homeCanvas;
    // Start is called before the first frame update
    void Start()
    {
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
