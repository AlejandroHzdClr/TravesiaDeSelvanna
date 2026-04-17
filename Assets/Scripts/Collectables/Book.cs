using System;
using Interfaces;
using Managers;
using UnityEngine;

namespace Collectables
{
    public class Book : MonoBehaviour, ICollectable<int>
    {

        [SerializeField] private int bookID;

        private AudioSource audioSource;
        private void Awake()
        {
            if (GameManager.Instance != null &&
                GameManager.Instance.BooksList.Contains(bookID))
            {
                gameObject.SetActive(false);
            }

            audioSource = GetComponent<AudioSource>();

        }

        public void BeingCollected(int id)
        {
            GameManager.Instance.AddThisBook(id);
            EventManager.Instance.PickBook(id);
            if (audioSource.clip != null)
            {
                AudioSource.PlayClipAtPoint(audioSource.clip,transform.position);
            }
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