using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTakeImageScreen : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button retakeImageButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private GameObject BlitImageUI;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private TMPro.TMP_Text placementText;


    private PlayerData playerData;

    void Start()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();

        if (playerData.IsPlayerHost())
        {
            placementText.text = "Take a picture of your environment.";
        }
        else 
        {
            placementText.text = "Take the same picture as the host.";
        }

        homeButton.onClick.AddListener(SwapScreens.Instance.QuitGame);
        retakeImageButton.onClick.AddListener(DisplayPicture);
        nextPageButton.onClick.AddListener(MoveNextPage);

        DisplayPicture();
    }

    public void MoveNextPage()
    {
        BlitImageUI.SetActive(false);
        if (playerData.IsPlayerHost())
        {
            SwapScreens.Instance.ToggleHostScreen();
        }
        else
        {
            SwapScreens.Instance.ToggleJoinScreen();
        }
    }

    private void DisplayPicture()
    {
        BlitImageForColocalization.Instance.TakePicture();

        if (_renderTexture != null)
        {
            _rectTransform.sizeDelta = new Vector2(_renderTexture.width / 3.25f, _renderTexture.height / 3.25f);
        }
        else
        {
            Debug.LogError("RenderTexture is not assigned or is null.");
        }

        BlitImageUI.SetActive(true);
    }
}
