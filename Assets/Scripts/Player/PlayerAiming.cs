using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public Vector3 direction = Vector3.zero;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    public void DetermineRotation(Vector2 mousePos)
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);

        mouseWorldPos.z = 0;

        direction = (mouseWorldPos - transform.position).normalized;
    }
}
