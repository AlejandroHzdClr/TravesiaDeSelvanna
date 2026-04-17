using Managers;
using UnityEngine;

public class FinalEventController : MonoBehaviour
{
    [SerializeField] private GameObject normalSpawners;
    [SerializeField] private GameObject finalSpawners;
    [SerializeField] private GameObject finalDialogue;

    [Header("PlayerPosition")] 
    [SerializeField] private Vector3 targetPosition;

    private GameObject player;

    void Start()
    {
        if (GameManager.Instance.BooksList.Count >= 5)
        {
            normalSpawners.SetActive(false);

            player = GameObject.FindWithTag("Player");
            MovePlayerHere(targetPosition);

            finalSpawners.SetActive(true);
            finalDialogue.SetActive(true);
        }
    }

    private void MovePlayerHere(Vector3 vector)
    {
        player.transform.position = vector;
    }
}