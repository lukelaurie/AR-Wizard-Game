using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        continueButton.onClick.AddListener(SwapScreens.Instance.QuitGame);
    }
}
