using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class challengeAI : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    [Header("Difficulty Settings")]
    public Difficulty currentDifficulty = Difficulty.Medium;
    
    [Header("UI Elements")]
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public GameObject difficultyPanel;
    public TextMeshProUGUI difficultyText;

    [Header("AI Stats")]
    private float attackSpeed;
    private float reactionTime;
    private float accuracy;
    private float damageMultiplier;

    [Header("Combat")]
    public float baseAttackDamage = 10f;
    public float health = 100f;
    private bool isAttacking = false;
    private Animator animator;

    void Start()
    {
        // Get the animator component
        animator = GetComponent<Animator>();
        
        // Setup difficulty selection buttons
        if (easyButton) easyButton.onClick.AddListener(() => SetDifficulty(Difficulty.Easy));
        if (mediumButton) mediumButton.onClick.AddListener(() => SetDifficulty(Difficulty.Medium));
        if (hardButton) hardButton.onClick.AddListener(() => SetDifficulty(Difficulty.Hard));

        // Initialize with default difficulty
        SetDifficulty(currentDifficulty);
    }

    void Update()
    {
        if (!isAttacking)
        {
            DecideAction();
        }
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
        UpdateAIStats();
        
        if (difficultyText)
        {
            difficultyText.text = "Difficulty: " + difficulty.ToString();
        }
    }

    private void UpdateAIStats()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                attackSpeed = 1.0f;
                reactionTime = 1.5f;
                accuracy = 0.6f;
                damageMultiplier = 0.8f;
                break;
            case Difficulty.Medium:
                attackSpeed = 1.5f;
                reactionTime = 1.0f;
                accuracy = 0.8f;
                damageMultiplier = 1.0f;
                break;
            case Difficulty.Hard:
                attackSpeed = 2.0f;
                reactionTime = 0.5f;
                accuracy = 0.95f;
                damageMultiplier = 1.2f;
                break;
        }
    }

    private void DecideAction()
    {
        // Random decision making based on difficulty
        float decisionValue = Random.value;
        
        if (decisionValue < accuracy)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Random attack type
        int attackType = Random.Range(0, 3);
        
        // Trigger different animations based on attack type
        switch (attackType)
        {
            case 0:
                if (animator) animator.SetTrigger("JabLeft");
                break;
            case 1:
                if (animator) animator.SetTrigger("JabRight");
                break;
            case 2:
                if (animator) animator.SetTrigger("Uppercut");
                break;
        }

        // Wait for attack animation
        yield return new WaitForSeconds(1f / attackSpeed);
        
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"AI took {damage} damage! Remaining health: {health}");
        if (health <= 0)
        {
            Debug.Log("AI Defeated!");
            gameObject.SetActive(false);
        }
    }

    public float GetAttackDamage()
    {
        float damage = baseAttackDamage * damageMultiplier;
        Debug.Log($"AI attacking with {damage} damage (Difficulty: {currentDifficulty})");
        return damage;
    }

    // Debug function to test AI behavior
    public void TestAIBehavior()
    {
        Debug.Log($"Testing AI on {currentDifficulty} difficulty");
        Debug.Log($"Attack Speed: {attackSpeed}");
        Debug.Log($"Reaction Time: {reactionTime}");
        Debug.Log($"Accuracy: {accuracy}");
        Debug.Log($"Damage Multiplier: {damageMultiplier}");
        
        // Trigger a test attack
        StartCoroutine(PerformAttack());
    }

    // Add keyboard input for testing
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Difficulty: {currentDifficulty}");
        GUI.Label(new Rect(10, 30, 200, 20), $"Health: {health}");
        
        if (GUI.Button(new Rect(10, 60, 100, 30), "Test Attack"))
        {
            TestAIBehavior();
        }
        
        if (GUI.Button(new Rect(10, 100, 100, 30), "Take Damage"))
        {
            TakeDamage(10f);
        }
    }
}
