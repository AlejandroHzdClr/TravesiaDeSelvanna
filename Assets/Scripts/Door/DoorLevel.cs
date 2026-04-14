using System;
using Managers;
using UnityEngine;

namespace Door
{
    public class DoorLevel : MonoBehaviour
    {
        [SerializeField] private Vector3 savedPosition;
        [SerializeField] private Vector3 savedOrientation;
        [SerializeField] private int sceneNumber;
        [SerializeField] private bool isDoor;

        private bool playerCanInteract;

        private void OnEnable()
        {
            EventManager.OnInteract += TryOpenDoor;
        }

        private void OnDisable()
        {
            EventManager.OnInteract -= TryOpenDoor;
        }

        private void TryOpenDoor()
        {
            if (playerCanInteract)
            {
                GameManager.Instance.LoadNextScene(savedPosition,savedOrientation,sceneNumber);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!isDoor)
                {
                    GameManager.Instance.LoadNextScene(savedPosition,savedOrientation,sceneNumber);
                }
                else
                {
                    playerCanInteract = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (isDoor)
                {
                    playerCanInteract = false;
                }
            }
        }
    }
}
