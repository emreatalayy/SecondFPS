using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    public float xSensitivity = 2f;  
    public float ySensitivity = 2f; 

    public float smoothTime = 0.2f;  
    public float sensitivityMultiplier = 0.5f; 

    private Vector2 currentMouseDelta; 
    private Vector2 currentMouseDeltaVelocity;

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * sensitivityMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityMultiplier;


        Vector2 targetMouseDelta = new Vector2(mouseX, mouseY);
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, smoothTime);

   
        ProcessLook(currentMouseDelta);
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * xSensitivity;
        float mouseY = input.y * ySensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
