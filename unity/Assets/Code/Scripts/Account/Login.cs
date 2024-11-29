using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text invalidLoginText;
    [SerializeField] private TMPro.TMP_InputField usernameField;
    [SerializeField] private TMPro.TMP_InputField passwordField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerPageButton;

    // the canvases needed to start the game 
    [SerializeField] private GameObject startScreens;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameStateScreens;

    [SerializeField] private GameObject registerScreen;

    // Start is called before the first frame update
    void Start()
    {
        IsLoggedIn();

        loginButton.onClick.AddListener(LoginUser);
        registerPageButton.onClick.AddListener(SwapRegisterPage);
    }

    private async void IsLoggedIn()
    {
        // UnityWebRequest.ClearCookieCache();        

        // check if the user is already logged in 
        string username = await AccountManager.Instance.IsLoggedIn();

        if (username != null)
        {
            var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
            clientData.SetUsername(username);

            SwapMainPage();
        }
    }


    private async void LoginUser()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (username == "" || password == "")
        {
            Debug.Log("Please enter username/password");
            return;
        }

        bool isLoggedIn = await AccountManager.Instance.LoginUser(username, password);

        if (isLoggedIn)
        {
            var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
            clientData.SetUsername(username);

            SwapMainPage();
        }
        else
        {
            invalidLoginText.text = "Invalid Username or Password";
            await Task.Delay(2000); // wait for 2 seconds
            invalidLoginText.text = "";
        }
    }

    private void SwapRegisterPage()
    {
        registerScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    private void SwapMainPage()
    {
        startScreens.SetActive(true);
        mainMenu.SetActive(true);
        gameStateScreens.SetActive(true);

        gameObject.SetActive(false);
    }
}
