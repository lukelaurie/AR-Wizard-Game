using System;
using UnityEngine;
using UnityEngine.UI;


public class RetakeImageLogic : MonoBehaviour
{
    [SerializeField] private Button retakeImageButton;
    public static event Action OnRetakePicture;

    private void Start()
    {
        retakeImageButton.onClick.AddListener(SwapScreens.Instance.QuitGame);
    }
}
