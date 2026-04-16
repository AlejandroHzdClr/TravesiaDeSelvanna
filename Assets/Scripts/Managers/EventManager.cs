using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        public event Action OnInteract;
        public event Action<float> PlayerHealthChanged;
        public event Action<int> BookHasBeenRecolected;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnRelease()
        {
            OnInteract?.Invoke();
        }

        public void HealthChanged(float damage)
        {
            PlayerHealthChanged?.Invoke(damage);
        }

        public void PickBook(int id)
        {
            BookHasBeenRecolected?.Invoke(id);
        }
    }
}