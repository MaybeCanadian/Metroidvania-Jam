using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public EntityAnims anims;
    public EntityMovement movement;

    private void Awake()
    {
        if(anims == null)
            anims = GetComponent<EntityAnims>();
        if(movement == null)
            movement = GetComponent<EntityMovement>();
    }

    private void LateUpdate()
    {
        
    }
}
