using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookAbility : BaseAbility
{
    public float grappleRange = 0.0f;
    public GameObject grappleHookPrefab;

    public override void Use(PlayerAbility abilityOwner)
    {
        Debug.Log("use grapple hook with range of " + grappleRange);
    }
}
