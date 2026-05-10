using UnityEngine;

// Clase ProyectileMovement
// Controla el movimiento del proyectil en línea recta hacia adelante.
// El proyectil se mueve continuamente en la dirección forward de su transform.
// Se adjunta a los GameObjects de proyectiles.
//
// Campos:
// - SPEED: velocidad de movimiento del proyectil (constante de 10 unidades por segundo)
public class ProyectileMovement : MonoBehaviour
{
    [SerializeField] float SPEED = 10.0f;    // Velocidad de movimiento del proyectil
    
    // Método Update
    // Se ejecuta cada frame. Mueve el proyectil hacia adelante a velocidad constante.
    void Update()
    {
        // Mover el proyectil hacia adelante (eje Z local)
        // Se multiplica por Time.deltaTime para independencia del framerate
        transform.Translate(Vector3.forward * SPEED * Time.deltaTime);        
    }
}
