using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [Header("Grapple Speeds")]
    [SerializeField]
    private float grappleRange = 10.0f;
    [SerializeField]
    private float grappleSpeed = 1.0f;

    [Header("Grapple Bools")]
    [SerializeField]
    private bool hooked = false;

    [Header("Collisions")]
    [SerializeField]
    private LayerMask grappleStopLayer;
    [SerializeField]
    private LayerMask grappleGrappleLayer;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject grappleHeadPrefab;

    private GameObject hookOBJ;
    private GrappleHead headScript;

    [Header("Connected Scripts")]
    [SerializeField]
    private PlayerAiming aim;

    #region Init Functions
    private void Awake()
    {
        if(aim == null)
            aim = GetComponentInChildren<PlayerAiming>();
    }
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

        headScript.SetUpHook(grappleSpeed, grappleRange, aim.direction, this, grappleStopLayer, grappleGrappleLayer);

        hookOBJ.SetActive(false);
    }
    #endregion

    #region Input Polling
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseGrappleHook();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PullToHead();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            PullToBase();
        }
    }
    #endregion

    #region Grapple Shooting
    private void UseGrappleHook()
    {
        hookOBJ.SetActive(true);

        headScript.SetUpHook(grappleSpeed, grappleRange, aim.direction, this, grappleStopLayer, grappleGrappleLayer);

        headScript.FireHook(transform.position);

        hooked = false;
    }
    public void GrappleHitWall()
    {
        hookOBJ.SetActive(false);
        Debug.Log("Wall");
    }
    public void GrappleHitGrappleOBJ()
    {
        Debug.Log("Grapple");

        hooked = true;
    }
    public void GrappleAtMaxRange()
    {
        hookOBJ.SetActive(false);
        Debug.Log("Range");
    }
    #endregion

    #region Grapple Pulling
    private void PullToHead()
    {
        if(hooked == false)
        {
            Debug.Log("Cannot pull to hook, not attached.");
            return;
        }

        transform.position = hookOBJ.transform.position;

        hooked = false;

        hookOBJ.SetActive(false);
    }
    private void PullToBase()
    {
        if(hooked == false)
        {
            Debug.Log("Cannot pull to base, not attached.");
            return;
        }


    }
    #endregion
}
