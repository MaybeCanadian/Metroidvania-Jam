using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Movement Values")]
    [Tooltip("The rate at which velocity is gained per second")]
    public float moveSpeed = 10.0f;
    [Tooltip("The Max speed in any direction of the entity")]
    public float maxSpeed = 10.0f;
    [Tooltip("The current velocity of the entity")]
    public Vector2 velocity = Vector2.zero;
    [Tooltip("The amount the velocity decays each frame")]
    public float decay = 0.9f;

    public MovementUpdateTypes updateType = MovementUpdateTypes.FixedUpdate;

    //The value set from a controller goes here
    private Vector2 input = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if(rb == null)
        {
            Debug.LogError("ERROR - Entity named: " + gameObject.name + " is missing a rigid body. Movement will not be active");
        }
    }

    #region Input
    public void SetInput(Vector2 newInput)
    {
        input = newInput;

        return;
    }
    #endregion

    #region Updates
    private void Update()
    {
        if(updateType != MovementUpdateTypes.Update)
        {
            return;
        }

        if(!CheckRB())
        {
            return;
        }

        MovementLoop(Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if (updateType != MovementUpdateTypes.FixedUpdate)
        {
            return;
        }

        if (!CheckRB())
        {
            return;
        }

        MovementLoop(Time.fixedDeltaTime);
    }
    private void LateUpdate()
    {
        if (updateType != MovementUpdateTypes.LateUpdate)
        {
            return;
        }

        if (!CheckRB())
        {
            return;
        }

        MovementLoop(Time.deltaTime);
    }
    private void MovementLoop(float delta)
    {
        Accelerate(delta);

        CapMoveSpeed();

        Move(delta);

        ApplyDecay(delta);

        return;
    }
    #endregion

    #region Movement Loop
    private void Accelerate(float delta)
    {
        velocity.x += input.x * moveSpeed * delta;

        velocity.y += input.y * moveSpeed * delta;

        return;
    }
    private void CapMoveSpeed()
    {
        if(velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * moveSpeed;
        }
    }
    private void Move(float delta)
    {
        Vector3 newPos = transform.position;

        newPos.x += velocity.x * delta;
        newPos.y += velocity.y * delta;

        rb.MovePosition(newPos);
    }
    private void ApplyDecay(float delta)
    {
        //this should be modified to only happen when you are not pressing the button. I want snappy controls.

        velocity *= decay;

        return;
    }
    #endregion

    #region Debug
    private bool CheckRB()
    {
        if (rb == null)
        {
            Debug.LogError("ERROR - Entity named: " + gameObject.name + " is missing a rigidbody. Movement disabled.");

            return false;
        }

        return true;
    }
    #endregion
}

[System.Serializable]
public enum MovementUpdateTypes
{
    Update,
    FixedUpdate,
    LateUpdate
}
