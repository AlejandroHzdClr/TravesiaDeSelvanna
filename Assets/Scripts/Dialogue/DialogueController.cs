using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private AudioSource audioSound;

        private void Awake()
        {
            audioSound = GetComponent<AudioSource>();
            panelDialogo.SetActive(false);
        }

        void Update()
        {
            if (isInRange && Input.GetKeyDown(KeyCode.Q))
            {
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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                isInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                isInRange = false;
        }

        private void EmpezarDialogo()
        {
            isTalking = true;
            panelDialogo.SetActive(true);
            lineasIndex = 0;
            Time.timeScale = 0;
            StartCoroutine(MostrarLineas());
        }

        private void SiguienteLinea()
        {
            lineasIndex++;

            if (lineasIndex < lineas.dialogo.Count)
            {
                StartCoroutine(MostrarLineas());
            }
            else
            {
                isTalking = false;
                panelDialogo.SetActive(false);
                Time.timeScale = 1;
            }
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
