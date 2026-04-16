using System.Collections;
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
        private bool isSubscribed = false;

        private void OnEnable()
        {
            StartCoroutine(WaitForEventManager());
        }

        private IEnumerator WaitForEventManager()
        {
            while (EventManager.Instance == null)
                yield return null;

            if (!isSubscribed)
            {
                EventManager.Instance.OnInteract += TryOpenDoor;
                isSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (isSubscribed && EventManager.Instance != null)
            {
                EventManager.Instance.OnInteract -= TryOpenDoor;
                isSubscribed = false;
            }
        }

        private void TryOpenDoor()
        {
            if (playerCanInteract)
            {
                GameManager.Instance.LoadNextScene(savedPosition, savedOrientation, sceneNumber);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!isDoor)
                {
                    GameManager.Instance.LoadNextScene(savedPosition, savedOrientation, sceneNumber);
                }
                else
                {
                    playerCanInteract = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && isDoor)
            {
                playerCanInteract = false;
            }
        }
    }
}