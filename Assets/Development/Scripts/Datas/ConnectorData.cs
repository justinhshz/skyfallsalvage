using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentRecipe
{
    public ElementData[] requiredElements; // Basic components required to generate this device
    public EquipmentData generatedEquipment; // Generated special equipment
    public int scoreValue; // Score bonus when generating this equipment
}

[CreateAssetMenu(fileName = "Connector", menuName = "Components/Connector", order = 2)]
public class ConnectorData : ScriptableObject
{
    // public string componentName; // Connector component name
    public Sprite connectorSprite; // Displayed icon
    public GameObject connectorPrefab; // Prefab of the connector
    public List<EquipmentRecipe> equipmentRecipes; // List to store multiple recipes

    [TextArea] public string description; // Connector description
}
