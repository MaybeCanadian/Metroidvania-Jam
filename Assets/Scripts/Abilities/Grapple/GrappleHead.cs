using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrappleHead : MonoBehaviour
{
    [Header("Movemnet")]
    [SerializeField]
    float speed = 0.0f;
    [SerializeField]
    float range = 0.0f;
    [SerializeField]
    bool active = false;

    [SerializeField, Tooltip("This is used for the collision detection")]
    float collisionDetectRadius = 0.1f;

    GrappleHook hookBase;
    LayerMask stopLayer;
    LayerMask grappleLayer;

    private void Start()
    {
        active = false;
    }
    public void FireHook(float speed, float range, Vector3 direction, GrappleHook hookBase, LayerMask grappleStopLayer, LayerMask grappleGrappleLayer)
    {
        transform.position = hookBase.transform.position;

        this.speed= speed;
        this.range = range;

        transform.localEulerAngles = direction;

        this.hookBase = hookBase;

        this.stopLayer = grappleStopLayer;
        this.grappleLayer = grappleGrappleLayer;

        active = true;
    }

    public void FixedUpdate()
    {
        if (active == false)
        {
            return;
        }

        MoveHook(Time.fixedDeltaTime);

        CheckCollisions();

        CheckRange();
    }

    private void MoveHook(float delta)
    {
        transform.position += transform.forward * speed * delta;
    }
    private void CheckRange()
    {
        float dist = (transform.position - hookBase.transform.position).magnitude;

        if (dist > range)
        {
            active = false;
        }
    }
    private void CheckCollisions()
    {
        Collider2D stopCol = Physics2D.OverlapCircle(transform.position, collisionDetectRadius, stopLayer);

        if(stopCol != null)
        {
            active = false;
            Debug.Log("Wall");

            hookBase.GrappleHitWall();
        }

        Collider2D grappleCol = Physics2D.OverlapCircle(transform.position, collisionDetectRadius, grappleLayer);

        if(grappleCol != null)
        {
            active = false;
            Debug.Log("Grapple");

            hookBase.GrappleHitGrappleOBJ();
        }
    }
}
