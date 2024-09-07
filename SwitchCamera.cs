using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SwitchCamera : MonoBehaviour
{

    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;
    public PlayerScript player;

    [Header("Camera Animator")]
    public Animator animator;
         

    private void Update()
    {
        if(player.mobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButton("Aim") && player.currentPlayerSpeed >0)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("AimWalk", true);
                animator.SetBool("Walk", true);


                ThirdPersonCam.SetActive(false);
                ThirdPersonCanvas.SetActive(false);
                AimCam.SetActive(true);
                AimCanvas.SetActive(true);
            }
            else if (CrossPlatformInputManager.GetButton("Aim"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("AimWalk", false);
                animator.SetBool("Walk", false);

                ThirdPersonCam.SetActive(false);
                ThirdPersonCanvas.SetActive(false);
                AimCam.SetActive(true);
                AimCanvas.SetActive(true);

            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("IdleAim", false);
                animator.SetBool("AimWalk", false);

                ThirdPersonCam.SetActive(true);
                ThirdPersonCanvas.SetActive(true);
                AimCam.SetActive(false);
                AimCanvas.SetActive(false);
            }
        }
        else
        {

            if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("AimWalk", true);
                animator.SetBool("Walk", true);


                ThirdPersonCam.SetActive(false);
                ThirdPersonCanvas.SetActive(false);
                AimCam.SetActive(true);
                AimCanvas.SetActive(true);
            }
            else if (Input.GetButton("Fire2"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("AimWalk", false);
                animator.SetBool("Walk", false);

                ThirdPersonCam.SetActive(false);
                ThirdPersonCanvas.SetActive(false);
                AimCam.SetActive(true);
                AimCanvas.SetActive(true);

            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("IdleAim", false);
                animator.SetBool("AimWalk", false);

                ThirdPersonCam.SetActive(true);
                ThirdPersonCanvas.SetActive(true);
                AimCam.SetActive(false);
                AimCanvas.SetActive(false);
            }
        }

    }


















}
