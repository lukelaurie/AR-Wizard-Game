using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    public Button fireButton;
    public Button lightningButton;
    public GameObject fireball;
    public float speed = 8f;
    public float spawnDist = 2f;
    public float lightningSpawnDist = 3f;

    public Camera camera;

    private float waitTime = 1f;
    private float lWaitTime = 0.2f;
    private float fireballTimer = 0.0f;
    private float lightningTimer = 0.0f;

    public GameObject lightning;

    void Start()
    {
        fireButton.onClick.AddListener(ShootFireball);
        lightningButton.onClick.AddListener(ShootLightning);
    }

    void Update()
    {
        fireballTimer += Time.deltaTime;

        //must wait a second between shooting
        if (Input.GetKey(KeyCode.F) && fireballTimer >= waitTime)
        {
            ShootFireball();
            fireballTimer = 0;
        }



        lightningTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.L) && lightningTimer >= lWaitTime)
        {
            ShootLightning();
            lightningTimer = 0;
        }
    }

    void ShootFireball()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Ray ray = camera.ScreenPointToRay(mouseScreenPos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        Vector3 spawnPos = camera.transform.position + camera.transform.forward * spawnDist;

        GameObject projectile = Instantiate(fireball, spawnPos, Quaternion.LookRotation(ray.direction));
        projectile.GetComponent<Rigidbody>().velocity = ray.direction * speed;
    }

    void ShootLightning()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Ray ray = camera.ScreenPointToRay(mouseScreenPos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        Vector3 spawnPos = camera.transform.position + camera.transform.forward * lightningSpawnDist;
        GameObject projectile = Instantiate(lightning, spawnPos, Quaternion.LookRotation(ray.direction) * Quaternion.Euler(80, 0, 0));

        //Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), projectile.GetComponent<Collider>());
    }
}
