using System;
using UnityEngine;

// Clase MouseLook
// Controla la rotación de la cámara del jugador basada en el movimiento del mouse.
// Permite rotación horizontal del personaje y rotación vertical de la cámara con límites.
// Se adjunta al GameObject de la cámara que es hijo del jugador.
//
// Campos:
// - CLAMP_MIN: ángulo mínimo de rotación vertical (-45 grados hacia arriba)
// - CLAMP_MAX: ángulo máximo de rotación vertical (45 grados hacia abajo)
// - lookSensitivity: sensibilidad del mouse para la rotación
// - player: referencia al GameObject del jugador (padre de la cámara)
// - rotation: almacena la rotación objetivo basada en la entrada del mouse
// - smoothRot: rotación suavizada para transiciones fluidas
// - velRot: velocidad de rotación utilizada por SmoothDamp
public class MouseLook : MonoBehaviour
{
    const float CLAMP_MIN = -45.0f;    // Límite superior de rotación vertical
    const float CLAMP_MAX = 45.0f;     // Límite inferior de rotación vertical

    [SerializeField] float lookSensitivity;   // Sensibilidad de la rotación con el mouse
    GameObject player;                        // Referencia al GameObject del jugador
    Vector2 rotation = Vector2.zero;          // Rotación objetivo
    Vector2 smoothRot = Vector2.zero;         // Rotación suavizada actual
    Vector2 velRot = Vector2.zero;            // Velocidad de rotación para suavizado

    // Método Start
    // Se ejecuta al iniciar. Obtiene la referencia al GameObject padre (el jugador).
    void Start()
    {
        // Obtener referencia al objeto padre (jugador)
        player = transform.parent.gameObject;    
    }

    // Método Update
    // Se ejecuta cada frame. Maneja la rotación horizontal del jugador y vertical de la cámara
    // basándose en el movimiento del mouse.
    void Update()
    {
        // Rotación horizontal del jugador (alrededor del eje Y)
        // Rotar el jugador alrededor de la posición de la cámara usando el eje vertical (Y)
        // Se multiplica por la sensibilidad para ajustar la velocidad de rotación
        player.transform.RotateAround(transform.position, Vector3.up, 
            Input.GetAxis("Mouse X") * lookSensitivity);

        // Rotación vertical de la cámara (mirar arriba/abajo)
        // Acumular el movimiento vertical del mouse
        rotation.y += Input.GetAxis("Mouse Y");
        
        // Limitar la rotación vertical entre los valores mínimo y máximo
        rotation.y = Mathf.Clamp(rotation.y, CLAMP_MIN, CLAMP_MAX);
        
        // Suavizar la rotación para evitar movimientos bruscos
        smoothRot.y = Mathf.SmoothDamp(smoothRot.y, rotation.y, ref velRot.y, 0.1f);
        
        // Aplicar la rotación suavizada al eje X local (negativo para invertir el movimiento)
        transform.localEulerAngles = new Vector3(-smoothRot.y, 0, 0);
    }
}
