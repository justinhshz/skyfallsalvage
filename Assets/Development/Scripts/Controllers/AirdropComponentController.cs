using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the spawning and throwing of component objects in the game.
/// </summary>
public class AirdropComponentController : MonoBehaviour
{
    public static AirdropComponentController instance;

    [Header("Settings")]
    public Transform spawnPoint; // Spawn position
    private Transform spawnContainer; // Container for dropped objects

    [SerializeField] private Collider2D leftBoundary; // Left boundary
    [SerializeField] private Collider2D rightBoundary; // Right boundary

    [SerializeField] private GameSettings gameSettings;

    internal bool isDropped = false;
    internal bool isPaused = false; // Used to control the pause and resume of the coroutine
    private bool useComponentForBoundaryCheck = true; // Used to determine whether to use currentComponent for boundary checking

    internal float movementSpeed;
    internal float countdownDuration;
    internal float countdownTimer;

    private PlayerInput playerInput;
    private InputAction dropAction;

    private GameObject currentComponent;

    private Vector3 movementDirection = Vector3.left; // Initial movement direction is left

    private void Awake()
    {
        // Implementing Singleton pattern with protection against duplicates
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple instances of AirdropComponentController detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        instance = this;

        playerInput = FindObjectOfType<PlayerInput>();
        dropAction = playerInput.actions.FindAction("Drop");
        dropAction.Disable();
    }

    private void Start()
    {
        spawnContainer = GameManager.instance.spawnContainer;
        movementSpeed = gameSettings.movementSpeed;
        countdownDuration = gameSettings.countdownDuration;
        ResetCountdownTimer(); // Initialize countdown timer
        InstantiateCurrentComponent(); // Initialize and generate the first currentComponent
    }

    private void Update()
    {
        if (isPaused) return;

        countdownTimer -= Time.deltaTime;

        if (countdownTimer <= 0f || (!isDropped && dropAction.WasPressedThisFrame()))
        {
            DropCurrentComponent();
            ResetCountdownTimer();
            GameManager.instance.ResetCountdownTimerUI();
        }
    }

    /// <summary>
    /// Resets the countdown timer.
    /// </summary>
    private void ResetCountdownTimer()
    {
        countdownTimer = countdownDuration; // Reset countdown timer to default duration
    }

    /// <summary>
    /// Controls the horizontal movement of the object - Coroutine version.
    /// </summary>
    private IEnumerator MoveHorizontallyCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => !isPaused); // Pause the coroutine until isPaused is false

            if (leftBoundary == null || rightBoundary == null) yield break;

            // Move based on the current direction
            transform.Translate(movementSpeed * Time.deltaTime * movementDirection);

            if (BoundaryReached())
            {
                movementDirection = movementDirection == Vector3.left ? Vector3.right : Vector3.left;
            }
            yield return null;
        }
    }

    private bool BoundaryReached()
    {
        if (useComponentForBoundaryCheck && currentComponent.TryGetComponent(out CircleCollider2D collider))
        {
            float radius = collider.radius * currentComponent.transform.lossyScale.x;
            float leftEdge = currentComponent.transform.position.x - radius;
            float rightEdge = currentComponent.transform.position.x + radius;

            return (movementDirection == Vector3.left && leftEdge <= leftBoundary.bounds.max.x) ||
                   (movementDirection == Vector3.right && rightEdge >= rightBoundary.bounds.min.x);
        }

        return (movementDirection == Vector3.left && transform.position.x <= leftBoundary.bounds.max.x) ||
               (movementDirection == Vector3.right && transform.position.x >= rightBoundary.bounds.min.x);
    }

    public void StartMovingHorizontally()
    {
        StopAllCoroutines(); // Stop all current coroutines to prevent duplicate execution
        StartCoroutine(MoveHorizontallyCoroutine());
        dropAction.Enable();
    }

    /// <summary>
    /// Generates the currentComponent and initializes its state.
    /// </summary>
    private void InstantiateCurrentComponent()
    {
        ScriptableObject componentData = ComponentSelector.instance.GetCurrentComponent();
        GameObject prefab = GetPrefabFromComponentData(componentData);

        if (prefab == null) return;

        currentComponent = Instantiate(prefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        InitializeComponent(currentComponent, componentData);

        useComponentForBoundaryCheck = true;
    }

    private GameObject GetPrefabFromComponentData(ScriptableObject componentData)
    {
        return componentData switch
        {
            ElementData element => element.elementPrefab,
            ConnectorData connector => connector.connectorPrefab,
            _ => null
        };
    }

    private void InitializeComponent(GameObject component, ScriptableObject componentData)
    {
        if (component.TryGetComponent(out Rigidbody2D rigidbody)) rigidbody.simulated = false;
        if (component.TryGetComponent(out CircleCollider2D collider)) collider.isTrigger = true;

        if (componentData is ElementData && component.TryGetComponent(out ElementIntegrator elementIntegrator))
        {
            elementIntegrator.SetAsCurrentElement();
        }
        else if (componentData is ConnectorData && component.TryGetComponent(out ConnectorIntegrator connectorIntegrator))
        {
            connectorIntegrator.SetAsCurrentConnector();
        }
    }

    /// <summary>
    /// Makes the currentComponent start falling.
    /// </summary>
    private void DropCurrentComponent()
    {
        if (currentComponent == null) return;

        // Set currentComponent as a child of spawnContainer
        currentComponent.transform.SetParent(spawnContainer);

        // Enable Rigidbody2D to make the object fall
        if (currentComponent.TryGetComponent(out Rigidbody2D rigidbody)) rigidbody.simulated = true;

        // Cancel Collider's isTrigger state
        if (currentComponent.TryGetComponent(out CircleCollider2D collider)) collider.isTrigger = false;

        isDropped = true;

        // Use the object itself for boundary checking after it drops
        useComponentForBoundaryCheck = false;
    }

    public void OnComponentLanded()
    {
        if (isDropped)
        {
            // Update currentComponent and generate the next object
            ComponentSelector.instance.GenerateCurrentComponent();
            InstantiateCurrentComponent();
            isDropped = false;
            ResetCountdownTimer(); // Reset the countdown timer after generating a new object
        }
    }

    public void PauseMovementAndActivateNavigator() => SetPausedState(true, "Navigation");

    public void ResumeMovementAndDeactivateNavigator() => SetPausedState(false, "Gameplay");

    public void PauseMovementOnly() => SetPausedState(true);

    public void ResumeMovementOnly() => SetPausedState(false);

    public void ResumeMovementForReset() => SetPausedState(false, updatePausedState: false);

    /// <summary>
    /// Generalized method to pause or resume movement and optionally switch action maps.
    /// </summary>
    /// <param name="paused">Whether to pause or resume movement.</param>
    /// <param name="actionMap">Optional action map to switch to.</param>
    /// <param name="updatePausedState">Whether to update the `isPaused` state.</param>
    private void SetPausedState(bool paused, string actionMap = null, bool updatePausedState = true)
    {
        if (updatePausedState) isPaused = paused;

        if (paused)
        {
            dropAction.Disable();
        }
        else
        {
            dropAction.Enable();
        }

        if (!string.IsNullOrEmpty(actionMap)) playerInput.SwitchCurrentActionMap(actionMap);
    }
}
