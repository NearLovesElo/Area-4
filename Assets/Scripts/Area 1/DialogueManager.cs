using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject joystickBackground;
    public GameObject interactButton;
    public GameObject objectiveUIBackground;
    public TextMeshProUGUI objectiveTitleText;
    public TextMeshProUGUI objectiveBodyText;

    [Header("Typing Settings")]
    public float typingSpeed = 0.04f;

    // ── Intro dialogue ──────────────────────────────────────────────
    private string[] introLines = new string[]
    {
        "Ugh... my head hurts.",
        "The last thing I remember was a huge explosion... then a shockwave hit me.",
        "...What happened?",
        "(Radio static)",
        "I should check the radio."
    };

    // ── Radio broadcast ─────────────────────────────────────────────
    private string[] radioLines = new string[]
    {
        "Emergency broadcast. Three days ago, the Area 1 Nuclear Power Plant exploded, releasing radioactive contamination across Areas 1, 2, and 3.",
        "Many exposed survivors have become infected and hostile.",
        "If you're hearing this, you are still alive. Stay indoors whenever possible. Radiation exposure outside will gradually damage your health.",
        "Scavenge for food to satisfy hunger, water to prevent dehydration, and medicine to slow the effects of radiation sickness.",
        "If you find a working vehicle, use it. Faster travel means less exposure.",
        "Area 4 remains safe. Emergency containment barriers prevented the contamination from reaching the region, and a treatment is available there.",
        "We cannot send help. Your only chance of survival is to reach Area 4.",
        "Good luck, survivor."
    };

    // ── State ────────────────────────────────────────────────────────
    private string[] activeLines;
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool dialogueActive = false;
    private bool isRadioBroadcast = false;
    private Coroutine typingCoroutine;

    // Reference to interact button logic
    private RadioInteract radioInteract;

    void Start()
    {
        radioInteract = FindFirstObjectByType<RadioInteract>();

        // Disable everything except dialogue at start
        joystickBackground.SetActive(false);
        interactButton.SetActive(false);
        objectiveUIBackground.SetActive(false);

        dialoguePanel.SetActive(true);
        dialogueActive = true;

        activeLines = introLines;
        typingCoroutine = StartCoroutine(TypeLine(activeLines[currentLineIndex]));
    }

    void Update()
    {
        // ── Interact button press (radio) ────────────────────────────
        // Call this from your Interact Button's OnClick event instead if preferred
        // But if you want to detect it here, expose a public method (see below)

        if (!dialogueActive) return;

        bool inputDetected = Input.GetMouseButtonDown(0) ||
                             (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

        if (inputDetected)
        {
            if (isTyping)
            {
                // Instantly complete current line
                StopCoroutine(typingCoroutine);
                dialogueText.text = activeLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                AdvanceDialogue();
            }
        }
    }

    void AdvanceDialogue()
    {
        currentLineIndex++;

        if (currentLineIndex >= activeLines.Length)
        {
            if (isRadioBroadcast)
                EndRadioBroadcast();
            else
                EndIntroDialogue();
            return;
        }

        typingCoroutine = StartCoroutine(TypeLine(activeLines[currentLineIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // ── Called after intro dialogue finishes ─────────────────────────
    void EndIntroDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);

        // Enable controls + objective
        joystickBackground.SetActive(true);
        interactButton.SetActive(true);
        objectiveUIBackground.SetActive(true);

        if (objectiveTitleText) objectiveTitleText.text = "Objective";
        if (objectiveBodyText) objectiveBodyText.text = "Listen to radio";
    }

    // ── Called by Interact Button (assign in Inspector OnClick) ──────
    public void OnInteractPressed()
    {
        if (radioInteract == null || !radioInteract.IsPlayerInRange()) return;
        if (dialogueActive) return; // already in dialogue

        // Disable controls, show dialogue
        joystickBackground.SetActive(false);
        interactButton.SetActive(false);

        dialoguePanel.SetActive(true);
        dialogueActive = true;
        isRadioBroadcast = true;

        activeLines = radioLines;
        currentLineIndex = 0;
        typingCoroutine = StartCoroutine(TypeLine(activeLines[currentLineIndex]));
    }

    // ── Called after radio broadcast finishes ────────────────────────
    void EndRadioBroadcast()
    {
        dialogueActive = false;
        isRadioBroadcast = false;
        dialoguePanel.SetActive(false);

        // Re-enable controls
        joystickBackground.SetActive(true);
        interactButton.SetActive(true);

        // Update objective
        if (objectiveTitleText) objectiveTitleText.text = "Objective";
        if (objectiveBodyText) objectiveBodyText.text = "Get Food";
    }
}