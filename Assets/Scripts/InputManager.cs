using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class InputManager : MonoBehaviour
{
    private InputManager inputManager;
    public InputManager.OnfootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

    void Awake()
    {
        inputManager = new InputManager();
        onFoot = inputManager.Onfoot;

        motor = GetComponent<PlayerMotor>();   
        look = GetComponent<PlayerLook>();
        
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
    }

    void FixedUpdate()
    {
       motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate() 
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }

    public InputManager.OnfootActions GetOnFootActions()
    {
        return onFoot;
    }
}
