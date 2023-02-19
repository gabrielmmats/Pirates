using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour

{

    [Header("Input Config")]
    [SerializeField]
    InputAction moveForwardAction;
    [SerializeField]
    InputAction rotateAction, singleShotAction, tripleShotAction;

    [Header("Movement Config")]
    [SerializeField]
    float movementSpeed = 1f;
    [SerializeField]
    float rotationSpeed = 1f;

    [Header("Destruction")]
    [SerializeField]
    float fadeTime = 5f;

    Rigidbody2D body2D;
    Collider2D col2D;
    PlayerActions playerActions;
    readonly (float, float) x_boundaries = (-55, 50);
    readonly (float, float) y_boundaries = (-40, 45);

    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        playerActions = GetComponent<PlayerActions>();
    }

    void OnEnable()
    {
        moveForwardAction.Enable();
        rotateAction.Enable();
        singleShotAction.Enable();
        tripleShotAction.Enable();
    }

    void Update()
    {
        var rotate = rotateAction.ReadValue<float>();
        transform.Rotate(new Vector3(0, 0, rotate * rotationSpeed * Time.deltaTime));
        var move = moveForwardAction.ReadValue<float>();
        Vector2 new_position;
        if (move == 1)
        {
            new_position = transform.position + transform.up * movementSpeed * Time.deltaTime;
            new_position = new Vector3(Mathf.Clamp(new_position.x, x_boundaries.Item1, x_boundaries.Item2), Mathf.Clamp(new_position.y, y_boundaries.Item1, y_boundaries.Item2));
            body2D.MovePosition(new_position);
        }  
        if (singleShotAction.triggered)
            playerActions.SingleShot();
        if (tripleShotAction.triggered)
            playerActions.TripleShot();
    }

    void DisableInput()
    {
        moveForwardAction.Disable();
        rotateAction.Disable();
        singleShotAction.Disable();
        tripleShotAction.Disable();
    }

    public void DestroyShip()
    {
        Destroy(gameObject, fadeTime);
        DisableShip();      
    }

    public void DisableShip()
    {
        DisableInput();
        col2D.enabled = false;
        body2D.velocity = Vector3.zero;
        body2D.angularVelocity = 0;
        body2D.isKinematic = true;
    }

    private void OnDestroy()
    {
        GameObject manager = GameObject.Find("MatchManager");
        if (manager != null)
            manager.SendMessage("EndMatch");
    }
}
