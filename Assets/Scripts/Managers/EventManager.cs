using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }
        public static event Action OnInteract;
        public static event Action<float> PlayerHealthChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        public static void OnRelease()
        {
            OnInteract?.Invoke();
        }

        public static void HealthChanged(float damage)
        {
            PlayerHealthChanged?.Invoke(damage);
        }
    }
}