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

    [SerializeField, Tooltip("The connected Sprite Renderer")]
    private SpriteRenderer sr;
    [SerializeField, Tooltip("Should the controller try and find a connected Sprite Renderer in the object.")]
    private bool findSpriteRendererIfNull = true;
    [SerializeField, Tooltip("Should the controller check inside the children for the Sprite Renderer")]
    private bool checkChildForSpriteRenderer = true;

    private Camera mainCamera;

    private GameObject playerSpriteOBJ;
    #endregion

    #region Inputs
    [Header("Inputs")]
    [SerializeField, Tooltip("The update cycle that inputs will be polled in.")]
    private PlayerUpdateTypes inputPollingUpdateType = PlayerUpdateTypes.Update;
    [SerializeField, Tooltip("The current active input type to be checked for.")]
    private PlayerInputTypes currentInputType = PlayerInputTypes.KeyboardAndMouse;

    [Header("Movement Inputs")]
    [SerializeField, Tooltip("The raw movement Input")]
    private Vector2 rawMovementInput = Vector2.zero;
    [SerializeField, Tooltip("The normalized Movement Input")]
    private Vector2 normalizedMovementInput = Vector2.zero;
    [SerializeField, Tooltip("Should the movement inputs be checked")]
    private bool pollMovementInput = true;

    [Header("Directional Inputs")]
    [SerializeField, Tooltip("The raw Directional Input")]
    private Vector2 rawDirectionalInput = Vector2.zero;
    [SerializeField, Tooltip("The normalized Directional Input")]
    private Vector2 normalizedDirectionalInput = Vector2.zero;
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
    private PlayerUpdateTypes movementUpdateType = PlayerUpdateTypes.FixedUpdate;

    [SerializeField, Tooltip("How fast the player will move when input is pressed.")]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private float walkSpeed = 5.0f;
    #endregion

    #region Animations
    [Header("Animations")]
    [SerializeField, Tooltip("The update cycle the animations will be updated in.")]
    private PlayerUpdateTypes animationUpdateType = PlayerUpdateTypes.LateUpdate;

    [SerializeField, Tooltip("The name of the parameter the animator is checking for.")]
    private string animationParamter = "AnimState";

    [SerializeField, Tooltip("Is the player currently moving?")]
    private bool isMoving = false;

    [Header("Sprite Flip")]
    [SerializeField]
    private PlayerSpriteDirections transformDirection = PlayerSpriteDirections.Right;
    private PlayerSpriteDirections lastTransformDirection;

    [SerializeField]
    private bool doTransformFlip = true;

    [SerializeField]
    private PlayerSpriteDirections spriteDirection = PlayerSpriteDirections.Right;
    private PlayerSpriteDirections lastSpriteDirection;

    [SerializeField]
    private bool doSpriteFlip = true;
    #endregion

    #endregion

    //------------------------------

    #region Init Functions
    private void Start()
    {
        rawMovementInput = Vector2.zero;

        lastTransformDirection = transformDirection;

        lastSpriteDirection = spriteDirection;

        mainCamera = Camera.main;

        ConnectComponents();

        SetUpPlayerSpriteOBJ();
    }
    private void ConnectComponents()
    {
        ConnectRB();

        ConnectAnims();

        ConnectSR();
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
    private void ConnectSR()
    {
        if(sr != null)
        {
            return;
        }

        if(findSpriteRendererIfNull == false)
        {
            Debug.LogError("ERROR - Sprite Renderer for player is null.");
            return;
        }

        sr = (checkChildForSpriteRenderer == true) ? GetComponentInChildren<SpriteRenderer>() : GetComponent<SpriteRenderer>();

        if(sr != null)
        {
            return;
        }

        Debug.LogError("ERROR - Could not locate a Sprite Renderer in the player.");
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

        if (inputPollingUpdateType == PlayerUpdateTypes.Update)
        {
            InputPollingUpate(delta);
        }

        if (movementUpdateType == PlayerUpdateTypes.Update)
        {
            MovementUpdate(delta);
        }

        if (animationUpdateType == PlayerUpdateTypes.Update)
        {
            AnimationUpdate(delta);
        }
    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (inputPollingUpdateType == PlayerUpdateTypes.FixedUpdate)
        {
            InputPollingUpate(delta);
        }

        if (movementUpdateType == PlayerUpdateTypes.FixedUpdate)
        {
            MovementUpdate(delta);
        }

        if (animationUpdateType == PlayerUpdateTypes.FixedUpdate)
        {
            AnimationUpdate(delta);
        }
    }
    private void LateUpdate()
    {
        float delta = Time.deltaTime;

        if (inputPollingUpdateType == PlayerUpdateTypes.LateUpdate)
        {
            InputPollingUpate(delta);
        }

        if (movementUpdateType == PlayerUpdateTypes.LateUpdate)
        {
            MovementUpdate(delta);
        }

        if (animationUpdateType == PlayerUpdateTypes.LateUpdate)
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
        if (currentInputType == PlayerInputTypes.KeyboardAndMouse)
        {
            PollKeyboardMovementInput(delta);
            return;
        }

        if(currentInputType == PlayerInputTypes.Controller)
        {
            PollControllerMovementInput(delta); 
            return;
        }

        //add other input types here
        return;
    }
    private void PollKeyboardMovementInput(float delta)
    {
        rawMovementInput.x = Input.GetAxisRaw("Horizontal");
        rawMovementInput.y = Input.GetAxisRaw("Vertical");

        normalizedMovementInput = rawMovementInput.normalized;
    }
    private void PollControllerMovementInput(float delta)
    {
        //poll left stick input for this control type
    }
    #endregion

    #region Directional Inputs
    private void PollDirectionalInput(float delta)
    {
        if(currentInputType == PlayerInputTypes.KeyboardAndMouse)
        {
            PollMouseDirectionalInput(delta);
            return;
        }

        if(currentInputType == PlayerInputTypes.Controller)
        {
            PollControllerDirectionalInput(delta);
            return;
        }
    }
    private void PollMouseDirectionalInput(float delta)
    {
        Vector2 mousePos = Input.mousePosition;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);

        Vector3 direction = mouseWorldPos - transform.position;

        direction.z = 0;

        rawDirectionalInput = direction;

        normalizedDirectionalInput = rawDirectionalInput.normalized;
    }
    private void PollControllerDirectionalInput(float delta)
    {
        //poll right stick for this control type
    }
    #endregion

    #region Button Inputs
    private void PollButtonInputs(float delta)
    {
        if(currentInputType == PlayerInputTypes.KeyboardAndMouse)
        {
            PollKeyboardButtonInputs(delta);
            return;
        }

        if(currentInputType == PlayerInputTypes.Controller)
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

        CheckDirections(delta);
    }
    private void PlayerBasicMove(float delta)
    {
        Vector2 moveValue = normalizedMovementInput * moveSpeed * delta;

        rb.MovePosition(transform.position + (Vector3)moveValue);

        isMoving = (moveValue.magnitude > 0);

        if(rawMovementInput.x < 0)
        {
            transformDirection = PlayerSpriteDirections.Left;
        }
        else if(rawMovementInput.x > 0)
        {
            transformDirection = PlayerSpriteDirections.Right;
        }
    }

    #region Directions
    private void CheckDirections(float delta)
    {
        PlayerTransformDirectionCheck(delta);

        PlayerSpriteDirectionCheck(delta);
    }
    private void PlayerTransformDirectionCheck(float delta)
    {
        if (rawMovementInput.x < 0)
        {
            transformDirection = PlayerSpriteDirections.Left;
            return;
        }

        if (rawMovementInput.x > 0)
        {
            transformDirection = PlayerSpriteDirections.Right;
            return;
        }

        return;
    }
    private void PlayerSpriteDirectionCheck(float delta)
    {
        if (rawDirectionalInput.x > 0)
        {
            spriteDirection = PlayerSpriteDirections.Right;
            return;
        }

        if (rawDirectionalInput.x < 0)
        {
            spriteDirection = PlayerSpriteDirections.Left;
            return;
        }

        return;
    }
    #endregion

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

        FlipTransform(delta);

        FlipSprite(delta);
    }
    private void DetermineAnimState(float Delta)
    {
        if (isMoving)
        {
            anims.SetInteger(animationParamter, (int)PlayerAnimStates.Moving);
            return;
        }

        anims.SetInteger(animationParamter, (int)PlayerAnimStates.Idle);
    }

    #region Flips
    private void FlipTransform(float delta)
    {
        if (doTransformFlip == false)
        {
            return;
        }

        if (lastTransformDirection == transformDirection)
        {
            return;
        }

        if(playerSpriteOBJ == null)
        {
            return;
        }
        
        lastTransformDirection = transformDirection;

        playerSpriteOBJ.transform.localScale = 
            new Vector3(playerSpriteOBJ.transform.localScale.x * -1.0f, playerSpriteOBJ.transform.localScale.y, playerSpriteOBJ.transform.localScale.z);

        return;
    }
    private void FlipSprite(float delta)
    {
        if(doSpriteFlip == false)
        {
            return;
        }

        if (sr == null)
        {
            return;
        }

        sr.flipX = !(transformDirection == spriteDirection);

        return;
    }
    #endregion

    #endregion

    //------------------------------
}

#region Enums
[System.Serializable]
public enum PlayerUpdateTypes
{
    Update,
    FixedUpdate,
    LateUpdate
}
[System.Serializable]
public enum PlayerInputTypes
{
    KeyboardAndMouse,
    Controller
}
[System.Serializable]
public enum PlayerAnimStates
{
    Idle,
    Moving,
}
[System.Serializable]
public enum PlayerSpriteDirections
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
