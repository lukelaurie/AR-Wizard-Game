using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject rock;

    private readonly float fireballSpeed = 8f;
    private readonly float rockSpeed = 3f;
    private readonly float lightningSpawnDist = 2f;
    private readonly string fireballName = "fireball";
    private readonly string lightningName = "lightning";
    private readonly string healName = "healing";
    private readonly string rockName = "rock";
    private NotifyServer server;

    public readonly float FireBallCoolDown = 1f;
    public readonly float LightningCoolDown = 0.2f;
    public readonly float HealCoolDown = 10f;
    public readonly float RockCoolDown = 1f;

    private readonly Dictionary<string, float> spellAmounts = new(){
        {"fireball", 3f},
        {"lightning", 1f},
        {"healing", 2f},
        {"rock", 6f},
    };

    private readonly float increasePerLevel = 0.3f;
    private Camera playerCamera;

    private PlayerData playerData;

    public static PlayerShoot Instance { get; private set; }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

        GameObject projectile = Instantiate(fireball, spawnPos, Quaternion.LookRotation(fireballDirection));

        projectile.GetComponent<PlayerFireball>().SetDamage(CalcAmount(fireballName));
        projectile.GetComponent<Rigidbody>().velocity = fireballDirection * fireballSpeed;

        server.SpawnObjectServerRpc(spawnPos, fireballDirection, playerData.GetUsername(), fireballName);
    }

    public void ShootLightning()
    {
        Vector3 spawnPos = playerCamera.transform.position + playerCamera.transform.forward * lightningSpawnDist;

        Vector3 lightningDetection = -playerCamera.transform.forward;

        GameObject projectile = Instantiate(lightning, spawnPos, Quaternion.LookRotation(lightningDetection) * Quaternion.Euler(80, 0, 0));
        projectile.GetComponent<Lightning>().SetDamage(CalcAmount(lightningName));

        server.SpawnObjectServerRpc(spawnPos, lightningDetection, playerData.GetUsername(), lightningName);
    }

    public void Heal()
    {
        playerData.HealPlayer(CalcAmount(healName));
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
}
