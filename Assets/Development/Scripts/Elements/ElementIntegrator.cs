using UnityEngine;

/// <summary>
/// Handles the combination of components when they collide and meet specified conditions.
/// </summary>
public class ElementIntegrator : MonoBehaviour
{
    private Transform spawnContainer; // Container used to store newly merged objects
    private ElementData elementData; // Current object's ElementData
    private AirdropComponentController airdropComponentController;
    private bool isCurrentElement = false; // Whether this is the current dropped object

    private void Awake()
    {
        InitializeReferences();
    }

    /// <summary>
    /// Set this object as the current dropped object.
    /// </summary>
    public void SetAsCurrentElement()
    {
        isCurrentElement = true;
    }

    /// <summary>
    /// Trigger collision event.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If this is the current dropped object, check if we need to notify AirdropComponentController
        if (isCurrentElement && !collision.gameObject.CompareTag("Boundary"))
        {
            airdropComponentController?.OnComponentLanded();
            isCurrentElement = false; // Ensure notification is sent only once
        }

        // Try to get ComponentIntegrator from the collided object and check if it can merge
        if (collision.gameObject.TryGetComponent(out ElementIntegrator other) && CanMergeWith(other))
        {
            // Ensure only one object performs the merge
            HandleMerge(collision);
        }
    }

    /// <summary>
    /// Initializes component references.
    /// </summary>
    private void InitializeReferences()
    {
        // Ensure spawnContainer has been assigned
        spawnContainer = GameManager.instance.spawnContainer;

        // Ensure the object has ElementAttribute
        elementData = GetComponent<ElementAttribute>().elementData;

        // Get AirdropComponentController instance
        airdropComponentController = FindObjectOfType<AirdropComponentController>();
    }

    /// <summary>
    /// Check if two objects can merge.
    /// </summary>
    private bool CanMergeWith(ElementIntegrator other)
    {
        // Check if the componentData of both objects is the same
        return elementData != null && other.elementData != null && elementData == other.elementData;
    }

    /// <summary>
    /// Handle merge logic.
    /// </summary>
    private void HandleMerge(Collision2D collision)
    {
        // Use object ID to determine which side performs the merge
        if (gameObject.GetInstanceID() > collision.gameObject.GetInstanceID())
        {
            PerformMerge(collision);
        }
    }

    /// <summary>
    /// Execute merge, generate new object and delete old object.
    /// </summary>
    private void PerformMerge(Collision2D collision)
    {
        // Check if there is a subsequent advanced object
        if (elementData?.subsequentElement == null) return;

        GameObject subsequentPrefab = elementData.subsequentElement.elementPrefab;
        if (subsequentPrefab == null) return;

        // Calculate spawn position
        Vector3 mergePosition = CalculateMergePosition(collision);

        // Generate new object
        GameObject spawnedElement = Instantiate(subsequentPrefab, mergePosition, Quaternion.identity, spawnContainer);

        // Directly set Rigidbody2D
        if (spawnedElement.TryGetComponent(out Rigidbody2D dynamicRigidbody))
        {
            dynamicRigidbody.simulated = true;

            // Generate random direction vector
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Set force magnitude
            float forceMagnitude = 3f;

            // Apply force in random direction
            dynamicRigidbody.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);
        }

        // Increase score
        GameManager.instance.IncreaseScore(elementData.scoreValue);

        // Delete merged objects
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// Calculates the position for spawning the merged element.
    /// </summary>
    private Vector3 CalculateMergePosition(Collision2D collision)
    {
        return (transform.position + collision.transform.position) / 2;
    }
}
