using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTester : MonoBehaviour
{
    public EntityMovement movement;

    [Header("Timings")]
    public float startDelay = 1.0f;
    public float repeatDelay = 1.0f;

    [Header("Inputs")]
    public List<Vector2> inputList;
    private int inputIndex = 0;

    private void Start()
    {
        movement = GetComponent<EntityMovement>();

        if (inputList.Count > 0)
        {
            InvokeRepeating(nameof(CycleInputs), startDelay, repeatDelay);
        }
    }

    private void CycleInputs()
    {
        movement.SetInput(inputList[inputIndex]);

        inputIndex++;

        if(inputIndex >= inputList.Count)
        {
            inputIndex = 0;
        }

        return;
    }
}
