using UnityEngine;

public class RadioInteract : MonoBehaviour
{
    private bool playerInRange = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    public bool IsPlayerInRange() => playerInRange;
}