using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

//keeps track of health, moveSpeed, animations, and following
//used for dragon enemies,etc
public class Enemy : NetworkBehaviour
{
    private float waitTime = 3.0f;
    private float timer = 0.0f;

    BossData bossData;

    public Animator enemyAnimator;

    private bool canPlayAnim;
    private bool canBeHit;

    [SerializeField] public GameObject fireball;
    [SerializeField] public GameObject rock;

    void Start()
    {
        bossData = gameObject.GetComponent<BossData>();

        FindObjectOfType<AudioManager>().Stop("StartMusic");
        FindObjectOfType<AudioManager>().Play("BossMusic");

        canPlayAnim = true;
        canBeHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < waitTime)
            return;

        // int randAttack = UnityEngine.Random.Range(0, 3);
        int randAttack = 2;

        switch (randAttack)
        {
            case 0:
                //roar attack
                enemyAnimator.Play("Scream");
                FindObjectOfType<AudioManager>().Play("AlbinoRoar");

                canPlayAnim = false;
                StartCoroutine(WaitAndPerformAction(3f));
                break;
            case 1:
                // ground pound
                enemyAnimator.Play("Jump");
                canPlayAnim = false;
                StartCoroutine(WaitAndThrowObjects(2f, "rock"));
                FindObjectOfType<AudioManager>().Play("AlbinoJump");
                break;
            case 2:
                // fireball
                enemyAnimator.Play("Basic Attack");
                canPlayAnim = false;
                StartCoroutine(WaitAndThrowObjects(1f, "fireball"));
                break;
        }

        waitTime = (float)UnityEngine.Random.Range(6, 9); // how long wait between attacks
        timer = 0;
    }

    private void SpawnObjectsAround(string name)
    {
        int numberOfObjects = 12;
        float radius = 2f;
        float speed = 6f;

        float angleStep = 360f / numberOfObjects;
        float angle = 0f;

        GameObject spawnObj;

        if (name == "fireball")
        {
            spawnObj = fireball;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("AlbinoRoarAttack");
            spawnObj = rock;
            speed = 11f;
            numberOfObjects = 9;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            //calc spawn pos based on angle
            float spawnX = transform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float spawnZ = transform.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float spawnY = transform.position.y + 1f; //spawns above slightly
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

            GameObject spawnedObj = Instantiate(spawnObj, spawnPosition, Quaternion.identity);
            //Fireball fireballScript = spawnedObj.GetComponent<Fireball>();
            //fireballScript.SetTargetToBoss();
            //Debug.LogError("TODO");
            Rigidbody rb = spawnedObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //shoot in the direction of where the object was spawned
                Vector3 direction = new Vector3(
                    spawnPosition.x - transform.position.x,
                    0, //zero out Y to ignore height offset
                    spawnPosition.z - transform.position.z
                ).normalized;
                rb.velocity = direction * speed;
            }

            //increment angle
            angle += angleStep;
        }
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
        if (randInt == 0 && canPlayAnim)
        {
            FindObjectOfType<AudioManager>().Play("AlbinoBlock");
            canPlayAnim = false;
            StartCoroutine(WaitAndPerformAction(2f));
        }
        else if (canPlayAnim && bossData.GetBossHealth() > 0)
        {
            enemyAnimator.Play("AlbinoHurt");
            canPlayAnim = false;
            StartCoroutine(WaitAndPerformAction(1.5f));

            bossData.BossTakeDamage(damageAmount);

            if (bossData.GetBossHealth() <= 0)
            {
                //play for everyone
                // FindObjectOfType<AudioManager>().Play("WinScreen");

                // float newTime = 0.0f;
                // float animTime = 3.0f;
                enemyAnimator.Play("Die");
                // AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
                // FindObjectOfType<AudioManager>().Stop("BossMusic");
                // FindObjectOfType<AudioManager>().Play("StartMusic");


                canBeHit = false;
            }
        }
    }

    public IEnumerator PlayBossDeath(float seconds, System.Action onDeathComplete)
    {
        enemyAnimator.Play("Die");
        FindObjectOfType<AudioManager>().Play("AlbinoDeath");
        canPlayAnim = false;
        canBeHit = false;

        yield return new WaitForSeconds(seconds);

        // have the host destroy
        PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        if (playerData.IsPlayerHost())
        {
            Destroy(gameObject);
        }

        // after boss dies toggle the screen
        onDeathComplete?.Invoke();
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3f);

        NotifyServer server = GameObject.FindWithTag(TagManager.GameLogic).GetComponent<NotifyServer>();
        // server.NotifyBossDeathServerRpc();

        FindObjectOfType<AudioManager>().Play("StartMusic");
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
}