using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public Vector3 SavedPosition { get; private set; }
        public Vector3 SavedOrientation { get; private set; }
        public static GameManager Instance { get; private set; }

        public List<int> BooksList { get; private set; }
        public List<int> DialogueID { get; private set; }
        public float playerHealth = 100f;

        public Random rng;

        private bool isSubscribed = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                BooksList = new List<int>();
                DialogueID = new List<int>();
                rng = new Random();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

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
                EventManager.Instance.PlayerHealthChanged += UpdateHealth;
                isSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (isSubscribed && EventManager.Instance != null)
            {
                EventManager.Instance.PlayerHealthChanged -= UpdateHealth;
                isSubscribed = false;
            }
        }

        public void LoadNextScene(Vector3 targetPosition, Vector3 targetOrientation, int sceneNumber)
        {
            SavedPosition = targetPosition;
            SavedOrientation = targetOrientation;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneNumber);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                player.transform.position = SavedPosition;
                player.transform.eulerAngles = SavedOrientation;
            }
        }

        
        public void AddThisBook(int id)
        {
            BooksList.Add(id);
        }

        private void UpdateHealth(float newHealth)
        {
            playerHealth = newHealth;
        }
    }
}
