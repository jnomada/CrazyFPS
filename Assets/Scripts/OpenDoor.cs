using UnityEngine;

// Clase OpenDoor
// Controla la apertura automática de una puerta cuando el jugador se acerca.
// Utiliza un trigger collider para detectar la proximidad del jugador.
// Requiere un Animator con un parámetro bool "Open".
//
// Campos:
// - anim: componente Animator que controla la animación de apertura de la puerta
public class OpenDoor : MonoBehaviour
{
    [SerializeField] Animator anim;    // Animator de la puerta

    // Método OnTriggerEnter
    // Se ejecuta cuando otro collider entra en el trigger de este objeto.
    // Detecta al jugador y activa la animación de apertura de la puerta.
    // Parámetros:
    // - other: el collider que entró en el trigger
    void OnTriggerEnter(Collider other)
    {
        // Registrar en consola que se activó el trigger (para debug)
        Debug.Log("Triggered");
        
        // Verificar si el objeto que entró es el jugador
        if (other.gameObject.tag == "Player")
        {
            // Activar el parámetro "Open" del animator para abrir la puerta
            anim.SetBool("Open", true);
        }
    }
}
