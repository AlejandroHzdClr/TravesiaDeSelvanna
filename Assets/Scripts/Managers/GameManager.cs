using System;
using System.Numerics;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField]public Vector3 SavedPosition { get; private set; }
        public Vector3 SavedOrientation { get; private set; }
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(this);
            }
        }

        public void LoadNextScene(Vector3 targetPosition, Vector3 targetOrientation, int sceneNumber)
        {
            SavedPosition = targetPosition;
            SavedOrientation = targetOrientation;
            SceneManager.LoadScene(sceneNumber);
        }
    }
}