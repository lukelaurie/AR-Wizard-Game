using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetakeImageLogic : MonoBehaviour
{
    [SerializeField] private Button retakeImageButton;

    private void Start()
    {
        // TODO -> follow tutorial and fix this functionality
        // retakeImageButton.onClick.AddListener(QuitGameToRetakeImage);
    }

    private void QuitGameToRetakeImage() {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
