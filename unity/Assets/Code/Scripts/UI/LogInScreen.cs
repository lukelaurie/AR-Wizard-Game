using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogInScreen : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text invalidText;
    [SerializeField] private TMPro.TMP_InputField usernameField;
    [SerializeField] private TMPro.TMP_InputField passwordField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerPageButton;
    // Start is called before the first frame update
    void Start()
    {
        IsLoggedIn();

        loginButton.onClick.AddListener(LoginUser);
        registerPageButton.onClick.AddListener(RegisterUser);
    }

    private async void IsLoggedIn()
    {
        // check if the user is already logged in 
        string username = await AccountManager.Instance.IsLoggedIn();

        if (username != null)
        {
            var clientData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            clientData.SetUsername(username);

            SwapScreens.Instance.ToggleMainPage();
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private async void RegisterUser()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (username == "" || password == "")
        {
            Debug.Log("Please enter username/password");
            return;
        }

        bool isRegisteredIn = await AccountManager.Instance.RegisterUser(username, password);

        if (!isRegisteredIn)
        {
            invalidText.text = "Username already exists";
            await Task.Delay(2000); // wait for 2 seconds
            invalidText.text = "";

            return;
        }
        LoginUser();
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
            var clientData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            clientData.SetUsername(username);

            SwapScreens.Instance.ToggleMainPage();
            gameObject.SetActive(false);
        }
        else
        {
            invalidText.text = "Invalid Username or Password";
            await Task.Delay(2000); // wait for 2 seconds
            invalidText.text = "";
        }
    }
}
