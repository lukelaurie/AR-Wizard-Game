using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellStore : MonoBehaviour
{
    private Dictionary<string, int> spells = new Dictionary<string, int>(){
                                {"fireball", 0},
                                {"lightning", 1},
                                {"healing", 2},
                                {"rock", 0}, };
    [SerializeField] private Button fireballButton;
    [SerializeField] private Button lightningButton;
    [SerializeField] private Button healingButton;
    [SerializeField] private Button rockButton;
    [SerializeField] private GameObject fireBallLock;
    [SerializeField] private GameObject lightningLock;
    [SerializeField] private GameObject healingLock;
    [SerializeField] private GameObject rockLock;
    [SerializeField] private TMPro.TMP_Text fireballPrice;
    [SerializeField] private TMPro.TMP_Text lightningPrice;
    [SerializeField] private TMPro.TMP_Text healingPrice;
    [SerializeField] private TMPro.TMP_Text rockPrice;
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        fireBallLock.SetActive(spells["fireball"] == 0);
        lightningLock.SetActive(spells["lightning"] == 0);
        healingLock.SetActive(spells["healing"] == 0);
        rockLock.SetActive(spells["rock"] == 0);

        fireballButton.onClick.AddListener(HandleFireBallClick);
        lightningButton.onClick.AddListener(HandleLightningClick);
        healingButton.onClick.AddListener(HandleHealingClick);
        rockButton.onClick.AddListener(HandleRockClick);

        fireballPrice.text = "$1";
        lightningPrice.text = "$2";
        healingPrice.text = "$3";
        rockPrice.text = "$4";
    }

    private void HandleFireBallClick()
    {
        Debug.Log("FireBallClicked");
    }
    private void HandleLightningClick()
    {
        Debug.Log("LightningClicked");
    }
    private void HandleHealingClick()
    {
        Debug.Log("HealingClicked");
    }
    private void HandleRockClick()
    {
        Debug.Log("RockClicked");
    }
}
