using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Components/Equipment", order = 3)]
public class EquipmentData : ScriptableObject
{
    [Header("Equipment Settings")]
    public Sprite equipmentSprite; // Displayed icon
    public GameObject equipmentPrefab; // Prefab of the equipment
    public int activationScore; // Score required to activate

    [Header("Equipment Effects")]
    public EquipmentEffect equipmentEffect; // Equipment effect

    [TextArea] public string description; // Equipment description

    public void ActivateEquipmentEffect(GameObject equipmentObject)
    {
        switch (equipmentEffect)
        {
            case EquipmentEffect.InstantUpgrade:
                InstantUpgrade(equipmentObject);
                break;
            case EquipmentEffect.ZoneReplacement:
                ZoneReplacement(equipmentObject);
                break;
            case EquipmentEffect.ScaleReduction:
                ScaleReduction(equipmentObject);
                break;
            case EquipmentEffect.ConnectorTransformation:
                ConnectorTransformation(equipmentObject);
                break;
        }
    }

    private void InstantUpgrade(GameObject equipmentObject)
    {
        // Get EquipmentActivator component
        EquipmentActivator activator = equipmentObject.GetComponent<EquipmentActivator>();
        if (activator == null) return;

        // Copy collidingElements to a temporary list
        List<GameObject> elementsToUpgrade = new(activator.GetCollidingElements());

        // Iterate through all colliding elements and upgrade them
        foreach (GameObject element in elementsToUpgrade)
        {
            if (element == null) continue; // Ensure element is not destroyed

            ElementAttribute elementAttribute = element.GetComponent<ElementAttribute>();
            if (elementAttribute != null && elementAttribute.elementData != null && elementAttribute.elementData.subsequentElement != null)
            {
                // Save element's position and parent
                Vector3 position = element.transform.position;
                Transform parent = element.transform.parent;
                GameObject nextElementPrefab = elementAttribute.elementData.subsequentElement.elementPrefab;

                // Destroy current element
                // Destroy(element);

                // Mark element for destruction and let GameManager handle it
                GameManager.instance.MarkElementForDestruction(element);

                // Generate upgraded element
                Instantiate(nextElementPrefab, position, Quaternion.identity, parent);

                #if UNITY_EDITOR
                Debug.Log($"Upgraded element to {elementAttribute.elementData.subsequentElement.name}");
                #endif
            }
        }

        // Clear the list of colliding elements
        activator.ClearCollidingElements();
    }

    private void ZoneReplacement(GameObject equipmentObject)
    {
        // Get EquipmentActivator component
        EquipmentActivator activator = equipmentObject.GetComponent<EquipmentActivator>();
        if (activator == null) return;

        // Get all colliding elements
        List<GameObject> elements = new(activator.GetCollidingElements());
        if (elements.Count == 0) return;

        // Find the lowest tier element
        GameObject lowestElement = FindLowestTierElement(elements);
        Debug.Log(lowestElement);
        if (lowestElement == null) return;

        ElementAttribute lowestElementAttribute = lowestElement.GetComponent<ElementAttribute>();
        if (lowestElementAttribute == null || lowestElementAttribute.elementData == null) return;

        GameObject lowestElementPrefab = lowestElementAttribute.elementData.elementPrefab;

        // Replace all colliding elements with the lowest tier element
        foreach (GameObject element in elements)
        {
            if (element == null) continue; // Ensure element is not destroyed

            ElementAttribute elementAttribute = element.GetComponent<ElementAttribute>();
            if (elementAttribute != null && elementAttribute.elementData != null)
            {
                // Save element's position and parent
                Vector3 position = element.transform.position;
                Transform parent = element.transform.parent;

                // Destroy current element
                // Destroy(element);

                // Mark element for destruction and let GameManager handle it
                GameManager.instance.MarkElementForDestruction(element);

                // Generate lowest tier element
                Instantiate(lowestElementPrefab, position, Quaternion.identity, parent);
            }
        }

        // Clear the list of colliding elements
        activator.ClearCollidingElements();

        #if UNITY_EDITOR
        Debug.Log($"Zone replacement completed. All elements replaced with {lowestElementAttribute.elementData.name}");
        #endif
    }

