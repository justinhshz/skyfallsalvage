using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles game state, score management, and scene transitions.
/// </summary>
public class GameManager : MonoBehaviour
{
    private class RankingEntry
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public RankingEntry(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    public static GameManager instance;
    internal AirdropComponentController airdropComponentController;

    [Header("Game Settings")]
    internal int currentScore;
    private int currentThreshold;
    private int componentScoreBonus;
    public Transform spawnContainer;
    [SerializeField] internal GameSettings gameSettings;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image gameOverPanel;
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Transform rankingBoard;
    [SerializeField] private Image countdownImage;

    private WaitForEndOfFrame waitForEndOfFrame = new();
    internal bool isGameOver = false;

    private List<RankingEntry> rankingEntry = new();
    private List<GameObject> elementsToDestroy = new();

    private void Awake()
    {
        // Implementing Singleton pattern with protection against duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void OnEnable()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        currentThreshold = gameSettings.scoreThreshold;
        UpdateScoreUI();
        UpdateRankingBoardUI();
    }

    private void Update()
    {
        UpdateCountdownUI();

        if (currentScore >= currentThreshold)
        {
            airdropComponentController.PauseMovementAndActivateNavigator();
            if (!isGameOver) AttributeUpgradeManager.instance.ShowUpgradePanel(ApplyAttributeUpgrade);
            currentThreshold += gameSettings.scoreThreshold;
        }
    }

    private void LateUpdate()
    {
        // Destroy all marked elements
        if (elementsToDestroy.Count > 0)
        {
            foreach (var element in elementsToDestroy)
            {
                if (element != null)
                {
                    Destroy(element);
                }
            }
            elementsToDestroy.Clear();
        }
    }

    /// <summary>
    /// Mark an element for destruction.
    /// </summary>
    public void MarkElementForDestruction(GameObject element)
    {
        if (!elementsToDestroy.Contains(element))
        {
            elementsToDestroy.Add(element);
        }
    }

    /// <summary>
    /// Update the fill amount of the countdown UI.
    /// </summary>
    private void UpdateCountdownUI()
    {
        if (airdropComponentController == null || countdownImage == null || airdropComponentController.isDropped) return;

        // Calculate fill amount (remaining time / maximum time)
        float fillAmount = Mathf.Clamp01(airdropComponentController.countdownTimer / airdropComponentController.countdownDuration);

        // Update the fill amount of the image
        countdownImage.fillAmount = fillAmount;
    }

    public void ResetCountdownTimerUI()
    {
        if (countdownImage != null)
        {
            countdownImage.fillAmount = 1f;
        }
    }

    /// <summary>
    /// Increases the current score and updates the UI if needed.
    /// </summary>
    public void IncreaseScore(int amount)
    {
        if (amount <= 0) return; // Early return to reduce nesting

        int totalAmount = amount + componentScoreBonus;
        currentScore += totalAmount;

        UpdateScoreUI();
    }

    /// <summary>
    /// Updates the score display on the UI.
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString("0");
        }
    }

    /// <summary>
    /// Handles game over state and scene reset.
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        airdropComponentController.PauseMovementOnly();

        // Check if the current score qualifies for the ranking board
        int rank = CheckRank(currentScore);

        if (rank >= 0 && rank < 3)
        {
            ShowNameInputPanel(rank);
        }

        else
        {
            StartCoroutine(ResetGameCoroutine());
            ResetGameState();
        }

        #if UNITY_EDITOR
        Debug.Log("Game Over!");
        #endif
    }

    /// <summary>
    /// Checks if the current score qualifies for the ranking board.
    /// </summary>
    /// <param name="score">The score to check.</param>
    /// <returns>The rank (0-based index) or -1 if not qualified.</returns>
    private int CheckRank(int score)
    {
        rankingEntry.Add(new RankingEntry("Temp", score));
        rankingEntry.Sort((a, b) => b.Score.CompareTo(a.Score));
        if (rankingEntry.Count > 3) rankingEntry.RemoveAt(3);
        return rankingEntry.FindIndex(entry => entry.Score == score);
    }

    /// <summary>
    /// Shows the name input panel for players who entered the ranking board.
    /// </summary>
    /// <param name="rank">The player's rank.</param>
    private void ShowNameInputPanel(int rank)
    {
        airdropComponentController.PauseMovementOnly();
        nameInputPanel.SetActive(true);
        nameInputPanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Congratulations!\nYou are #{rank + 1}! Enter your name:";
        nameInputField.onEndEdit.AddListener(name =>
        {
            SubmitName(rank, name);
            nameInputField.onEndEdit.RemoveAllListeners(); // Clear all listeners
            nameInputField.text = string.Empty;
            nameInputPanel.SetActive(false);
            StartCoroutine(ResetGameCoroutine());
            ResetGameState();
        });
    }

    /// <summary>
    /// Resets the game state for the next game.
    /// </summary>
    private void ResetGameState()
    {
        isGameOver = false;
        currentScore = 0;
        componentScoreBonus = 0;
        currentThreshold = gameSettings.scoreThreshold;
        UpdateScoreUI();
        CleanSpawnContainer();
        airdropComponentController.ResumeMovementForReset();
    }

    /// <summary>
    /// Submits the player's name and updates the ranking board UI.
    /// </summary>
    private void SubmitName(int rank, string name)
    {
        if (rank < rankingEntry.Count)
        {
            rankingEntry[rank].Name = name;
        }
        UpdateRankingBoardUI();
    }

    private void CleanSpawnContainer()
    {
        foreach (Transform child in spawnContainer)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Updates the ranking board UI to reflect the latest leaderboard.
    /// </summary>
    private void UpdateRankingBoardUI()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < rankingEntry.Count)
            {
                Transform rankEntry = rankingBoard.GetChild(i);
                rankEntry.Find("[Text] Name").GetComponent<TextMeshProUGUI>().text = rankingEntry[i].Name;
                rankEntry.Find("[Text] Score").GetComponent<TextMeshProUGUI>().text = rankingEntry[i].Score.ToString();
            }
            else
            {
                // Clear unused entries
                Transform rankEntry = rankingBoard.GetChild(i);
                rankEntry.Find("[Text] Name").GetComponent<TextMeshProUGUI>().text = "-";
                rankEntry.Find("[Text] Score").GetComponent<TextMeshProUGUI>().text = "0";
            }
        }
    }

    /// <summary>
    /// Coroutine to handle game over fade out and scene reset.
    /// </summary>
    private IEnumerator ResetGameCoroutine()
    {
        gameOverPanel.gameObject.SetActive(true);
        yield return FadePanel(gameOverPanel, 0f, 1f, gameSettings.fadeTime);

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ApplyAttributeUpgrade(AttributeUpgrade upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case AttributeUpgrade.UpgradeType.CountdownDuration:
                airdropComponentController.countdownDuration += upgrade.value;
                Debug.Log($"Countdown Duration increased by {upgrade.value}");
                break;

            case AttributeUpgrade.UpgradeType.MovementSpeed:
                airdropComponentController.movementSpeed += upgrade.value;
                Debug.Log($"Movement Speed increased by {upgrade.value}");
                break;

            case AttributeUpgrade.UpgradeType.ComponentScore:
                componentScoreBonus += upgrade.value;
                Debug.Log($"Component Score increased by {upgrade.value}");
                break;
        }
    }

    /// <summary>
    /// Handles fading in the game when a new scene is loaded.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        airdropComponentController = FindObjectOfType<AirdropComponentController>();
        StartCoroutine(FadeGameInCoroutine());
    }

    /// <summary>
    /// Coroutine to fade in the game scene.
    /// </summary>
    private IEnumerator FadeGameInCoroutine()
    {
        airdropComponentController.isPaused = true;
        gameOverPanel.gameObject.SetActive(true);
        yield return FadePanel(gameOverPanel, 1f, 0f, gameSettings.fadeTime);
        gameOverPanel.gameObject.SetActive(false);
        airdropComponentController.isPaused = false;

        // Start horizontal movement coroutine
        airdropComponentController?.StartMovingHorizontally();
    }

    /// <summary>
    /// Fades the specified panel between two alpha values over time.
    /// </summary>
    private IEnumerator FadePanel(Image panel, float startAlpha, float endAlpha, float duration)
    {
        Color color = panel.color;
        color.a = startAlpha;
        panel.color = color;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            panel.color = color;
            yield return waitForEndOfFrame;
        }

        // Ensure the panel reaches the final alpha value
        color.a = endAlpha;
        panel.color = color;
    }
}
