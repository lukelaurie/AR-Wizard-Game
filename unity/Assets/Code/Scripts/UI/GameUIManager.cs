using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameUIManager : NetworkBehaviour
{
    [SerializeField] private Canvas createGameCanvas;
    [SerializeField] private Camera startCamera;

    private void Start()
    {
        ShowCreateGameCanvas();
    }

    private void ShowCreateGameCanvas()
    {
        createGameCanvas.gameObject.SetActive(true);
    }

    public override void OnNetworkSpawn()
    {
        createGameCanvas.gameObject.SetActive(false);
        startCamera.gameObject.SetActive(false);
    }
}
