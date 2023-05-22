using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnims : MonoBehaviour
{
    public AnimStates startingAnim = AnimStates.IDLE;

    public Animator anims;
    public string animParamName = "Anim";

    private void Awake()
    {
        anims = GetComponent<Animator>();

        anims?.SetInteger(animParamName, (int)startingAnim);
    }

    public void GoToAnimState(AnimStates state)
    {
        if (anims == null)
        {
            Debug.LogError("ERROR - Animator is null");
            return;
        }

        Debug.Log("Switching to anim state " + state);

        anims.SetInteger(animParamName, (int)state);
    }
}

[System.Serializable]
public enum AnimStates
{
    IDLE,
    WALK
}
