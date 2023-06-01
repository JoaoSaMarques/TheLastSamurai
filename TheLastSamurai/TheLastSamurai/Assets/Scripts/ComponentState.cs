using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentState : MonoBehaviour
{
    public GameObject dialogueBox; // Reference to the DialogueBox object
    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            dialogueBox.SetActive(true); // Set the DialogueBox object to active
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(false); // Set the DialogueBox object to inactive
        }
    }
}
