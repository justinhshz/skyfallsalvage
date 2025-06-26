using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttributeUpgradeManager : MonoBehaviour
{
    public static AttributeUpgradeManager instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button option1Button;
    [SerializeField] private Button option2Button;
    [SerializeField] private Button option3Button;
    [SerializeField] private TextMeshProUGUI option1TitleText;
    [SerializeField] private TextMeshProUGUI option2TitleText;
    [SerializeField] private TextMeshProUGUI option3TitleText;
    [SerializeField] private TextMeshProUGUI option1Description;
    [SerializeField] private TextMeshProUGUI option2Description;
    [SerializeField] private TextMeshProUGUI option3Description;

    [Header("Attribute Upgrades")]
    [SerializeField] private AttributeUpgrade[] availableUpgrades;

    private AttributeUpgrade selectedUpgrade1;
    private AttributeUpgrade selectedUpgrade2;
    private AttributeUpgrade selectedUpgrade3;

    private Action<AttributeUpgrade> onUpgradeSelected;

    internal bool isShown = false;

    private void Awake()
    {
        // Implementing Singleton pattern with protection against duplicates
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple instances of AttributeUpgradeManager detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    private void Start()
    {
        upgradePanel.SetActive(false);
    }

    public void ShowUpgradePanel(Action<AttributeUpgrade> onUpgradeChosen)
    {
        isShown = true;
        onUpgradeSelected = onUpgradeChosen;
        
        GameManager.instance.airdropComponentController.PauseMovementAndActivateNavigator();
        List<AttributeUpgrade> upgradePool = new(availableUpgrades);
        selectedUpgrade1 = GetRandomUpgrade(upgradePool);
        selectedUpgrade2 = GetRandomUpgrade(upgradePool);
        selectedUpgrade3 = GetRandomUpgrade(upgradePool);

        option1TitleText.text = selectedUpgrade1.upgradeName;
        option2TitleText.text = selectedUpgrade2.upgradeName;
        option3TitleText.text = selectedUpgrade3.upgradeName;

        option1Description.text = selectedUpgrade1.description;
        option2Description.text = selectedUpgrade2.description;
        option3Description.text = selectedUpgrade3.description;

        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();
        option3Button.onClick.RemoveAllListeners();

        option1Button.onClick.AddListener(() => SelectUpgrade(selectedUpgrade1));
        option2Button.onClick.AddListener(() => SelectUpgrade(selectedUpgrade2));
        option3Button.onClick.AddListener(() => SelectUpgrade(selectedUpgrade3));

        upgradePanel.SetActive(true);
    }

    private AttributeUpgrade GetRandomUpgrade(List<AttributeUpgrade> pool)
    {
        int index = UnityEngine.Random.Range(0, pool.Count);
        AttributeUpgrade upgrade = pool[index];
        pool.RemoveAt(index);
        return upgrade;
    }

    private void SelectUpgrade(AttributeUpgrade upgrade)
    {
        isShown = false;
        upgradePanel.SetActive(false);
        GameManager.instance.airdropComponentController.ResumeMovementAndDeactivateNavigator();
        onUpgradeSelected?.Invoke(upgrade);
    }
}
