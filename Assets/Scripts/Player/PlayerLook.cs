using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    public float xSensitivity = 2f;  // Hassasiyet daha düşük
    public float ySensitivity = 2f;  // Hassasiyet daha düşük

    public float smoothTime = 0.2f;   // Sönümleme süresi arttırıldı (daha yumuşak geçiş)
    public float sensitivityMultiplier = 0.5f; // Ekstra bir çarpan ile hassasiyeti daha da düşür

    private Vector2 currentMouseDelta; 
    private Vector2 currentMouseDeltaVelocity;

    void Update()
    {
        // Fare hareketini al
        float mouseX = Input.GetAxis("Mouse X") * sensitivityMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityMultiplier;

        // Fare hareketini sönümleme ile yumuşat
        Vector2 targetMouseDelta = new Vector2(mouseX, mouseY);
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, smoothTime);

        // Kamera dönüşünü işle
        ProcessLook(currentMouseDelta);
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * xSensitivity;
        float mouseY = input.y * ySensitivity;

        // Yukarı ve aşağı bakış için kamera rotasyonunu hesapla
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
