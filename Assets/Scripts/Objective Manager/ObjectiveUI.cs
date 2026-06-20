using TMPro;
using UnityEngine;

/// <summary>
/// Put this on "Objective UI Background". Purely reacts to ObjectiveManager events -
/// it never decides what is complete or not, it just displays whatever the
/// manager says is currently active. Hides itself automatically when there is
/// no active step left (e.g. all objectives finished).
/// </summary>
public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;     // usually this same GameObject
    [SerializeField] private TMP_Text objectiveTitle;   // "Objective"
    [SerializeField] private TMP_Text objectiveText;    // "Listen to radio"

    private void OnEnable()
    {
        // Subscribe when enabled, in case this UI is toggled on/off elsewhere.
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.OnActiveStepChanged += HandleActiveStepChanged;
        }
    }

    private void OnDisable()
    {
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.OnActiveStepChanged -= HandleActiveStepChanged;
        }
    }

    private void HandleActiveStepChanged(ObjectiveManager.Step step)
    {
        if (step == null)
        {
            panelRoot.SetActive(false);
            return;
        }

        panelRoot.SetActive(true);
        objectiveTitle.text = step.title;
        objectiveText.text = step.description;
    }
}
