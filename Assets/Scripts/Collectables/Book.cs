using System;
using Interfaces;
using Managers;
using UnityEngine;

namespace Collectables
{
    public class Book : MonoBehaviour, ICollectable<int>
    {

        [SerializeField] private int bookID;
        
        private void Awake()
        {
            if (GameManager.Instance != null &&
                GameManager.Instance.BooksList.Contains(bookID))
            {
                gameObject.SetActive(false);
            }

        }

        public void BeingCollected(int id)
        {
            GameManager.Instance.AddThisBook(id);
            EventManager.Instance.PickBook(id);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Trigger con: " + other.name);

            if (other.CompareTag("Player"))
            {
                BeingCollected(bookID);
                this.gameObject.SetActive(false);
            }
        }
        
        
    }
}