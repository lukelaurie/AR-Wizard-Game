using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject rock;

    private readonly float fireballSpeed = 8f;
    private readonly float rockSpeed = 3f;
    private readonly float lightningSpawnDist = 2f;

    public readonly float FireBallCoolDown = 1f;
    public readonly float LightningCoolDown = 0.2f;
    public readonly float HealCoolDown = 10f;
    public readonly float RockCoolDown = 1f;

    private readonly Dictionary<string, float> spellAmounts = new(){
        {"fireball", 3f},
        {"lightning", 1f},
        {"healing", -1f}, //TODO
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
    }

    public void ShootFireball()
    {
        Vector3 spawnPos = playerCamera.transform.position;
        Vector3 fireballDirection = playerCamera.transform.forward;

        GameObject projectile = Instantiate(fireball, spawnPos, Quaternion.LookRotation(fireballDirection));
        projectile.GetComponent<PlayerFireball>().SetDamage(CalcAmount("fireball"));
        projectile.GetComponent<Rigidbody>().velocity = fireballDirection * fireballSpeed;
    }

    public void ShootLightning()
    {
        Vector3 spawnPos = playerCamera.transform.position + playerCamera.transform.forward * lightningSpawnDist;

        Vector3 lightningDetection = -playerCamera.transform.forward;

        GameObject projectile = Instantiate(lightning, spawnPos, Quaternion.LookRotation(lightningDetection) * Quaternion.Euler(80, 0, 0));
        projectile.GetComponent<Lightning>().SetDamage(CalcAmount("lightning"));
    }

    public void Heal()
    {
        Debug.Log("TODO: HEAL");
    }

    public void ShootRock()
    {
        Vector3 spawnPos = playerCamera.transform.position;
        Vector3 rockDirection = playerCamera.transform.forward;

        GameObject projectile = Instantiate(rock, spawnPos, Quaternion.LookRotation(rockDirection));
        projectile.GetComponent<Rock>().SetDamage(CalcAmount("rock"));
        projectile.GetComponent<Rigidbody>().velocity = rockDirection * rockSpeed;
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
