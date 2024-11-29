using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInScreen : MonoBehaviour
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private Canvas homeCanvas;
    [SerializeField] private Canvas loginCanvas;
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(() =>
        {
            homeCanvas.gameObject.SetActive(true);
            loginCanvas.gameObject.SetActive(false);
        });

        createButton.onClick.AddListener(() =>
        {
            homeCanvas.gameObject.SetActive(true);
            loginCanvas.gameObject.SetActive(false);
        });
    }
}
