using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Event Dispatchers

    #region Generic
    public delegate void PlayerEventGeneric();
    public static PlayerEventGeneric OnPlayerSpawned;
    public static PlayerEventGeneric OnPlayerDeath;
    #endregion

    #endregion

    //------------------------------

    #region Member Variables

    #region Components
    [Header("Components")]
    [SerializeField, Tooltip("The connected Rigid Body")]
    private Rigidbody2D rb;
    [SerializeField, Tooltip("Should the controller try and find a connected RB in the object.")]
    private bool findRigidBodyIfNull = true;
    [SerializeField, Tooltip("Should the controller check inside the children for the Rigid Body.")]
    private bool checkChildForRigidBody = true;

    [SerializeField, Tooltip("The connected Animator")]
    private Animator anims;
    [SerializeField, Tooltip("Should the controller try and find a connected Animator in the object.")]
    private bool findAnimsIfNull = true;
    [SerializeField, Tooltip("Should the controller check inside the children for the Animator")]
    private bool checkChildForAnims = true;
    #endregion

    #region Inputs
    [Header("Inputs")]
    [SerializeField, Tooltip("The update cycle that inputs will be polled in.")]
    private UpdateTypes inputPollingUpdateType = UpdateTypes.Update;
    [SerializeField, Tooltip("The current active input type to be checked for.")]
    private InputTypes currentInputType = InputTypes.KeyboardAndMouse;

    [SerializeField, Tooltip("The current recorded input for movement, this is not normalized.")]
    private Vector2 currentMovementInput = Vector2.zero;
    [SerializeField, Tooltip("The current recorded input for movement, this is not normalized")]
    private Vector2 currentDirectionalInput = Vector2.zero;

    [SerializeField, Tooltip("Should the movement inputs be checked")]
    private bool pollMovementInput = true;
    [SerializeField, Tooltip("Should the mouse inputs me checked")]
    private bool pollDirectionalInput = true;


    #endregion

    #region Movements
    [Header("Movement")]
    [SerializeField, Tooltip("The update cycle that movement will occur in.")]
    private UpdateTypes movementUpdateType = UpdateTypes.FixedUpdate;

    [SerializeField, Tooltip("How fast the player will move when input is pressed.")]
    private float moveSpeed = 5.0f;
    #endregion

    #region Animations
    [Header("Animations")]
    [SerializeField, Tooltip("The update cycle the animations will be updated in.")]
    private UpdateTypes animationUpdateType = UpdateTypes.LateUpdate;

    [SerializeField, Tooltip("The name of the parameter the animator is checking for.")]
    private string animationParamter = "AnimState";
    #endregion

    #endregion

    //------------------------------

    #region Init Functions
    private void Start()
    {
        currentMovementInput = Vector2.zero;

        ConnectComponents();
    }
    private void ConnectComponents()
    {
        ConnectRB();

        ConnectAnims();
    }
    private void ConnectRB()
    {
        if(rb != null)
        {
            return;
        }

        if(findRigidBodyIfNull == false)
        {
            Debug.LogError("ERROR - RB for the player controller is null.");
            return;
        }

        rb = (checkChildForRigidBody == true) ? GetComponentInChildren<Rigidbody2D>() : GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            return;
        }

        Debug.LogError("ERROR - Could not locate a Rigid Body in the player.");
        return;
    }
    private void ConnectAnims()
    {
        if(anims != null)
        {
            return;
        }

        if(findAnimsIfNull == false)
        {
            Debug.LogError("ERROR - Anims for player is null.");
            return;
        }

        anims = (checkChildForAnims == true) ? GetComponentInChildren<Animator>() : GetComponent<Animator>();

        if(anims != null)
        {
            return;
        }

        Debug.LogError("ERRIR - Could not locate an animator in the player.");
        return;
    }
    #endregion

    //------------------------------

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

    //------------------------------

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

    #region Movement Inputs
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
        //poll left stick input for this control type
    }
    #endregion

    #region Directional Inputs
    private void PollDirectionalInput(float delta)
    {
        if(currentInputType == InputTypes.KeyboardAndMouse)
        {
            PollMouseDirectionalInput(delta);
            return;
        }

        if(currentInputType == InputTypes.Controller)
        {
            PollControllerDirectionalInput(delta);
            return;
        }
    }
    private void PollMouseDirectionalInput(float delta)
    {

    }
    private void PollControllerDirectionalInput(float delta)
    {
        //poll right stick for this control type
    }
    #endregion

    #endregion

    //------------------------------

    #region Movement
    private void MovementUpdate(float delta)
    {
        PlayerBasicMove(delta);
    }
    private void PlayerBasicMove(float delta)
    {
        
    }
    #endregion

    //------------------------------

    #region Animations
    private void AnimationUpdate(float delta)
    {

    }
    #endregion

    //------------------------------
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
