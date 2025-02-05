using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SpellStore : MonoBehaviour
{
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
    [SerializeField] private TMPro.TMP_Text fireballLevel;
    [SerializeField] private TMPro.TMP_Text lightningLevel;
    [SerializeField] private TMPro.TMP_Text healingLevel;
    [SerializeField] private TMPro.TMP_Text rockLevel;
    // Start is called before the first frame update

    private readonly int MAX_LEVEL = 5;

    private readonly Dictionary<string, int> spellPrices = new(){
        {"fireball",  750},
        {"lightning", 1000},
        {"healing",   1250},
        {"rock",      2000},
    };

    private readonly Dictionary<int, int> levelPrices = new()
    {
        {1, 1},
        {2, 2},
        {3, 4},
        {4, 7},
        {5, 12},
    };

    private readonly Color red = new Color(255, 0, 0);
    private readonly Color green = new Color(0, 255, 0);

    private PlayerData playerData;

    void Start()
    {
        playerData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

        fireballButton.onClick.AddListener(() => HandleSpellClick("fireball"));
        lightningButton.onClick.AddListener(() => HandleSpellClick("lightning"));
        healingButton.onClick.AddListener(() => HandleSpellClick("healing"));
        rockButton.onClick.AddListener(() => HandleSpellClick("rock"));

        UpdateUi(playerData.GetSpells());
        UpdatePlayerData.Instance.AddSpellsUpdateCallBack(UpdateUi);
    }

    private void UpdateUi(Dictionary<string, int> spells)
    {
        UpdateButton(spells, "fireball", fireBallLock, fireballPrice, fireballLevel);
        UpdateButton(spells, "lightning", lightningLock, lightningPrice, lightningLevel);
        UpdateButton(spells, "healing", healingLock, healingPrice, healingLevel);
        UpdateButton(spells, "rock", rockLock, rockPrice, rockLevel);
    }

    private void UpdateButton(Dictionary<string, int> spells, string spellName, GameObject lockPanel, TMPro.TMP_Text priceText, TMPro.TMP_Text levelText)
    {
        int coins = playerData.GetCoinTotal();

        if (spells.ContainsKey(spellName))
        {
            lockPanel.SetActive(false);
            int lvl = spells[spellName];
            if (lvl == 5)
            {
                priceText.text = "";
                levelText.text = $"Level MAX";
                return;
            }
            int basePrice = spellPrices[spellName];
            int lvlMulti = levelPrices[lvl + 1];
            int price = basePrice * lvlMulti;
            priceText.text = $"Upgrade:\n${price}";
            levelText.text = $"Level {lvl}";
            priceText.color = price <= coins ? green : red;
        }
        else
        {
            lockPanel.SetActive(true);
            levelText.text = "";
            int price = spellPrices[spellName];
            priceText.text = $"Unlock:\n${price}";
            priceText.color = price <= coins ? green : red;
        }
    }

    private async void HandleSpellClick(string spellName)
    {
        if (playerData.IsSpellUnlocked(spellName))
        {
            if (playerData.GetSpells()[spellName] == MAX_LEVEL)
            {
                return;
            }
            await SpellManager.Instance.UpgradeSpell(spellName);
            UpdatePlayerData.Instance.UpdatePlayerCoinTotal();
            UpdatePlayerData.Instance.UpdatePlayerSpells();

        }
        else
        {
            await SpellManager.Instance.PurchaseSpell(spellName);
            UpdatePlayerData.Instance.UpdatePlayerCoinTotal();
            UpdatePlayerData.Instance.UpdatePlayerSpells();
        }
    }
}
