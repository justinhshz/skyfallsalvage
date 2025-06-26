using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentNavigator : MonoBehaviour
{
    public static EquipmentNavigator instance;
    private List<GameObject> equipmentList = new();
    private GameObject currentSelectedEquipment;
    private GameObject previousSelectedEquipment;
    private PlayerInput playerInput;
    private InputAction navigateAction;
    private InputAction clickAction;

    private void Awake()
    {
        // Implementing Singleton pattern with protection against duplicates
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple instances of EquipmentNavigator detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        // DontDestroyOnLoad(gameObject);

        playerInput = FindObjectOfType<PlayerInput>();
        clickAction = playerInput.actions.FindAction("Click");
        // airdropComponentController = FindObjectOfType<AirdropComponentController>();
    }

    private void Update()
    {
        if (!AttributeUpgradeManager.instance.isShown && !GameManager.instance.isGameOver)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // If the ray hits an object
            if (hit.collider != null)
            {
                GameObject selectedEquipment = hit.collider.gameObject;
                if (selectedEquipment != null && selectedEquipment.TryGetComponent(out EquipmentAttribute equipmentAttribute))
                {
                    if (equipmentList.Contains(selectedEquipment) && equipmentAttribute.equipmentData.activationScore <= GameManager.instance.currentScore)
                    {
                        GameManager.instance.airdropComponentController.PauseMovementOnly();
                        if (selectedEquipment != currentSelectedEquipment)
                        {
                            if (previousSelectedEquipment != null && previousSelectedEquipment.TryGetComponent(out SpriteRenderer previousSprite))
                            {
                                previousSprite.color = Color.white;
                            }

                            currentSelectedEquipment = selectedEquipment;
                            previousSelectedEquipment = currentSelectedEquipment;

                            if (currentSelectedEquipment.TryGetComponent(out SpriteRenderer currentSprite))
                            {
                                currentSprite.color = Color.red;
                            }
                        }

                        if (clickAction.WasPressedThisFrame())
                        {
                            // Debug.Log($"Equipment clicked: {selectedEquipment.name}");
                            equipmentAttribute.equipmentData.ActivateEquipmentEffect(selectedEquipment);
                            DestroyEquipment(selectedEquipment);
                            currentSelectedEquipment = null;
                            previousSelectedEquipment = null;
                            GameManager.instance.airdropComponentController.ResumeMovementAndDeactivateNavigator();
                        }
                    }
                }
            }

            // If no objects are hit
            else
            {
                if (currentSelectedEquipment != null)
                {
                    GameManager.instance.airdropComponentController.ResumeMovementOnly();
                    currentSelectedEquipment.TryGetComponent(out SpriteRenderer spriteRenderer);
                    spriteRenderer.color = Color.white;
                    currentSelectedEquipment = null;
                    previousSelectedEquipment = null;
                }
            }
        }
    }

    public void AddEquipment(GameObject equipment)
    {
        equipmentList.Add(equipment);
    }

    private void DestroyEquipment(GameObject equipment)
    {
        equipmentList.Remove(equipment);
        Destroy(equipment);
    }
}
