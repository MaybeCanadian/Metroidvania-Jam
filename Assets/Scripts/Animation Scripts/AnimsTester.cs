using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimsTester : MonoBehaviour
{
    public EntityAnims anims;

    public float swapDelay = 1.0f;
    public float startDelay = 1.0f;

    public AnimStates startState = AnimStates.IDLE;
    public AnimStates currentState = AnimStates.IDLE;

    private void Awake()
    {
        anims = GetComponent<EntityAnims>();

        currentState = startState;

        InvokeRepeating("SwapToNextAnimState", startDelay, swapDelay);
    }

    public void SwapToNextAnimState()
    {
        switch(currentState)
        {
            case AnimStates.IDLE:
                currentState = AnimStates.WALK;
                break;
            case AnimStates.WALK:
                currentState = AnimStates.IDLE;
                break;
        }

        anims.GoToAnimState(currentState);

        return;
    }
}
