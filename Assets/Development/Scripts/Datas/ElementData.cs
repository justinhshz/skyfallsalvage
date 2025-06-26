using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Components/Element", order = 1)]
public class ElementData : ScriptableObject
{
    // public string componentName; // Name of the component
    public int componentLevel; // Component level (e.g., 1 for level one, 2 for level two)
    public Sprite elementSprite; // Displayed icon
    public GameObject elementPrefab; // Prefab of the element
    public ElementData subsequentElement; // Merged next element
    public int scoreValue; // Score gained when merging

    [TextArea] public string description; // Element description
}
