using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField, Tooltip("The update cycle that inputs will be polled in.")]
    private UpdateTypes inputPollingUpdateType = UpdateTypes.Update;
    [SerializeField, Tooltip("The current active input type to be checked for.")]
    private InputTypes currentInputType = InputTypes.KeyboardAndMouse;

    [SerializeField, Tooltip("The current recorded input for movement, this is not normalized.")]
    private Vector2 currentMovementInput = Vector2.zero;
    [SerializeField, Tooltip("Should the movement inputs be checked")]
    private bool pollMovementInput = true;
    [SerializeField, Tooltip("Should the mouse inputs me checked")]
    private bool pollDirectionalInput = true;

    [Header("Movement")]
    [SerializeField, Tooltip("The update cycle that movement will occur in.")]
    private UpdateTypes movementUpdateType = UpdateTypes.FixedUpdate;

    [SerializeField, Tooltip("How fast the player will move when input is pressed.")]
    private float moveSpeed = 5.0f;

    [Header("Animations")]
    [SerializeField, Tooltip("The update cycle the animations will be updated in.")]
    private UpdateTypes animationUpdateType = UpdateTypes.LateUpdate;

    [SerializeField, Tooltip("The name of the parameter the animator is checking for.")]
    private string animationParamter = "AnimState";

    #region Init Functions
    private void Start()
    {
        currentMovementInput = Vector2.zero;
    }
    #endregion

    #region Updates
    private void Update()
    {
        float delta = Time.deltaTime;

        if (inputPollingUpdateType == UpdateTypes.Update)
        {
            InputPollingUpate(delta);
        }

        if (movementUpdateType == UpdateTypes.Update)
        {
            MovementUpdate(delta);
        }

        if (animationUpdateType == UpdateTypes.Update)
        {
            AnimationUpdate(delta);
        }
    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (inputPollingUpdateType == UpdateTypes.FixedUpdate)
        {
            InputPollingUpate(delta);
        }

        if (movementUpdateType == UpdateTypes.FixedUpdate)
        {
            MovementUpdate(delta);
        }

        if (animationUpdateType == UpdateTypes.FixedUpdate)
        {
            AnimationUpdate(delta);
        }
    }
    private void LateUpdate()
    {
        float delta = Time.deltaTime;

        if (inputPollingUpdateType == UpdateTypes.LateUpdate)
        {
            InputPollingUpate(delta);
        }

        if (movementUpdateType == UpdateTypes.LateUpdate)
        {
            MovementUpdate(delta);
        }

        if (animationUpdateType == UpdateTypes.LateUpdate)
        {
            AnimationUpdate(delta);
        }
    }
    #endregion

    #region Input
    private void InputPollingUpate(float delta)
    {
        if (pollMovementInput)
        {
            PollMovementInput(delta);
        }

        if(pollDirectionalInput)
        {
            PollDirectionalInput(delta);
        }
    }
    private void PollMovementInput(float delta)
    {
        if (currentInputType == InputTypes.KeyboardAndMouse)
        {
            PollKeyboardMovementInput(delta);
            return;
        }

        if(currentInputType == InputTypes.Controller)
        {
            PollControllerMovementInput(delta); 
            return;
        }

        //add other input types here
        return;
    }
    private void PollKeyboardMovementInput(float delta)
    {
        currentMovementInput.x = Input.GetAxisRaw("Horizontal");
        currentMovementInput.y = Input.GetAxisRaw("Vertical");
    }
    private void PollControllerMovementInput(float delta)
    {
        //controller stuff goes here
    }
    private void PollDirectionalInput(float delta)
    {

    }
    #endregion

    #region Movement
    private void MovementUpdate(float delta)
    {

    }
    #endregion

    #region Animations
    private void AnimationUpdate(float delta)
    {

    }
    #endregion
}

#region Enums
[System.Serializable]
public enum UpdateTypes
{
    Update,
    FixedUpdate,
    LateUpdate
}
[System.Serializable]
public enum InputTypes
{
    KeyboardAndMouse,
    Controller
}
#endregion
