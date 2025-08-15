using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject difficultyPanel;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public TextMeshProUGUI currentDifficultyText;
    public Button startFightButton;

    [Header("References")]
    public challengeAI aiBoxer;

    void Start()
    {
        // Setup button listeners
        easyButton.onClick.AddListener(() => SelectDifficulty(challengeAI.Difficulty.Easy));
        mediumButton.onClick.AddListener(() => SelectDifficulty(challengeAI.Difficulty.Medium));
        hardButton.onClick.AddListener(() => SelectDifficulty(challengeAI.Difficulty.Hard));
        startFightButton.onClick.AddListener(StartFight);

        // Show difficulty panel at start
        ShowDifficultyPanel();
    }

    public void SelectDifficulty(challengeAI.Difficulty difficulty)
    {
        if (aiBoxer != null)
        {
            aiBoxer.SetDifficulty(difficulty);
            currentDifficultyText.text = "Selected: " + difficulty.ToString();
        }
    }

    public void ShowDifficultyPanel()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game while selecting difficulty
        }
    }

    public void StartFight()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }
}
