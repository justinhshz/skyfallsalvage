using System.Collections.Generic;
using UnityEngine;

public class ConnectorIntegrator : MonoBehaviour
{
    private Transform spawnContainer; // Container used to store newly generated merged objects
    private ConnectorData connectorData;
    private AirdropComponentController airdropComponentController;
    private bool isCurrentConnector = false;

    private HashSet<GameObject> collidingElements = new(); // Use HashSet to optimize efficiency

    private void Awake()
    {
        InitializeReferences();
    }

    private void Update()
    {
        // Regularly clean up the list by removing destroyed objects
        CleanUpDestroyedElements();
    }

    /// <summary>
    /// Sets this Connector as the current dropped object.
    /// </summary>
    public void SetAsCurrentConnector()
    {
        isCurrentConnector = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCurrentConnector)
        {
            airdropComponentController?.OnComponentLanded();
            isCurrentConnector = false;
        }

        // Check if the collided object has ElementIntegrator
        if (collision.gameObject.TryGetComponent(out ElementIntegrator _))
        {
            collidingElements.Add(collision.gameObject);
        }

        CheckForEquipmentGeneration();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collidingElements.Remove(collision.gameObject);
    }

    /// <summary>
    /// Initializes component references.
    /// </summary>
    private void InitializeReferences()
    {
        // Ensure spawnContainer exists
        spawnContainer = GameManager.instance.spawnContainer;

        // Ensure the object has ConnectorComponentData
        connectorData = GetComponent<ConnectorAttribute>().connectorData;

        // Get the AirdropComponentController instance
        airdropComponentController = FindObjectOfType<AirdropComponentController>();
    }

    /// <summary>
    /// Cleans up destroyed objects.
    /// </summary>
    private void CleanUpDestroyedElements()
    {
        collidingElements.RemoveWhere(element => element == null);
    }

    /// <summary>
    /// Checks if the conditions for generating equipment are met.
    /// </summary>
    private void CheckForEquipmentGeneration()
    {
        if (connectorData == null || connectorData.equipmentRecipes == null) return;

        // Only check once, generate equipment when the first matching recipe is found
        foreach (var recipe in connectorData.equipmentRecipes)
        {
            if (IsRecipeFulfilled(recipe))
            {
                GenerateEquipment(recipe);
                break; // Stop after finding the first matching recipe
            }
        }
    }

    /// <summary>
    /// Checks if the objects in the list meet the recipe requirements.
    /// </summary>
    private bool IsRecipeFulfilled(EquipmentRecipe recipe)
    {
        HashSet<ElementData> requiredElements = new(recipe.requiredElements);
        List<GameObject> matchingElements = new();

        foreach (var element in collidingElements)
        {
            if (element.TryGetComponent(out ElementAttribute elementAttribute) && elementAttribute.elementData != null)
            {
                if (requiredElements.Contains(elementAttribute.elementData))
                {
                    matchingElements.Add(element);
                    requiredElements.Remove(elementAttribute.elementData);

                    // If all required elements are satisfied
                    if (requiredElements.Count == 0)
                        return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Generates equipment and destroys related objects.
    /// </summary>
    private void GenerateEquipment(EquipmentRecipe recipe)
    {
        EquipmentData equipment = recipe.generatedEquipment;

        if (equipment == null || equipment.equipmentPrefab == null) return;

        // Get the list of elements that match the recipe
        List<GameObject> elementsToDestroy = GetElementsToDestroyForRecipe(recipe);

        if (elementsToDestroy.Count == 0) return;

        // Calculate the position for generation (the midpoint between the Connector and the matching Elements)
        Vector3 averagePosition = CalculateAveragePosition(elementsToDestroy);

        // Generate the equipment and increase the corresponding score
        GameObject generatedEquipment = Instantiate(equipment.equipmentPrefab, averagePosition, Quaternion.identity, spawnContainer);

        // Generate a random direction unit vector
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // Set the force magnitude
        float forceMagnitude = 3f;

        // Apply the force in the random direction
        generatedEquipment.TryGetComponent(out Rigidbody2D dynamicRigidbody);
        dynamicRigidbody.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);

        GameManager.instance.IncreaseScore(recipe.scoreValue);
        EquipmentNavigator.instance.AddEquipment(generatedEquipment);

        // Delete objects that meet the conditions
        foreach (var element in elementsToDestroy)
        {
            collidingElements.Remove(element);
            Destroy(element);
        }

        Destroy(gameObject);
    }

    private Vector3 CalculateAveragePosition(List<GameObject> elements)
    {
        Vector3 sumPosition = transform.position; // Position of the object that contains the Connector component
        foreach (var element in elements)
        {
            sumPosition += element.transform.position;
        }

        // Calculate the average position
        return sumPosition / (elements.Count + 1);
    }

    /// <summary>
    /// Gets the list of objects that satisfy the current recipe.
    /// </summary>
    private List<GameObject> GetElementsToDestroyForRecipe(EquipmentRecipe recipe)
    {
        List<GameObject> elementsToDestroy = new();
        List<GameObject> matchingElements = new();
        HashSet<ElementData> requiredComponents = new(recipe.requiredElements);

        foreach (var element in collidingElements)
        {
            if (element.TryGetComponent(out ElementAttribute elementAttribute) && elementAttribute.elementData != null)
            {
                if (requiredComponents.Contains(elementAttribute.elementData))
                {
                    matchingElements.Add(element);
                    requiredComponents.Remove(elementAttribute.elementData);

                    if (requiredComponents.Count == 0)
                    {
                        elementsToDestroy.AddRange(matchingElements);
                        return elementsToDestroy;
                    }
                }
            }
        }
        return elementsToDestroy;
    }
}
