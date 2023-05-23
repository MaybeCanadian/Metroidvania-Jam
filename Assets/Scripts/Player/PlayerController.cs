using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public EntityAnims anims;
    public EntityMovement movement;

    private void Awake()
    {
        anims = GetComponent<EntityAnims>();
        movement = GetComponent<EntityMovement>();
    }


}
