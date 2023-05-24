using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Ability", menuName = "Abilties/Base", order = 1)]
public class BaseAbility : ScriptableObject
{
    public virtual void Use(PlayerAbility abilityOwner)
    {

    }
}
