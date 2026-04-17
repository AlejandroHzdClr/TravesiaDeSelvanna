using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        private bool isInRange = false;
        private bool isTalking = false;
        private int lineasIndex;

        [SerializeField] private float tiempoTexto = 0.05f;
        [SerializeField] private GameObject panelDialogo;
        [SerializeField] public DialogueSO lineas;
        [SerializeField] private TMP_Text textoNombre;
        [SerializeField] private TMP_Text dialogoText;
        [SerializeField] private AudioClip sonidoLetras;
        [SerializeField] private Image imagen;

        [Header("Opciones")]
        [SerializeField] private bool isAutomatic;
        [SerializeField] private bool willBeDestroy;
        [SerializeField] private bool canBeReproductedOne;

        private AudioSource audioSound;

        // Para evitar suscripciones duplicadas
        private bool isSubscribed = false;

        private void Awake()
        {
            audioSound = GetComponent<AudioSource>();
            panelDialogo.SetActive(false);
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
                EventManager.Instance.OnInteract += OnInteractPressed;
                isSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (isSubscribed && EventManager.Instance != null)
            {
                EventManager.Instance.OnInteract -= OnInteractPressed;
                isSubscribed = false;
            }
        }

        private void OnInteractPressed()
        {
            if (!isInRange) return;

            if (!isTalking)
            {
                EmpezarDialogo();
            }
            else if (dialogoText.text == lineas.dialogo[lineasIndex].texto)
            {
                SiguienteLinea();
            }
            else
            {
                StopAllCoroutines();
                dialogoText.text = lineas.dialogo[lineasIndex].texto;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = true;

                if (isAutomatic && !isTalking )
                    EmpezarDialogo();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                isInRange = false;
        }

        private void EmpezarDialogo()
        {
            // Bloqueo si ya se reprodujo
            if (canBeReproductedOne && GameManager.Instance.DialogueID.Contains(lineas.id))
            {
                Debug.Log("Este diálogo ya fue ejecutado");
                return;
            }

            isTalking = true;
            panelDialogo.SetActive(true);
            lineasIndex = 0;
            Time.timeScale = 0;

            StopAllCoroutines();
            StartCoroutine(MostrarLineas());
        }

        private void SiguienteLinea()
        {
            lineasIndex++;

            if (lineasIndex < lineas.dialogo.Count)
            {
                StopAllCoroutines();
                StartCoroutine(MostrarLineas());
            }
            else
            {
                TerminarDialogo();
            }
        }

        private void TerminarDialogo()
        {
            isTalking = false;
            panelDialogo.SetActive(false);
            Time.timeScale = 1;

            // Guardar estado en el ScriptableObject
            if (canBeReproductedOne)
                GameManager.Instance.DialogueID.Add(lineas.id);

            if (willBeDestroy)
                Destroy(gameObject);
        }

        private IEnumerator MostrarLineas()
        {
            dialogoText.text = string.Empty;
            textoNombre.text = lineas.dialogo[lineasIndex].nombre;

            string textoActual = lineas.dialogo[lineasIndex].texto;

            foreach (char ch in textoActual)
            {
                dialogoText.text += ch;

                if (sonidoLetras != null)
                    audioSound.PlayOneShot(sonidoLetras, 1f);

                yield return new WaitForSecondsRealtime(tiempoTexto);
            }
        }
    }
}
