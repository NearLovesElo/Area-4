using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Put this on your DialoguePanel (or a dedicated empty GameObject under Canvas).
/// Types out each line letter-by-letter. Clicking anywhere on screen while a
/// line is still typing instantly finishes that line. Clicking again once a
/// line is fully shown advances to the next line.
///
/// Setup: put a full-screen invisible Image on DialoguePanel (so it catches
/// clicks) and add an EventTrigger -> Pointer Click -> DialogueManager.
/// OnScreenClicked. That's the only UI wiring needed - no separate skip button.
///
/// Usage from anywhere:
///     DialogueManager.Instance.PlayLines(myStringArray, onFinished: () => { ... });
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Typewriter")]
    [Tooltip("Seconds between each revealed character.")]
    [SerializeField] private float typingSpeed = 0.03f;

    private readonly Queue<string> _lines = new Queue<string>();
    private Action _onFinished;

    private Coroutine _typingRoutine;
    private string _currentFullLine;   // the line currently being typed/displayed
    private bool _isLineFullyShown;
    private bool _isPlaying;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    /// <summary>Queues up a sequence of lines and shows the panel.</summary>
    public void PlayLines(IEnumerable<string> lines, Action onFinished = null)
    {
        _lines.Clear();
        foreach (var line in lines)
        {
            _lines.Enqueue(line);
        }

        _onFinished = onFinished;
        _isPlaying = true;

        dialoguePanel.SetActive(true);
        ShowNextLine();
    }

    /// <summary>
    /// Hook this to a Pointer Click (EventTrigger) on a full-screen invisible
    /// graphic placed on top of the DialoguePanel. This is the ONLY click
    /// entry point - it decides whether to finish-typing or advance.
    /// </summary>
    public void OnScreenClicked()
    {
        if (!_isPlaying) return;

        if (_isLineFullyShown)
        {
            ShowNextLine();          // 2nd click on this line: move on
        }
        else
        {
            FinishTypingInstantly(); // 1st click: snap to full line
        }
    }

    private void ShowNextLine()
    {
        if (_lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        _currentFullLine = _lines.Dequeue();

        if (_typingRoutine != null)
        {
            StopCoroutine(_typingRoutine);
        }

        _typingRoutine = StartCoroutine(TypeLine(_currentFullLine));
    }

    private IEnumerator TypeLine(string line)
    {
        _isLineFullyShown = false;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        _isLineFullyShown = true;
        _typingRoutine = null;
    }

    private void FinishTypingInstantly()
    {
        if (_typingRoutine != null)
        {
            StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }

        dialogueText.text = _currentFullLine;
        _isLineFullyShown = true;
    }

    private void EndDialogue()
    {
        _isPlaying = false;
        dialoguePanel.SetActive(false);

        var callback = _onFinished;
        _onFinished = null;
        callback?.Invoke();
    }

    public bool IsPlaying => _isPlaying;
}