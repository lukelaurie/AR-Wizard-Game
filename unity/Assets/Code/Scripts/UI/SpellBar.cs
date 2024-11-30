using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBar : MonoBehaviour
{
    [SerializeField] private Button fireballButton;
    [SerializeField] private Button lightningButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button rockButton;
    [SerializeField] private RectTransform fireballCoolDownIndicator;
    [SerializeField] private RectTransform lightningCoolDownIndicator;
    [SerializeField] private RectTransform healCoolDownIndicator;
    [SerializeField] private RectTransform rockCoolDownIndicator;

    private float timeUnitlFireballOffCoolDown = 0f;
    private float timeUnitlLightningOffCoolDown = 0f;
    private float timeUnitlHealOffCoolDown = 0f;
    private float timeUnitlRockOffCoolDown = 0f;

    private PlayerData playerData;

    private readonly float buttonHeight = 140f;
    // Start is called before the first frame update
    void OnEnable()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();

        if (playerData.IsSpellUnlocked("fireball"))
        {
            fireballButton.gameObject.SetActive(true);
            fireballButton.onClick.AddListener(HandleFireBallClick);
        }
        else
        {
            fireballButton.gameObject.SetActive(false);
        }

        if (playerData.IsSpellUnlocked("lightning"))
        {
            lightningButton.gameObject.SetActive(true);
            lightningButton.onClick.AddListener(HandleLightningClick);
        }
        else
        {
            lightningButton.gameObject.SetActive(false);
        }

        if (playerData.IsSpellUnlocked("healing"))
        {
            healButton.gameObject.SetActive(true);
            healButton.onClick.AddListener(HandleHealClick);
        }
        else
        {
            healButton.gameObject.SetActive(false);
        }

        if (playerData.IsSpellUnlocked("rock"))
        {
            rockButton.gameObject.SetActive(true);
            rockButton.onClick.AddListener(HandleRockClick);
        }
        else
        {
            rockButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeUnitlFireballOffCoolDown = Mathf.Clamp(timeUnitlFireballOffCoolDown - Time.deltaTime, 0f, Mathf.Infinity);
        timeUnitlLightningOffCoolDown = Mathf.Clamp(timeUnitlLightningOffCoolDown - Time.deltaTime, 0f, Mathf.Infinity);
        timeUnitlHealOffCoolDown = Mathf.Clamp(timeUnitlHealOffCoolDown - Time.deltaTime, 0f, Mathf.Infinity);
        timeUnitlRockOffCoolDown = Mathf.Clamp(timeUnitlRockOffCoolDown - Time.deltaTime, 0f, Mathf.Infinity);

        UpdateCoolDownState(fireballCoolDownIndicator, timeUnitlFireballOffCoolDown / PlayerShoot.Instance.FireBallCoolDown);
        UpdateCoolDownState(lightningCoolDownIndicator, timeUnitlLightningOffCoolDown / PlayerShoot.Instance.LightningCoolDown);
        UpdateCoolDownState(healCoolDownIndicator, timeUnitlHealOffCoolDown / PlayerShoot.Instance.HealCoolDown);
        UpdateCoolDownState(rockCoolDownIndicator, timeUnitlRockOffCoolDown / PlayerShoot.Instance.RockCoolDown);
    }

    private void UpdateCoolDownState(RectTransform indicator, float progress)
    {
        indicator.offsetMax = new Vector2(indicator.offsetMax.x, -(1f - progress) * buttonHeight);
        indicator.offsetMin = new Vector2(indicator.offsetMin.x, 0);
    }

    private void HandleFireBallClick()
    {
        if (timeUnitlFireballOffCoolDown != 0f)
        {
            return;
        }
        timeUnitlFireballOffCoolDown = PlayerShoot.Instance.FireBallCoolDown;
        PlayerShoot.Instance.ShootFireball();
    }

    private void HandleLightningClick()
    {
        if (timeUnitlLightningOffCoolDown != 0f)
        {
            return;
        }
        timeUnitlLightningOffCoolDown = PlayerShoot.Instance.LightningCoolDown;
        PlayerShoot.Instance.ShootLightning();
    }
    private void HandleHealClick()
    {
        if (timeUnitlHealOffCoolDown != 0f)
        {
            return;
        }
        timeUnitlHealOffCoolDown = PlayerShoot.Instance.HealCoolDown;
        PlayerShoot.Instance.ShootRock();
    }

    private void HandleRockClick()
    {
        if (timeUnitlRockOffCoolDown != 0f)
        {
            return;
        }
        timeUnitlRockOffCoolDown = PlayerShoot.Instance.RockCoolDown;
        PlayerShoot.Instance.Heal();
    }
}
