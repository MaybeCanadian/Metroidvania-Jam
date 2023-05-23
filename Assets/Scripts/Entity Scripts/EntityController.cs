using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public EntityAnims anims;
    public EntityMovement movement;

    private void Awake()
    {
        anims = GetComponent<EntityAnims>();
        movement = GetComponent<EntityMovement>();
    }

    private void LateUpdate()
    {
        if(movement.velocity.magnitude > 0)
        {
            anims.GoToAnimState(AnimStates.WALK);
        }
        else
        {
            anims.GoToAnimState(AnimStates.IDLE);
        }
    }
}
