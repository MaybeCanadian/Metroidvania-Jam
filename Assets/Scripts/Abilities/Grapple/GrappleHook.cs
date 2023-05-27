using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [Header("Grapple Speeds")]
    public float grappleRange = 10.0f;
    public float grappleSpeed = 1.0f;

    [Header("Collisions")]
    public LayerMask grappleStopLayer;
    public LayerMask grappleGrappleLayer;

    [Header("Prefabs")]
    public GameObject grappleHeadPrefab;

    private GameObject hookOBJ;
    private GrappleHead headScript;

    private void Start()
    {
        SetUpHook();
    }

    private void SetUpHook()
    {
        hookOBJ = Instantiate(grappleHeadPrefab, transform.position, transform.rotation);

        headScript = hookOBJ.GetComponent<GrappleHead>();

        if (headScript == null)
        {
            Debug.LogError("ERROR - Grapple Head Object is missing the grapple head script.");
            return;
        }

        hookOBJ.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            UseGrappleHook();
        }
    }

    private void UseGrappleHook()
    {
        hookOBJ.SetActive(true);

        headScript.FireHook(grappleSpeed, grappleRange, Vector3.left, this, grappleStopLayer, grappleGrappleLayer);
    }

    public void GrappleHitWall()
    {
        hookOBJ.SetActive(false);
    }

    public void GrappleHitGrappleOBJ()
    {
        
    }
}