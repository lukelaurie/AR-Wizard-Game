using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text errorText;
    [SerializeField] private TMPro.TMP_InputField usernameField;
    [SerializeField] private TMPro.TMP_InputField passwordField;
    [SerializeField] private Button crateAccountButton;
    [SerializeField] private Button backToLoginButton;

    // the canvases needed to start the game 
    [SerializeField] private GameObject startScreens;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameStateScreens;

    // Start is called before the first frame update
    void Start()
    {
        crateAccountButton.onClick.AddListener(RegisterUser);
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

        bool isLoggedIn = await AccountManager.Instance.RegisterUser(username, password);

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
            errorText.text = "An error occured while creating account";
            await Task.Delay(2000); // wait for 2 seconds
            errorText.text = "";
        }
    }
}
