using System.Collections;

using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    [Header("Player Health and Damage")]
    private float PlayerHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float PlayerSpeed;

    [Header("Player Things")]
    public NavMeshAgent PlayerAgent;
    public Transform LookPoint;
    public GameObject ShootingRaycastArea;
    public Transform enemyBody;
    public LayerMask enemyLayer;
    public Transform Spawn;
    public Transform PlayerCharacter;


    [Header("Player Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;


    [Header("Player States")]
    public float visionRadius;
    public float shootingRadius;
    public bool enemyInvisionRadius;
    public bool enemyInshootingRadius;
    public ScoreManager scoreManager;

    [Header("Player Animation and Spark effect ")]
    public Animator anim;
    public ParticleSystem muzzleSpark;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;
    private void Awake()
    {
        PlayerAgent = GetComponent<NavMeshAgent>(); //we are loading the Player component 
        presentHealth = PlayerHealth;
    }



    // Update is called once per frame
    void Update()
    {
        enemyInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, enemyLayer);
        enemyInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, enemyLayer);

        //check if the enemy is inside the visionRadius or in shooting radius
        if (enemyInvisionRadius && !enemyInshootingRadius)
        {
            PursueEnemy();
        }
        if (enemyInvisionRadius && enemyInshootingRadius)
        {
            ShootEnemy();
        }

    }



    private void PursueEnemy()
    {
        // Check if playerBody is not null before trying to access it
        if ( PlayerAgent.SetDestination(enemyBody.position))
        {
            //animations
            anim.SetBool("Running", true);
            anim.SetBool("Shooting", false);
        }
        else
        {
            anim.SetBool("Running", false);
            anim.SetBool("Shooting", false);
        }
    }


    private void ShootEnemy()
    {
        PlayerAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);

        if (!previouslyShoot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);

            RaycastHit hit;
            if (Physics.Raycast(ShootingRaycastArea.transform.position, ShootingRaycastArea.transform.forward, out hit, shootingRadius))
            {
                Debug.Log("Shooting: " + hit.transform.name);

                Enemy enemy = hit.transform.GetComponent<Enemy>();
                 if (enemy != null)
                {
                    enemy.enemyHitDamage(giveDamage);
                   

                }
                anim.SetBool("Running", false);
                anim.SetBool("Shooting", true);
            }

            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }
      
    }



    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void PlayerAIHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        if (presentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        PlayerAgent.SetDestination(transform.position);
        PlayerSpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        enemyInvisionRadius = false;
        enemyInshootingRadius = false;

        //animations
        anim.SetBool("Die", true);
        anim.SetBool("Running", false);
        anim.SetBool("Shooting", false);
        Debug.Log("Player Dead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        scoreManager.enemyKills += 1;

        yield return new WaitForSeconds(5f);
        Debug.Log("Player Spawn");
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        presentHealth = 120f;
        PlayerSpeed = 1f;
        shootingRadius = 10f;
        visionRadius = 100f;
        enemyInvisionRadius = true;
        enemyInshootingRadius = false;

        //animations
        anim.SetBool("Die", false);
        anim.SetBool("Running", true);
        //spawn point
        PlayerCharacter.transform.position = Spawn.transform.position;
        PursueEnemy();

    }
}
