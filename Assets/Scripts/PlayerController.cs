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

    [Header("Movement Inputs")]
    [SerializeField, Tooltip("The current recorded input for movement, this is not normalized.")]
    private Vector2 currentMovementInput = Vector2.zero;
    [SerializeField, Tooltip("Should the movement inputs be checked")]
    private bool pollMovementInput = true;

    [Header("Directional Inputs")]
    [SerializeField, Tooltip("The current recorded input for movement, this is not normalized")]
    private Vector2 currentDirectionalInput = Vector2.zero;
    [SerializeField, Tooltip("Should the mouse inputs me checked")]
    private bool pollDirectionalInput = true;

    [Header("Button Inputs")]
    [SerializeField, Tooltip("Should Button Inputs be checked?")]
    private bool pollButtonInputs = true;

    [Header("Dash Inputs")]
    [SerializeField]
    private bool pollDashInputs = true;
    [SerializeField, Tooltip("The keyboard key assigned to dash")]
    private KeyCode dashKeyboardKey = KeyCode.LeftShift;
    [SerializeField, Tooltip("The controller button assigned to dash")]
    private string dashControllerBinding = "Left Bumper";

    [Header("Attack Inputs")]
    [SerializeField]
    private bool pollAttackInputs = true;
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

    [SerializeField, Tooltip("Is the player currently moving?")]
    private bool isMoving = false;

    [Header("Sprite Flip")]
    [SerializeField]
    private SpriteDirections spriteDirection = SpriteDirections.Right;
    private SpriteDirections lastSpriteDirection;

    [SerializeField]
    private bool doSpriteFlip = true;

    private GameObject playerSpriteOBJ;
    #endregion

    #endregion

    //------------------------------

    #region Init Functions
    private void Start()
    {
        currentMovementInput = Vector2.zero;

        lastSpriteDirection = spriteDirection;

        ConnectComponents();

        SetUpPlayerSpriteOBJ();
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
    private void SetUpPlayerSpriteOBJ()
    {
        if(anims == null)
        {
            Debug.LogError("ERROR - Could not get player sprite obj as anims is null.");
            return;
        }
            
        playerSpriteOBJ = anims.gameObject;
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

        if(pollButtonInputs)
        {
            PollButtonInputs(delta);
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

    #region Button Inputs
    private void PollButtonInputs(float delta)
    {
        if(currentInputType == InputTypes.KeyboardAndMouse)
        {
            PollKeyboardButtonInputs(delta);
            return;
        }

        if(currentInputType == InputTypes.Controller)
        {
            PollControllerButtonInputs(delta);
            return;
        }

        return;
    }

    #region Keyboard
    private void PollKeyboardButtonInputs(float delta)
    {
        if(pollDashInputs == true)
        {
            PollDashKeyboardInputs(delta);
        }

        return;
    }
    private void PollDashKeyboardInputs(float delta)
    {
        if(Input.GetKeyDown(dashKeyboardKey))
        {
            OnDashInput(ButtonContext.Started);
            return;
        }

        if (Input.GetKeyUp(dashKeyboardKey))
        {
            OnDashInput(ButtonContext.Cancelled);
            return;
        }

        if (Input.GetKey(dashKeyboardKey))
        {
            OnDashInput(ButtonContext.Held);
            return;
        }

        return;
    }
    #endregion

    #region Controller
    private void PollControllerButtonInputs(float delta)
    {
        if(pollDashInputs)
        {
            PollDashControllerInputs(delta);
        }
    }
    private void PollDashControllerInputs(float delta)
    {
        //check for the controller input here
    }
    #endregion

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
        Vector2 moveValue = currentMovementInput.normalized * moveSpeed * delta;

        rb.MovePosition(transform.position + (Vector3)moveValue);

        isMoving = (moveValue.magnitude > 0);

        if(currentMovementInput.x < 0)
        {
            spriteDirection = SpriteDirections.Left;
        }
        else if(currentMovementInput.x > 0)
        {
            spriteDirection = SpriteDirections.Right;
        }
    }
    #endregion

    //------------------------------

    #region Button Events
    private void OnDashInput(ButtonContext context)
    {
        if(context != ButtonContext.Started)
        {
            return;
        }

        Debug.Log("Dash");
    }
    #endregion

    //------------------------------

    #region Animations
    private void AnimationUpdate(float delta)
    {
        DetermineAnimState(delta);

        FlipSprite(delta);
    }
    private void DetermineAnimState(float Delta)
    {
        if (isMoving)
        {
            anims.SetInteger(animationParamter, (int)AnimStates.Moving);
            return;
        }

        anims.SetInteger(animationParamter, (int)AnimStates.Idle);
    }
    private void FlipSprite(float delta)
    {
        if (doSpriteFlip == false)
        {
            return;
        }

        if (lastSpriteDirection == spriteDirection)
        {
            return;
        }
        
        lastSpriteDirection = spriteDirection;

        playerSpriteOBJ.transform.localScale = 
            new Vector3(playerSpriteOBJ.transform.localScale.x * -1.0f, playerSpriteOBJ.transform.localScale.y, playerSpriteOBJ.transform.localScale.z);

        return;
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
[System.Serializable]
public enum AnimStates
{
    Idle,
    Moving,
}
[System.Serializable]
public enum SpriteDirections
{
    Left,
    Right
}
public enum ButtonContext
{
    None,
    Started,
    Held,
    Cancelled
}
#endregion
