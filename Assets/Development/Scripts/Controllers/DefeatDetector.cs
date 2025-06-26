using UnityEngine;

/// <summary>
/// Detects if a "Component"-tagged object stays within the trigger for too long, causing a game over.
/// </summary>
public class DefeatDetector : MonoBehaviour
{   
    private float stayTimer = 0f; // Timer to track how long the object stays in the trigger
    private int componentCount = 0; // Counter to track the number of components in the trigger
    // private bool isTriggered = false; // Flag to track if the object is in the trigger

    private void Awake()
    {
        if (!TryGetComponent(out BoxCollider2D boxCollider))
        {
            Debug.LogError("DefeatDetector requires a BoxCollider2D component to function properly.");
            return;
        }

        if (!boxCollider.isTrigger)
        {
            Debug.LogWarning("DefeatDetector: BoxCollider2D should be set to 'Is Trigger'. Enabling it now.");
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Component"))
        {
            /* isTriggered = true; 
            stayTimer = 0f; // Reset the timer
            Debug.Log("Countdown started."); */

            componentCount++;

            if (componentCount == 1)
            {
                stayTimer = 0f; // Reset the timer when the first component enters
                // Debug.Log("Countdown started.");
            }
        }
    }

    /*
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (isTriggered && collider.CompareTag("Component"))
        {
            stayTimer += Time.deltaTime;
            Debug.Log("Countdown: " + stayTimer);

            if (stayTimer >= GameManager.instance.gameSettings.defeatTime)
            {
                GameManager.instance.GameOver();
                stayTimer = 0f; // Reset the timer after game over
            }
        }
    } */

    private void Update()
    {
        if (componentCount > 0)
        {
            stayTimer += Time.deltaTime;

            if (stayTimer >= GameManager.instance.gameSettings.defeatTime && !AttributeUpgradeManager.instance.isShown)
            {
                // Debug.Log("DefeatDetector: Game over triggered.");
                GameManager.instance.GameOver();

                // Reset the detector after game over
                stayTimer = 0f;
                componentCount = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Component"))
        {
            /*
            isTriggered = false;
            stayTimer = 0f; // Reset the timer
            Debug.Log("Countdown stopped."); */

            componentCount--;

            if (componentCount <= 0)
            {
                componentCount = 0;
                stayTimer = 0f; // Reset the timer when no components are in the trigger
                // Debug.Log("Countdown stopped.");
            }
        }
    }
}
