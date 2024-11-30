using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    public GameObject fireball;
    public float speed = 8f;
    public float spawnDist = 2f;
    public float lightningSpawnDist = 3f;

    public readonly float FireBallCoolDown = 1f;
    public readonly float LightningCoolDown = 0.2f;
    public readonly float HealCoolDown = 10f;
    public readonly float RockCoolDown = 4f;

    public Camera camera;

    public GameObject lightning;

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
        if (camera == null)
        {
            camera = GameObject.FindObjectOfType<Camera>();
        }
    }

    public void ShootFireball()
    {
        // Vector3 spawnPos = camera.transform.position - camera.transform.forward * 5;
        Vector3 spawnPos = camera.transform.position;
        Vector3 fireballDirection = camera.transform.forward;

        GameObject projectile = Instantiate(fireball, spawnPos, Quaternion.LookRotation(fireballDirection));
        projectile.GetComponent<Rigidbody>().velocity = fireballDirection * speed;
    }

    public void ShootLightning()
    {
        Vector3 spawnPos = camera.transform.position + camera.transform.forward * lightningSpawnDist;

        Vector3 lightningDetection = -camera.transform.forward;

        GameObject projectile = Instantiate(lightning, spawnPos, Quaternion.LookRotation(lightningDetection) * Quaternion.Euler(80, 0, 0));

        //Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), projectile.GetComponent<Collider>());
    }

    public void Heal()
    {
        Debug.Log("TODO: HEAL");
    }

    public void ShootRock()
    {
        Debug.Log("TODO: Shoot Rock");
    }
}
