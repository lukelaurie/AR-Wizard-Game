using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//keeps track of health, moveSpeed, animations, and following
//used for dragon enemies,etc
public class Enemy : MonoBehaviour
{
    //TODO: walk around, face player
    //TODO: jump attack, fireball attack, audio effects, etc.
    private float waitTime = 3.0f;
    private float timer = 0.0f;

    [SerializeField] float health;
    [SerializeField] float maxHealth = 200f;
    [SerializeField] HealthBar healthBar;


    //public NavMeshAgent enemy;
    //public Transform Player;

    public Animator enemyAnimator;

    /*[SerializeField] float moveSpeed = 5f;
    Rigidbody rb;
    Transform target;
    Vector2 moveDirection;*/

    private void Awake()
    {
        //rb = GetComponent<RigidBody>()
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        health = maxHealth;
        //need to find player object
        //target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
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

            // generate rand wait time between 4 and 6 sec to roar
            int randInt = Random.Range(6, 8);

            /*enemyAnimator.SetBool("Scream", true);
            //enemyAnimator.SetBool("Scream", false);
            float screamTime = 1.0f;
            float screamStartTime = 0.0f;
            screamStartTime += Time.deltaTime;
            if(screamStartTime >= screamTime)
            {
                enemyAnimator.SetBool("Scream", false);
            }*/
            enemyAnimator.Play("Scream");
            enemyAnimator.Play("Idle");
            //enemyAnimator.SetBool("Scream", false);

            //enemyAnimator.Play("Scream");
            //enemyAnimator.SetBool("Scream", false);
            //play a scream sound

            waitTime = (float)randInt;

            //reset timer
            timer = 0;

            Debug.Log($"{waitTime} seconds have passed!");
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
        int randInt = Random.Range(0, 5);

        //has a chance to randomly block an attack
        if (randInt == 0)
        {
            //Debug.Log("defended within Enemy.cs");
            enemyAnimator.Play("Defend");
            enemyAnimator.Play("Idle");
            //enemyAnimator.SetBool("Defend", true);
            //enemyAnimator.SetBool("Defend", false);
        }
        else
        {
            Debug.Log("took damage within Enemy.cs");
            enemyAnimator.Play("Get Hit");
            enemyAnimator.Play("Idle");
            //enemyAnimator.SetBool("GetHit", true);
            //enemyAnimator.SetBool("GetHit", false);

            health -= damageAmount;
            //play take damage animation
            healthBar.UpdateHealthBar(health, maxHealth);
            if (health <= 0)
            {
                //play death animation

                float newTime = 0.0f;
                float animTime = 3.0f;
                //have death animation be unineruptable
                enemyAnimator.Play("Die");
                StartCoroutine(WaitAndPerformAction());
            }
        }
    }

    private IEnumerator WaitAndPerformAction()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(3f);

        // Perform the action after the wait
        Debug.Log("play death animation");
        Destroy(gameObject);
        Debug.Log("Waited for 3 seconds!");
        // Add your logic here (e.g., triggering an animation, spawning an object, etc.)
    }
}
