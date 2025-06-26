using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentSelector : MonoBehaviour
{
    public static ComponentSelector instance; // Singleton pattern to ensure only one instance of ComponentSelector exists

    [Header("Component Lists")]
    [SerializeField] private List<ElementData> elements;  // Basic component list
    [SerializeField] private List<ConnectorData> connectors; // Connector component list

    [Header("UI Elements")]
    [SerializeField] private Image upcomingComponentImage; // Image to display the upcoming component

    private ScriptableObject currentComponent; // The currently active component in the game
    private ScriptableObject upcomingComponent; // The next component to drop

    [SerializeField, Range(0, 100)] private int elementProbability = 80; // Set the generation probability for basic components

    private int connectorCooldown = 0; // Tracks the generation cooldown for connector components

    private void Awake()
    {
        // Implementing Singleton pattern with protection against duplicates
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple instances of ComponentSelector detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        // DontDestroyOnLoad(gameObject);

        // Initialize currentComponent and upcomingComponent
        InitializeComponents();
    }

    /// <summary>
    /// Initialize currentComponent and upcomingComponent.
    /// </summary>
    private void InitializeComponents()
    {
        // Generate an initial upcomingComponent
        GenerateUpcomingComponent();

        // Set upcomingComponent as currentComponent and generate a new upcomingComponent
        currentComponent = upcomingComponent;
        GenerateUpcomingComponent();
    }

    /// <summary>
    /// Generates the current component and updates the next component.
    /// </summary>
    public void GenerateCurrentComponent()
    {
        // Set the next component as the current component
        currentComponent = upcomingComponent;

        // Generate a new upcomingComponent
        GenerateUpcomingComponent();
    }

    /// <summary>
    /// Randomly generate the next component and update the UI.
    /// </summary>
    private void GenerateUpcomingComponent()
    {
        // Ensure elementProbability is clamped between 0 and 100
        elementProbability = Mathf.Clamp(elementProbability, 0, 100);

        // Calculate connector component generation probability
        // int connectorProbability = 100 - elementProbability;

        // If in cooldown, force generate basic component
        if (connectorCooldown > 0)
        {
            if (elements.Count > 0)
            {
                upcomingComponent = elements[Random.Range(0, elements.Count)];
            }
            connectorCooldown--;
        }
        else
        {
            // Generate a random number between 0 and 99
            int randomValue = Random.Range(0, 100);

            // Determine the type of component to generate based on probability
            if (randomValue < elementProbability && elements.Count > 0)
            {
                // Generate a basic component
                upcomingComponent = elements[Random.Range(0, elements.Count)];
            }
            else if (randomValue >= elementProbability && connectors.Count > 0)
            {
                // Generate a connector component and enter cooldown
                upcomingComponent = connectors[Random.Range(0, connectors.Count)];
                connectorCooldown = 5; // Set cooldown to 5
            }
            else
            {
                // If unable to generate connector component, generate basic component
                if (elements.Count > 0)
                {
                    upcomingComponent = elements[Random.Range(0, elements.Count)];
                }
            }
        }

        // Update UI to display the next component
        UpdateUpcomingComponentUI();
    }

    /// <summary>
    /// Update UI to display the next component.
    /// </summary>
    private void UpdateUpcomingComponentUI()
    {
        if (upcomingComponent is ElementData element)
        {
            upcomingComponentImage.sprite = element.elementSprite;
        }
        else if (upcomingComponent is ConnectorData connector)
        {
            upcomingComponentImage.sprite = connector.connectorSprite;
        }
    }

    /// <summary>
    /// Get the currently selected component.
    /// </summary>
    /// <returns>ScriptableObject</returns>
    public ScriptableObject GetCurrentComponent()
    {
        return currentComponent;
    }

    /// <summary>
    /// Get the next component.
    /// </summary>
    /// <returns>ScriptableObject</returns>
    public ScriptableObject GetNextComponent()
    {
        return upcomingComponent;
    }
}
