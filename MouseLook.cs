using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //this script is for camera mvt in mobile

    [Header("Min & Max Camera View")]
    private const float YMin = -50f;
    private const float YMax = 50f;

    //to make the cam look at the player 
    [Header("Camera view")]
    public Transform lookAt;
    public Transform player;

    //camera distance between player and cam
    [Header("Camera position")]
    public float CameraDistance = 10f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float cameraSensitivity = 4f;

    public FloatingJoystick floatingJoystick;

    private void LateUpdate()
    {
        currentX += floatingJoystick.Horizontal * cameraSensitivity * Time.deltaTime;
        currentY += floatingJoystick.Vertical * cameraSensitivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 direction = new Vector3(0, 0, -CameraDistance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * direction;
        transform.LookAt(lookAt.position);
    }





}
