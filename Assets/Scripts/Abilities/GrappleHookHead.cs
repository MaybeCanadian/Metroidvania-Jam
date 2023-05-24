using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookHead : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Projectile Movement")]
    public Vector3 direction = Vector3.zero;
    public float moveSpeed = 1.0f;
    public LayerMask stoppingLayers;
    public LayerMask attachingLayers;
    public MovementUpdateTypes updateType = MovementUpdateTypes.FixedUpdate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (updateType != MovementUpdateTypes.Update)
        {
            return;
        }

        if (!CheckRB())
        {
            return;
        }

        MovementLoop(Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if(updateType != MovementUpdateTypes.FixedUpdate)
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

        if(!CheckRB())
        {
            return;
        }

        MovementLoop(Time.deltaTime);
    }
    private bool CheckRB()
    {
        if (rb == null)
        {
            Debug.LogError("ERROR - Grapple head is missing rigid body, movement is disabled.");
            return false;
        }

        return true;
    }
    private void MovementLoop(float delta)
    {

    }

}
