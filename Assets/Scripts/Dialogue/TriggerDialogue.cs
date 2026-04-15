using System;
using Dialogue;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private DialogueController dialogueController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueController.lineas = dialogue;
        }
    }
}
