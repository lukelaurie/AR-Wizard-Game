using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject rock;
    [SerializeField] private Image healFX;

    private readonly float fireballSpeed = 8f;
    private readonly float rockSpeed = 8f;
    private readonly string fireballName = "fireball";
    private readonly string lightningName = "lightning";
    private readonly string healName = "healing";
    private readonly string rockName = "rock";
    private NotifyServer server;

    public readonly float FireBallCoolDown = 2f;
    public readonly float LightningCoolDown = 1f;
    public readonly float HealCoolDown = 10f;
    public readonly float RockCoolDown = 3f;

    private readonly Dictionary<string, float> spellAmounts = new(){
        {"fireball", 15f},
        {"lightning", 8f},
        {"healing", 10f},
        {"rock", 35f},
    };

    private readonly float increasePerLevel = 0.75f;
    private Camera playerCamera;

    private PlayerData playerData;

    public static PlayerShoot Instance { get; private set; }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        if (playerCamera == null)
        {
            playerCamera = GameObject.FindObjectOfType<Camera>();
        }

        server = GameObject.FindWithTag(TagManager.GameLogic).GetComponent<NotifyServer>();
    }

    public void ShootFireball()
    {
        Vector3 spawnPos = playerCamera.transform.position;
        Vector3 fireballDirection = playerCamera.transform.forward;
        GameObject projectile = Instantiate(fireball, spawnPos, Quaternion.Euler(0, 0, 0));

        projectile.GetComponent<PlayerFireball>().SetDamage(CalcAmount(fireballName));
        projectile.GetComponent<Rigidbody>().velocity = fireballDirection * fireballSpeed;

        server.SpawnObjectServerRpc(spawnPos, fireballDirection, playerData.GetUsername(), fireballName);
    }

    public void ShootLightning()
    {
        Vector3 spawnPos = playerCamera.transform.position + playerCamera.transform.forward * (lightning.transform.localScale.y / 2f);

        Vector3 lightningLookVector = playerCamera.transform.forward;

        GameObject projectile = Instantiate(lightning, spawnPos, Quaternion.LookRotation(lightningLookVector) * Quaternion.Euler(80, 0, 0));
        projectile.GetComponent<Lightning>().SetDamage(CalcAmount(lightningName));

        server.SpawnObjectServerRpc(spawnPos, lightningLookVector, playerData.GetUsername(), lightningName);
    }

    // public void ShootLightning()
    // {
    //     Vector3 spawnPos = playerCamera.transform.position + playerCamera.transform.forward * lightningSpawnDist;

    //     Vector3 lightningDetection = -playerCamera.transform.forward;

    //     GameObject projectile = Instantiate(lightning, spawnPos, Quaternion.LookRotation(lightningDetection) * Quaternion.Euler(80, 0, 0));
    //     projectile.GetComponent<Lightning>().SetDamage(CalcAmount(lightningName));

    //     server.SpawnObjectServerRpc(spawnPos, lightningDetection, playerData.GetUsername(), lightningName);
    // }

    public void Heal()
    {
        playerData.HealPlayer(CalcAmount(healName));
        var coroutine = HealAnimation();
        StartCoroutine(coroutine);
    }

    public void ShootRock()
    {
        Vector3 spawnPos = playerCamera.transform.position;
        Vector3 rockDirection = playerCamera.transform.forward;

        GameObject projectile = Instantiate(rock, spawnPos, Quaternion.LookRotation(rockDirection));
        projectile.GetComponent<PlayerRock>().SetDamage(CalcAmount(rockName));
        projectile.GetComponent<Rigidbody>().velocity = rockDirection * rockSpeed;

        server.SpawnObjectServerRpc(spawnPos, rockDirection, playerData.GetUsername(), rockName);
    }

    private float CalcAmount(string spellName)
    {
        var spells = playerData.GetSpells();
        if (!spells.ContainsKey(spellName))
        {
            Debug.LogError("Attempting to calc damage for spell that player does not own");
            return 0f;
        }
        int lvl = spells[spellName];
        float multi = (lvl - 1) * increasePerLevel + 1;
        return multi * spellAmounts[spellName];
    }

    private IEnumerator HealAnimation()
    {
        var healColor = new Color(0.176f, 1, 0.836f);
        float maxAlpha = 0.5f;
        float minAlpha = 0f;
        float curAlpha = 0f;
        healFX.gameObject.SetActive(true);
        for (float t = 0; curAlpha < maxAlpha; t += Time.deltaTime * 3)
        {
            curAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);
            healColor.a = curAlpha;
            healFX.color = healColor;
            yield return null;
        }
        for (float t = 1; curAlpha > minAlpha; t -= Time.deltaTime)
        {
            curAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);
            healColor.a = curAlpha;
            healFX.color = healColor;
            yield return null;
        }
        healFX.gameObject.SetActive(false);
    }
}
