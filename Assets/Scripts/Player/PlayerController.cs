using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public EntityAnims anims;
    public EntityMovement movement;
    public EntityDirection direction;
    public PlayerAiming aiming;

    private void Awake()
    {
        if(anims == null)
            anims = GetComponentInChildren<EntityAnims>();

        if(movement == null)
            movement = GetComponentInChildren<EntityMovement>();

        if(direction == null)
            direction = GetComponentInChildren<EntityDirection>();

        if(aiming == null)
            aiming = GetComponentInChildren<PlayerAiming>();

        return;
    }
    private void Update()
    {
        Vector2 movementInput = PollMovementInput();

        movement.SetInput(movementInput);

        HandleDirection(movementInput);

        Vector2 mouseInput = PollMouseInput();

        aiming.DetermineRotation(mouseInput);
    }
    private void LateUpdate()
    {
        movement.HandleMovementAnims();
    }
    private Vector2 PollMovementInput()
    {
        Vector2 input = Vector2.zero;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        return input;
    }
    private Vector2 PollMouseInput()
    {
        return Input.mousePosition;
    }
    private void HandleDirection(Vector2 input)
    {
        if(input.x > 0)
        {
            direction.SetDirection(SpriteDirections.RIGHT);
        }
        else if (input.x < 0)
        {
            direction.SetDirection(SpriteDirections.LEFT);
        }

        return;
    }
}
