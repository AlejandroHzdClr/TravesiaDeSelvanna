using Managers;
using UnityEngine;

namespace Door
{
    public class DoorLevel : MonoBehaviour
    {
        [SerializeField] private Vector3 savedPosition;
        [SerializeField] private Vector3 savedOrientation;
        [SerializeField] private int sceneNumber;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.LoadNextScene(savedPosition,savedOrientation,sceneNumber);
            }
        }
    }
}
