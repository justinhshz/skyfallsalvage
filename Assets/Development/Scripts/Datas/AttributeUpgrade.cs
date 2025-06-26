using UnityEngine;

[CreateAssetMenu(fileName = "AttributeUpgrade", menuName = "Game/Attribute Upgrade", order = 2)]
public class AttributeUpgrade : ScriptableObject
{
    [Header("Upgrade Information")]
    public string upgradeName;
    public string description;

    [Header("Upgrade Effect")]
    public UpgradeType upgradeType;
    public int value;

    public enum UpgradeType
    {
        CountdownDuration,
        MovementSpeed,
        ComponentScore
    }
}
