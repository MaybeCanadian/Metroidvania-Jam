using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDirection : MonoBehaviour
{
    public SpriteRenderer sr;
    private float startingScale = 1;

    private void Awake()
    {
        if(sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        startingScale = transform.localScale.x;
    }

    public void SetDirection(SpriteDirections direction)
    {
        Vector3 scale = new Vector3((direction == SpriteDirections.LEFT) ? (startingScale * -1) : startingScale, transform.localScale.y, transform.localScale.z); ;

        transform.localScale = scale;

        return;
    }
}

public enum SpriteDirections
{
    RIGHT,
    LEFT
}
