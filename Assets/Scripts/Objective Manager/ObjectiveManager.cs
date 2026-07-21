using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One ordered list of objective steps for the whole intro/tutorial sequence.
/// This is the ONLY place that decides "is this objective already done".
///
/// Why this fixes your repeat-objective bug:
/// Triggering the radio again just calls TryCompleteStep("LISTEN_TO_RADIO") again.
/// Since that step is already marked complete, the manager ignores it and the
/// UI never updates - no matter how many times the player re-reads the radio.
///
/// To add a new step later: add ONE line to the _steps list below. Nothing else
/// in the codebase needs to change.
/// </summary>
public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [Serializable]
    public class Step
    {
        public string id;          // unique key, e.g. "LISTEN_TO_RADIO"
        public string title;       // shown in Objective Title text
        public string description; // shown in Objective Text

        [HideInInspector] public bool isComplete;
    }

    [Header("Define every objective step IN ORDER here")]
    [SerializeField]
    private List<Step> _steps = new List<Step>
    {
        new Step { id = "LISTEN_TO_RADIO", title = "Objective", description = "Listen to radio" },
        new Step { id = "GET_FOOD",        title = "Objective", description = "Get Food" },
        new Step { id = "EAT_FOOD",        title = "Objective", description = "Eat Food" },
        new Step { id = "GET_WATER",       title = "Objective", description = "Get Water" },
        new Step { id = "DRINK_WATER",     title = "Objective", description = "Drink Water" },
        new Step { id = "GET_MEDICINE",    title = "Objective", description = "Get Medicine" },
        new Step { id = "USE_MEDICINE",    title = "Objective", description = "Use Medicine" },
        new Step { id = "LEAVE_HOUSE",     title = "Objective", description = "Leave the House" },
        new Step { id = "WALK_TO_CAR",     title = "Objective", description = "Walk to the Car" },
        new Step { id = "FIND_CAR_PARTS",  title = "Objective", description = "Find Car Parts" },
        new Step { id = "REPAIR_CAR",      title = "Objective", description = "Repair the Car" },
        new Step { id = "REACH_AREA_4",    title = "Objective", description = "Survive & Reach Area 4" },
    };

    private readonly Dictionary<string, Step> _lookup = new Dictionary<string, Step>();
    private Step _activeStep; // the first incomplete step - this is what the UI shows

    /// <summary>Fired whenever the visible objective changes (new step, or all done = null).</summary>
    public event Action<Step> OnActiveStepChanged;

    private void Awake()
    {
        Instance = this;

        foreach (var step in _steps)
        {
            _lookup[step.id] = step;
        }
    }

    private void Start()
    {
        RecalculateActiveStep();
    }

    /// <summary>
    /// Call this whenever a gameplay event happens that MIGHT complete a step
    /// (e.g. radio interacted with, food eaten, car repaired).
    /// Safe to call repeatedly - already-completed steps are ignored, which is
    /// exactly what stops objectives from re-triggering.
    /// </summary>
    public void TryCompleteStep(string stepId)
    {
        if (!_lookup.TryGetValue(stepId, out var step))
        {
            Debug.LogWarning($"ObjectiveManager: no step with id '{stepId}'.");
            return;
        }

        if (step.isComplete) return; // already done - ignore silently, no repeat

        step.isComplete = true;
        RecalculateActiveStep();
    }

    public bool IsStepComplete(string stepId)
    {
        return _lookup.TryGetValue(stepId, out var step) && step.isComplete;
    }

    private void RecalculateActiveStep()
    {
        Step next = null;

        foreach (var step in _steps)
        {
            if (!step.isComplete)
            {
                next = step;
                break;
            }
        }

        _activeStep = next;
        OnActiveStepChanged?.Invoke(_activeStep);
    }
}
