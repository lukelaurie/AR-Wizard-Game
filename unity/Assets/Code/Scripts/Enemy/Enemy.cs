using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

//keeps track of health, moveSpeed, animations, and following
//used for dragon enemies,etc
public class Enemy : NetworkBehaviour
{
    [SerializeField] public GameObject fireball;
    [SerializeField] public GameObject otherProjectile;
    [SerializeField] public GameObject attackPuddle;

    public Animator enemyAnimator;

    private float waitTime = 3.0f;
    private float timer = 0.0f;

    private bool canPlayAnim;
    private bool canBeHit;

    private BossData bossData;
    private PlayerData playerData;

    private int bossLevel;

    private int projectileNum;
    private float projectileSpeed;
    private float projectileDamage;

    void Start()
    {
        bossData = gameObject.GetComponent<BossData>();

        FindObjectOfType<AudioManager>().Stop("StartMusic");

        bossLevel = bossData.GetBossLevel();
        switch (bossLevel)
        {
            case 1:
                projectileNum = 9;
                projectileSpeed = 5f;
                projectileDamage = 10;
                FindObjectOfType<AudioManager>().Play($"BossMusic{bossLevel}");
                break;
            case 2:
                projectileNum = 10;
                projectileSpeed = 6f;
                projectileDamage = 15;
                FindObjectOfType<AudioManager>().Play($"BossMusic{bossLevel}");
                break;
            case 3:
                projectileNum = 12;
                projectileSpeed = 6f;
                projectileDamage = 20;
                FindObjectOfType<AudioManager>().Play($"BossMusic{bossLevel}");
                break;
            case 4:
                projectileNum = 15;
                projectileSpeed = 8f;
                projectileDamage = 100;
                FindObjectOfType<AudioManager>().Play($"BossMusic{bossLevel}");
                break;
        }

        canPlayAnim = true;
        canBeHit = true;
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerData.IsPlayerHost())
            return;

        timer += Time.deltaTime;

        if (timer < waitTime)
            return;

        // int randAttack = UnityEngine.Random.Range(0, 3);
        int randAttack = 1;

        switch (randAttack)
        {
            case 0:
                AllClientsInvoker.Instance.InvokeBossAttackPlayers("Roar");
                break;
            case 1:
                AllClientsInvoker.Instance.InvokeBossAttackPlayers("GroundPound");
                break;
            case 2:
                AllClientsInvoker.Instance.InvokeBossAttackPlayers("Fireball");
                break;
        }

        waitTime = (float)UnityEngine.Random.Range(6, 9); // how long wait between attacks
        timer = 0;
    }

    private void SpawnObjectsAround(string name)
    {
        GameObject spawnObj;

        if (name == "fireball")
        {
            spawnObj = fireball;
        }
        else if(bossLevel == 1) //if its a rock
        {
            FindObjectOfType<AudioManager>().Play("AlbinoRoarAttack");
            spawnObj = otherProjectile;
        }
        else
        {
            spawnObj = otherProjectile;
        }

        float radius = 2f;
        float angleStep = 360f / projectileNum;

        // start angle is random each time
        float angle = UnityEngine.Random.Range(0f, angleStep);

        for (int i = 0; i < projectileNum; i++)
        {
            //calc spawn pos based on angle
            float spawnX = transform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float spawnZ = transform.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float spawnY = transform.position.y + .25f; //spawns above slightly
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

            GameObject spawnedObj = Instantiate(spawnObj, spawnPosition, Quaternion.identity);

            // IBossSpell spellScript = (name == "fireball") ? spawnedObj.GetComponent<BossFireball>() : spawnedObj.GetComponent<BossRock>();
            BossProjectile spellScript = spawnedObj.GetComponent<BossProjectile>();
            spellScript.SetDamage(projectileDamage);

            Rigidbody rb = spawnedObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //shoot in the direction of where the object was spawned
                Vector3 direction = new Vector3(
                    spawnPosition.x - transform.position.x,
                    0, //zero out Y to ignore height offset
                    spawnPosition.z - transform.position.z
                ).normalized;
                rb.velocity = direction * projectileSpeed;
            }

            //increment angle
            angle += angleStep;
        }
    }

    private IEnumerator WaitAndPerformAction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canPlayAnim = true;
    }

    private IEnumerator WaitAndThrowObjects(float seconds, string obj)
    {
        yield return new WaitForSeconds(seconds);

        SpawnObjectsAround(obj);
        //enemyAnimator.Play("Idle");
        canPlayAnim = true;
    }

    public void TakeDamage(float damageAmount)
    {
        if (!canBeHit)
        {
            Debug.Log("Enemy can't be hit (dead)");
            return;
        }

        int randInt = UnityEngine.Random.Range(0, 5);

        //has a chance to randomly block an attack
        if (randInt == 0)
        {
            if(canPlayAnim)
            {
                FindObjectOfType<AudioManager>().Play("AlbinoBlock");
                canPlayAnim = false;
                StartCoroutine(WaitAndPerformAction(2f));
            }
        }
        else if (bossData.GetBossHealth() > 0)
        {
            if (canPlayAnim)
            {
                enemyAnimator.Play("Get Hit");
                canPlayAnim = false;
                StartCoroutine(WaitAndPerformAction(1.5f));
            }
            bossData.BossTakeDamage(damageAmount);
        }
        else
        {
            bossData.BossTakeDamage(damageAmount);
        }
    }

    public void BossRoarAttack()
    {
        //roar attack
        enemyAnimator.Play("Scream");
        FindObjectOfType<AudioManager>().Play("AlbinoRoar");

        canPlayAnim = false;
        StartCoroutine(WaitAndPerformAction(3f));
    }

    public void BossGroundPoundAttack()
    {
        // ground pound
        enemyAnimator.Play("Jump");
        canPlayAnim = false;
        StartCoroutine(WaitAndThrowObjects(2f, "rock"));
        FindObjectOfType<AudioManager>().Play("AlbinoJump");

        Instantiate(attackPuddle, gameObject.transform.position, Quaternion.identity);
    }

    public void BossFireballAttack()
    {
        // fireball
        enemyAnimator.Play("Basic Attack");
        canPlayAnim = false;
        StartCoroutine(WaitAndThrowObjects(1f, "fireball"));
    }

    public IEnumerator PlayBossDeath(float seconds, System.Action onDeathComplete)
    {
        enemyAnimator.Play("Die", 0, 0);
        FindObjectOfType<AudioManager>().Play("AlbinoDeath");
        canPlayAnim = false;
        canBeHit = false;
        FindObjectOfType<AudioManager>().Stop($"BossMusic{bossLevel}");
        FindObjectOfType<AudioManager>().Play("StartMusic");

        yield return new WaitForSeconds(seconds);

        // have the host destroy
        if (playerData.IsPlayerHost())
        {
            Destroy(gameObject);
        }

        // after boss dies toggle the screen
        onDeathComplete?.Invoke();
    }
}