using System;
using System.Collections.Generic;
using System.Numerics;
using Collectables;
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
        
        public List<int> BooksList { get; private set; }

        private protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                BooksList = new List<int>();
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

        public void AddThisBook(int id)
        {
            BooksList.Add(id);
        }


    }
}