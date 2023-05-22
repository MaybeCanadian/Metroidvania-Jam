using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public EntityAnims anims = null;

    public float moveSpeed = 10.0f;
    public Vector2 velocity = Vector2.zero;

    public Vector2 input = Vector2.zero;

    private void Awake()
    {
        anims = GetComponent<EntityAnims>();
    }

    public void SetInput(Vector2 newInput)
    {
        input = newInput;

        return;
    }

    private void FixedUpdate()
    {
        
    }
}
