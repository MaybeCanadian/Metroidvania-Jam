using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDirection : MonoBehaviour
{
    public SpriteRenderer sr;

    private void Awake()
    {
        if(sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetDirection(SpriteDirections direction)
    {
        sr.flipX = direction == SpriteDirections.LEFT;

        return;
    }
}

public enum SpriteDirections
{
    RIGHT,
    LEFT
}
