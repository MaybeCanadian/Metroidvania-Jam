using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public EntityAnims anims;

    [Header("Movement Values")]
    [Tooltip("The rate at which velocity is gained per second")]
    public float moveSpeed = 3.0f;

    public MovementUpdateTypes updateType = MovementUpdateTypes.FixedUpdate;

    //The value set from a controller goes here
    private Vector2 input = Vector2.zero;
    private void Awake()
    {
        if(rb == null)
            rb = GetComponentInChildren<Rigidbody2D>();
        if(anims == null)
            anims = GetComponentInChildren<EntityAnims>();

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
        Move(delta);

        return;
    }
    #endregion

    #region Movement Loop
    private void Move(float delta)
    {
        input = input.normalized;

        Vector3 newPos = transform.position;

        newPos.x += input.x * moveSpeed * delta;
        newPos.y += input.y * moveSpeed * delta;

        rb.MovePosition(newPos);
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

    #region Animations
    public void HandleMovementAnims()
    {
        if(anims == null)
        {
            Debug.LogError("ERROR - Entity movement could not find animation script. Ignoring animations");
            return;
        }

        if(input.magnitude > 0)
        {
            anims.GoToAnimState(AnimStates.WALK);
        }
        else
        {
            anims.GoToAnimState(AnimStates.IDLE);
        }
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
