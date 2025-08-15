using UnityEngine;
using System.Collections.Generic;

public class ComboSystem : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private float comboTimeWindow = 1.5f; // Time window to continue combo
    [SerializeField] private float maxComboMultiplier = 2.0f; // Maximum damage multiplier
    [SerializeField] private AnimationCurve comboMultiplierCurve = AnimationCurve.Linear(0, 1, 10, 2); // Damage multiplier based on combo count

    [Header("Combo Feedback")]
    [SerializeField] private AudioClip comboSound;
    [SerializeField] private ParticleSystem comboVFX;

    private float lastPunchTime;
    private int currentCombo;
    private List<PunchType> currentComboSequence = new List<PunchType>();

    public enum PunchType
    {
        LeftJab,
        RightJab,
        LeftHook,
        RightHook,
        LeftUppercut,
        RightUppercut
    }

    // Predefined combo patterns and their multipliers
    private readonly Dictionary<string, float> comboBonuses = new Dictionary<string, float>
    {
        {"LeftJab,RightJab", 1.2f}, // Basic 1-2 combo
        {"LeftJab,RightJab,LeftHook", 1.5f}, // 1-2-3 combo
        {"LeftJab,RightJab,LeftUppercut", 1.8f}, // Advanced combo
        {"LeftUppercut,RightUppercut", 1.6f}, // Double uppercut
    };

    public void RegisterPunch(PunchType punchType)
    {
        float currentTime = Time.time;

        // Check if combo should reset
        if (currentTime - lastPunchTime > comboTimeWindow)
        {
            ResetCombo();
        }

        // Add punch to sequence
        currentComboSequence.Add(punchType);
        currentCombo++;
        lastPunchTime = currentTime;

        // Check for special combo patterns
        CheckComboPatterns();

        // Trigger feedback
        TriggerComboFeedback();
    }

    private void CheckComboPatterns()
    {
        string currentPattern = string.Join(",", currentComboSequence);
        foreach (var combo in comboBonuses)
        {
            if (currentPattern.EndsWith(combo.Key))
            {
                // Trigger special combo effect
                TriggerSpecialCombo(combo.Value);
                break;
            }
        }
    }

    private void TriggerSpecialCombo(float bonusMultiplier)
    {
        // Play special combo effect
        if (comboVFX != null)
        {
            comboVFX.Play();
        }

        if (comboSound != null)
        {
            AudioSource.PlayClipAtPoint(comboSound, transform.position);
        }
    }

    private void TriggerComboFeedback()
    {
        // Basic combo feedback
        if (currentCombo > 1)
        {
            // Could trigger different effects based on combo count
            if (comboVFX != null)
            {
                var emission = comboVFX.emission;
                emission.rateOverTime = currentCombo * 2;
                comboVFX.Play();
            }
        }
    }

    public float GetCurrentComboMultiplier()
    {
        // Get base multiplier from combo count
        float baseMultiplier = comboMultiplierCurve.Evaluate(currentCombo);

        // Check for special combo patterns
        string currentPattern = string.Join(",", currentComboSequence);
        foreach (var combo in comboBonuses)
        {
            if (currentPattern.EndsWith(combo.Key))
            {
                baseMultiplier *= combo.Value;
                break;
            }
        }

        // Clamp to max multiplier
        return Mathf.Min(baseMultiplier, maxComboMultiplier);
    }

    private void ResetCombo()
    {
        currentCombo = 0;
        currentComboSequence.Clear();
    }

    // Public getter for UI display
    public int GetCurrentCombo()
    {
        return currentCombo;
    }
}
