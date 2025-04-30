using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referencia al personaje
    public float smoothing = 5f; // Suavizado del movimiento de la cámara

    private Vector3 offset; // Desplazamiento de la cámara respecto al personaje

    void Start()
    {
        // Inicializa el desplazamiento según la posición inicial de la cámara
        offset = transform.position - player.position;
    }

    void FixedUpdate()
    {
        // Calcula la nueva posición de la cámara
        Vector3 targetCamPos = player.position + offset;
        
        // Mueve la cámara suavemente hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
