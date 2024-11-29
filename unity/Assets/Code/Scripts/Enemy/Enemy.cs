using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

//keeps track of health, moveSpeed, animations, and following
//used for dragon enemies,etc
public class Enemy : NetworkBehaviour
{
    //TODO: walk around, face player (mabye)
    //TODO: jump attack, fireball attack, audio effects, etc.

    private float waitTime = 3.0f;
    private float timer = 0.0f;

    private NetworkVariable<float> health = new NetworkVariable<float>();
    private float maxHealth = 200f;
    [SerializeField] HealthBar healthBar;


    //public NavMeshAgent enemy;
    //public Transform Player;

    public Animator enemyAnimator;

    private bool canPlayAnim;
    private bool canBeHit;

    /*[SerializeField] float moveSpeed = 5f;
    Rigidbody rb;
    Transform target;
    Vector2 moveDirection;*/

    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip roarSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip defendSound;
    [SerializeField] private AudioClip rockAttackSound;

    [SerializeField] public GameObject fireball;
    [SerializeField] public GameObject rock;


    private void Awake()
    {
        //rb = GetComponent<RigidBody>()
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        FindObjectOfType<AudioManager>().Stop("StartMusic");
        FindObjectOfType<AudioManager>().Play("BossMusic");
        maxHealth = 200f;
        UpdateHealthServerRpc(maxHealth);
        canPlayAnim = true;
        canBeHit = true;

        // health = maxHealth;
        //need to find player object
        //target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //play take damage animation
        healthBar.UpdateHealthBar(health.Value, maxHealth);


        //enemy.SetDestination(Player.position);
        /*if(target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction,x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            moveDirection = direction;
        }*/

        timer += Time.deltaTime;

        if (timer >= waitTime)
        {

            // generate rand wait time between 6 and 8 sec to roar or attack
            int randTime = Random.Range(6, 9);

            int randAttack = Random.Range(0, 3);

            if (randAttack == 0)
            {
                //roar attack
                enemyAnimator.Play("Scream");
                AudioSource.PlayClipAtPoint(roarSound, transform.position, 1f);
                canPlayAnim = false;
                StartCoroutine(WaitAndPerformAction(3f));
                Debug.Log("roar attack");
            }
            else if (randAttack == 1)
            {
                //ground pound
                //play jump animation
                //play jump and impact sound
                // fling rocks around
                Debug.Log("ground pound");

                enemyAnimator.Play("Jump");
                canPlayAnim = false;
                StartCoroutine(WaitAndThrowObjects(2f, "rock"));
                AudioSource.PlayClipAtPoint(jumpSound, transform.position, 1f);
            }
            else if (randAttack == 2)
            {
                //fire ball attack
                //spin 360 and shoot fireballs around
                Debug.Log("dragon fireball attack");

                enemyAnimator.Play("Basic Attack");
                canPlayAnim = false;
                StartCoroutine(WaitAndThrowObjects(1f, "fireball"));
            }

            waitTime = (float)randTime;

            //reset timer
            timer = 0;
        }

    }

    private void SpawnObjectsAround(string name)
    {
        int numberOfObjects = 12;
        float radius = 2f;
        float speed = 6f;

        float angleStep = 360f / numberOfObjects;
        float angle = 0f;

        GameObject spawnObj;

        if(name == "fireball")
        {
            spawnObj = fireball;
        }
        else
        {
            AudioSource.PlayClipAtPoint(rockAttackSound, transform.position, 1f);
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

    //for physics
    private void FixedUpdate()
    {
        /*if(target)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }*/
    }

    public void TakeDamage(float damageAmount)
    {
        if(!canBeHit)
        {
            Debug.Log("Enemy can't be hit (dead)");
            return;
        }
        Debug.Log("Called TakeDamage in Enemy.cs");

        int randInt = Random.Range(0, 5);

        //has a chance to randomly block an attack
        if (randInt == 0)
        {
            Debug.Log("defended within Enemy.cs");

            if(canPlayAnim)
            {
                enemyAnimator.Play("Defend");
                AudioSource.PlayClipAtPoint(defendSound, transform.position, 1f);
                canPlayAnim = false;
                StartCoroutine(WaitAndPerformAction(2f));
            }
        }
        else
        {
            Debug.Log("took damage within Enemy.cs");

            if(canPlayAnim)
            {
                enemyAnimator.Play("Get Hit");
                AudioSource.PlayClipAtPoint(takeDamageSound, transform.position, 1f);

                canPlayAnim = false;
                StartCoroutine(WaitAndPerformAction(1.5f));
            }

            UpdateHealthServerRpc(health.Value - damageAmount);

            if (health.Value <= 0)
            {
                //TODO: PUT ALL OF THE WIN INFORMATION HERE
                //BOSS DIES HERE
                //!!!
                //!!!
                //!!!
                //PUT WIN SCREEN AND SO ON

                Debug.Log("Played death animation");

                //play for everyone
                FindObjectOfType<AudioManager>().Play("WinScreen");

                float newTime = 0.0f;
                float animTime = 3.0f;

                enemyAnimator.Play("Die");
                AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
                FindObjectOfType<AudioManager>().Stop("BossMusic");
                FindObjectOfType<AudioManager>().Play("StartMusic");
                StartCoroutine(WaitAndDestroy());
                canBeHit = false;
            }
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
        Debug.Log("Died after 3 seconds");
        FindObjectOfType<AudioManager>().Play("StartMusic");
    }

    private IEnumerator WaitAndPerformAction(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //enemyAnimator.Play("Idle");
        canPlayAnim = true;
    }

    private IEnumerator WaitAndThrowObjects(float seconds, string obj)
    {
        yield return new WaitForSeconds(seconds);

        SpawnObjectsAround(obj);
        //enemyAnimator.Play("Idle");
        canPlayAnim = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateHealthServerRpc(float newHealth)
    {
        health.Value = newHealth;
    }

}