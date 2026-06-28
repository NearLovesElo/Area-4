using UnityEngine;

/// <summary>
/// Put this on any empty GameObject in the scene (e.g. "IntroSequence").
/// Plays the player's wake-up lines automatically when the scene starts.
/// This does NOT touch ObjectiveManager because there's no objective yet at
/// this point - the first objective only appears after the radio broadcast.
/// </summary>
public class IntroSequence : MonoBehaviour
{
    [TextArea]
    [SerializeField]
    private string[] wakeUpLines =
    {
        "Ugh... my head hurts.",
        "The last thing I remember was a huge explosion... then a shockwave hit me.",
        "...What happened?",
        "(Static Radio Noises)",
        "I should listen to the radio"
    };

    [SerializeField]
    private GameObject objectiveUI;

    private void Start()
    {
        DialogueManager.Instance.PlayLines(wakeUpLines, OnIntroDialogueFinished);
    }

    private void OnIntroDialogueFinished()
    {
            objectiveUI.SetActive(true);
    }
}
