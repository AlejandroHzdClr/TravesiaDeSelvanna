using System.Collections;
using Managers;
using UnityEngine;

namespace Door
{
    public class DestroyDoor : MonoBehaviour
    {
        [SerializeField] private int numberOfBooks;

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
                EventManager.Instance.BookHasBeenRecolected += BookHasBeenRecolected;
                isSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (isSubscribed && EventManager.Instance != null)
            {
                EventManager.Instance.BookHasBeenRecolected -= BookHasBeenRecolected;
                isSubscribed = false;
            }
        }

        private void BookHasBeenRecolected(int _)
        {
            if (GameManager.Instance.BooksList.Count >= numberOfBooks)
            {
                Destroy(gameObject);
            }
        }
    }
}