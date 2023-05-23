using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public EntityAnims anims;
    public EntityMovement movement;
    public EntityDirection direction;

    private void Awake()
    {
        if(anims == null)
            anims = GetComponentInChildren<EntityAnims>();
        if(movement == null)
            movement = GetComponentInChildren<EntityMovement>();
        if(direction == null)
            direction = GetComponentInChildren<EntityDirection>();
    }

    private void Update()
    {
        Vector2 movementInput = PollInput();

        movement.SetInput(movementInput);

        HandleDirection(movementInput);
    }

    private void LateUpdate()
    {
        movement.HandleMovementAnims();
    }

    private Vector2 PollInput()
    {
        Vector2 input = Vector2.zero;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        return input;
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
