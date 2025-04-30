using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia "GameScene" por el nombre de tu escena del juego
    }

    public void QuitGame()
    {
        Application.Quit(); // Funciona cuando el juego está compilado
        Debug.Log("Saliendo del juego...");
    }

    public void OpenSettings()
    {
        Debug.Log("Ajustes aún no implementados.");
    }
}
