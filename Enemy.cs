using JetBrains.Annotations;
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{

    [Header("Enemy Health and Damage")]
    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float enemySpeed;

    [Header("Enemy Things")]
    public NavMeshAgent enemyAgent;
    public Transform LookPoint;
    public GameObject ShootingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    public Transform Spawn;
    public Transform EnemyCharacter;


    [Header("Enemy Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;


    [Header("Enemy States")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;
    public bool isPlayer = false;
    public ScoreManager scoreManager;

    [Header("Enemy Animation and Spark effect ")]
    public Animator anim;
    public ParticleSystem muzzleSpark;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;
    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>(); //we are loading the enemy component 
        presentHealth = enemyHealth;
    }



    // Update is called once per frame
    void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);

        //check if the enemy is inside the visionRadius or in shooting radius
        if (playerInvisionRadius && !playerInshootingRadius)
        {
            Pursueplayer();
        }
        if(playerInvisionRadius && playerInshootingRadius){
            ShootPlayer();
        }

    }

   

    private void Pursueplayer()
    {
        // Check if playerBody is not null before trying to access it
        if (playerBody != null && enemyAgent.SetDestination(playerBody.position))
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


    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);

        if(!previouslyShoot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);

            RaycastHit hit;
            if(Physics.Raycast(ShootingRaycastArea.transform.position, ShootingRaycastArea.transform.forward, out hit , shootingRadius))
            {
                Debug.Log("Shooting: " + hit.transform.name);

                if(isPlayer == true)
                {
                    PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();
                    if (playerBody != null)
                    {
                        playerBody.playerHitDamage(giveDamage);

                    }
                }
                else
                {
                    PlayerAI playerBody = hit.transform.GetComponent<PlayerAI>();
                    if (playerBody != null)
                    {
                        playerBody.PlayerAIHitDamage(giveDamage);

                    }
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

    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        if (presentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInvisionRadius = false;
        playerInshootingRadius = false;

        //animations
        anim.SetBool("Die", true);
        anim.SetBool("Running", false);
        anim.SetBool("Shooting", false);
        Debug.Log("Enemy Dead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        scoreManager.kills += 1;

        yield return new WaitForSeconds(5f);
        Debug.Log("Enemy Spawn");

        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        presentHealth = 120f;
        enemySpeed = 1f;
        shootingRadius = 10f;
        visionRadius = 100f;
        playerInvisionRadius = true;
        playerInshootingRadius = false;

        //animations
        anim.SetBool("Die", false);
        anim.SetBool("Running", true);
        //spawn point
        EnemyCharacter.transform.position = Spawn.transform.position;
        Pursueplayer();

    }

}
