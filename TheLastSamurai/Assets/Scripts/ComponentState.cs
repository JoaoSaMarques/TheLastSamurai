using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentState : MonoBehaviour
{
    public GameObject dialogueBox; // Reference to the DialogueBox object

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(true); // Set the DialogueBox object to active
        }
    }
}