    private void ScaleReduction(GameObject equipmentObject)
    {
        // Get EquipmentActivator component
        EquipmentActivator activator = equipmentObject.GetComponent<EquipmentActivator>();
        if (activator == null) return;

        // Get all colliding elements
        List<GameObject> elements = new(activator.GetCollidingElements());
        List<GameObject> connectors = new(activator.GetCollidingConnectors());
        List<GameObject> equipments = new(activator.GetCollidingEquipments());
        if (elements.Count == 0 && connectors.Count == 0) return;

        // Scale down all colliding elements
        foreach (GameObject element in elements)
        {
            if (element == null) continue; // Ensure element is not destroyed

            ElementAttribute elementAttribute = element.GetComponent<ElementAttribute>();
            if (elementAttribute != null && elementAttribute.elementData != null)
            {
                // Scale down element
                element.transform.localScale *= 0.8f;
            }
        }

        // Scale down all colliding connectors
        foreach (GameObject connector in connectors)
        {
            if (connector == null) continue; // Ensure connector is not destroyed

            ConnectorAttribute connectorAttribute = connector.GetComponent<ConnectorAttribute>();
            if (connectorAttribute != null && connectorAttribute.connectorData != null)
            {
                // Scale down connector
                connector.transform.localScale *= 0.8f;
            }
        }

        // Scale down all colliding equipments
        foreach (GameObject equipment in equipments)
        {
            if (equipment == null) continue; // Ensure equipment is not destroyed

            EquipmentAttribute equipmentAttribute = equipment.GetComponent<EquipmentAttribute>();
            if (equipmentAttribute != null && equipmentAttribute.equipmentData != null)
            {
                // Scale down equipment
                equipment.transform.localScale *= 0.8f;
            }
        }

        // Clear the list of colliding elements
        activator.ClearCollidingElements();
        activator.ClearCollidingConnectors();
        activator.ClearCollidingEquipments();

        #if UNITY_EDITOR
        Debug.Log("All elements scaled down by 80%");
        #endif
    }

    /// <summary>
    /// Replace all Connectors with the lowest tier element that the equipment has collided with.
    /// </summary>
    private void ConnectorTransformation(GameObject equipmentObject)
    {
        // Get EquipmentActivator component
        EquipmentActivator activator = equipmentObject.GetComponent<EquipmentActivator>();
        if (activator == null) return;

        // Get all colliding elements
        List<GameObject> elements = new List<GameObject>(activator.GetCollidingElements());
        if (elements.Count == 0) return;

        // Find the lowest tier element
        GameObject lowestElementPrefab = FindLowestTierElement(elements);
        if (lowestElementPrefab == null) return;

        // Find all objects with ConnectorIntegrator
        ConnectorIntegrator[] connectors = FindObjectsOfType<ConnectorIntegrator>();

        foreach (ConnectorIntegrator connector in connectors)
        {
            if (connector == null) continue;

            // Save connector's position and parent
            Vector3 position = connector.transform.position;
            Transform parent = connector.transform.parent;

            // Instantiate the lowest tier element
            Instantiate(lowestElementPrefab, position, Quaternion.identity, parent);

            // Mark connector for destruction
            GameManager.instance.MarkElementForDestruction(connector.gameObject);
        }

        #if UNITY_EDITOR
        Debug.Log($"Transformed {connectors.Length} connectors to the lowest level element.");
        #endif

        // Clear the list of colliding elements
        activator.ClearCollidingElements();
    }

    /// <summary>
    /// Find the lowest tier element among the colliding elements.
    /// </summary>
    private GameObject FindLowestTierElement(List<GameObject> elements)
    {
        GameObject lowestElement = null;
        int lowestLevel = int.MaxValue;

        foreach (GameObject element in elements)
        {
            // Check if element is null to avoid accessing destroyed objects
            if (element == null) continue;

            ElementAttribute elementAttribute = element.GetComponent<ElementAttribute>();
            if (elementAttribute != null && elementAttribute.elementData != null)
            {
                int level = elementAttribute.elementData.componentLevel;
                if (level < lowestLevel)
                {
                    lowestLevel = level;
                    lowestElement = element;
                }
            }
        }

        return lowestElement;
    }
}

public enum EquipmentEffect
{
    InstantUpgrade,
    ZoneReplacement,
    ScaleReduction,
    ConnectorTransformation,
}
