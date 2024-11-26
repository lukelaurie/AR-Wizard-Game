using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text invalidLoginText;
    [SerializeField] private TMPro.TMP_InputField usernameField;
    [SerializeField] private TMPro.TMP_InputField passwordField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerPageButoon;

    // the canvases needed to start the game 
    [SerializeField] private GameObject startScreens;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameStateScreens;

    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(LoginUser);
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
            // PlayerData.username = 

            startScreens.SetActive(true);
            mainMenu.SetActive(true);
            gameStateScreens.SetActive(true);

            gameObject.SetActive(false);
        }
        else 
        {
            invalidLoginText.text = "Invalid Username or Password";
            await Task.Delay(2000); // wait for 2 seconds
            invalidLoginText.text = "";
        }
    }
}
