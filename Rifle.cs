using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Rifle : MonoBehaviour
{
    [Header("Rifle")]
    public Camera cam;
    public float giveDamage = 10f; //how much a bullet causes damage 
    public float shootingRange = 100f;// how long the rifle can shoot
    public float fireCharge = 15f;
    public PlayerScript player;
    public Animator animator;

    [Header("Rifle Ammunition and shooting")]
    private float nextTimeToShoot = 0f;
    private int maximumAmmunition = 20; //bullets number 
    private int mag = 15; //magazines=dha5ira:contains bullets 
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;


    [Header("Rifle effects")]
    public ParticleSystem muzzleSpark;
    public GameObject WoodEffect; //impact
    public GameObject goreEffect;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;

    
    private void Awake()
    {
        presentAmmunition = maximumAmmunition;

        
    }


    
    private void Update()
    {
        if (setReloading)
        {
            return;

        }

        if (presentAmmunition <= 0) {
            StartCoroutine(Reload());
            return;
        
        }
        if(player.mobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButton("Shoot") && Time.time >= nextTimeToShoot)
            {
                animator.SetBool("Fire", true);
                animator.SetBool("Idle", false);

                nextTimeToShoot = Time.time + 1f / fireCharge; //1f/firecharge = 1/15 = 0.06 : the rifle will emit a ray every 0.06 s
                Shoot();
            }
            else if (CrossPlatformInputManager.GetButton("Shoot") && player.currentPlayerSpeed>0) // player.currentPlayerSpeed>0 :player walking
            {
                animator.SetBool("Idle", false);
                animator.SetBool("FireWalk", true);


            }
            else if (CrossPlatformInputManager.GetButton("Shoot") && CrossPlatformInputManager.GetButton("Aim"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("FireWalk", true);
                animator.SetBool("Walk", true);
                animator.SetBool("Reloading", false);

            }
            else
            {
                animator.SetBool("Fire", false);
                animator.SetBool("Idle", true);
                animator.SetBool("FireWalk", false);
            }

        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
            {
                animator.SetBool("Fire", true);
                animator.SetBool("Idle", false);

                nextTimeToShoot = Time.time + 1f / fireCharge; //1f/firecharge = 1/15 = 0.06 : the rifle will emit a ray every 0.06 s
                Shoot();
            }
            else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("FireWalk", true);


            }
            else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("FireWalk", true);
                animator.SetBool("Walk", true);
                animator.SetBool("Reloading", false);

            }
            else
            {
                animator.SetBool("Fire", false);
                animator.SetBool("Idle", true);
                animator.SetBool("FireWalk", false);
            }
        }
    }
    void Shoot()

    {
        if( mag == 0)
        {
            //show ammo out text
        }
        presentAmmunition--;

        if(presentAmmunition == 0 )
        {
            mag--;

        }

        //Update UI
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurrence.UpdateMagText(mag);


        muzzleSpark.Play(); //fire effect
        audioSource.PlayOneShot(shootingSound);
        RaycastHit hitInfo;
        //giving a ray to the main camera
        if(Physics.Raycast(cam.transform.position , cam.transform.forward , out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            Objects objects = hitInfo.transform.GetComponent<Objects>(); //references to the object class , so if we hit an obj the amount = 10f

            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

            if(objects != null)
            {
                objects.objectHitDamage(giveDamage);
                GameObject WoodGo = Instantiate(WoodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(WoodGo, 1f);
                //hitInfo.point : give the exact position where the bullet hits 
                //Quaternion: hia el rotation
                //LookRotation : gives us the forward (x) and "up" (y) direction
                //hitInfo.normal:it gives us The normal vector : is a vector that points directly away from the surface.
                //Quaternion.LookRotation(hitInfo.normal) creates a rotation that aligns the object’s
                //forward direction with the normal of the surface it hit. This means the object will be oriented
                //to face directly outwards from the surface.
            }
            else if (enemy != null)
            {
                enemy.enemyHitDamage(giveDamage);
                GameObject goreGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreGo, 1f);

            }
        }   
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading");

        //animation and audio 
        animator.SetBool("Reloading", true);
        audioSource.PlayOneShot(reloadingSound);

        yield return new WaitForSeconds(reloadingTime);

        //deactivate animations
        animator.SetBool("Reloading", false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;
        setReloading = false;
    }







}
