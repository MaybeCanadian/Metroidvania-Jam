using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public BaseAbility activeAbility;

    public void UseActiveAbility()
    {
        activeAbility?.Use(this);
    }
}
