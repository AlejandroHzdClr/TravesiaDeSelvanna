using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Cargar la escena 0
    public void LoadScene0()
    {
        SceneManager.LoadScene(0);
    }

    // Cerrar el juego
    public void QuitGame()
    {
        // Funciona en build
        Application.Quit();
    }
}