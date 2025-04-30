using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Para manejar escenas

public class PauseMenu : MonoBehaviour
{
    public GameObject menuUI;
    public Button continueButton;
    public Button exitButton;
    private bool isPaused = false;

    void Update()
    {
        // Cambiar a usar el nuevo sistema de entrada
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        menuUI.SetActive(true);  // Muestra el menú
        Time.timeScale = 0f;     // Pausa el juego
        isPaused = true;
    }

    void ResumeGame()
    {
        menuUI.SetActive(false); // Oculta el menú
        Time.timeScale = 1f;     // Reanuda el juego
        isPaused = false;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MenuPrincipal");  // Cargar la escena principal, reemplaza con el nombre de tu escena
    }
}
