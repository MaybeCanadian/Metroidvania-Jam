using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnims : MonoBehaviour
{
    public AnimStates startingAnim = AnimStates.IDLE;

    public AnimStates currentAnim = AnimStates.IDLE;

    public Animator anims;
    public string animParamName = "Anim";

    private void Awake()
    {
        if(anims == null)
            anims = GetComponentInChildren<Animator>();

        anims?.SetInteger(animParamName, (int)startingAnim);
        currentAnim = startingAnim;
    }

    public void GoToAnimState(AnimStates state)
    {
        if (anims == null)
        {
            Debug.LogError("ERROR - Animator is null");
            return;
        }

        if(currentAnim == state)
        {
            return;
        }

        //Debug.Log("Switching to anim state " + state);

        anims.SetInteger(animParamName, (int)state);

        currentAnim = state;
    }
}

[System.Serializable]
public enum AnimStates
{
    IDLE,
    WALK
}
