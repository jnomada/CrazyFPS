using Unity.VisualScripting;
using UnityEngine;

// Clase PowerUpApply
// Gestiona el efecto de un power-up que restaura la salud del jugador.
// Cuando el jugador entra en contacto con el power-up, se cura y el objeto desaparece.
// Requiere un collider con "Is Trigger" activado.
//
// Campos:
// - POWER: cantidad de salud restaurada (constante de 50 puntos, se aplica como daño negativo)
// - clip: sonido que se reproduce al recoger el power-up
public class PowerUpApply : MonoBehaviour
{
    const int POWER = 50;                   // Cantidad de salud que restaura el power-up

    [SerializeField] AudioClip clip;        // Sonido al recoger el power-up

    // Método OnTriggerEnter
    // Se ejecuta cuando otro collider entra en el trigger de este objeto.
    // Detecta al jugador y le aplica curación.
    // Parámetros:
    // - other: el collider que entró en el trigger
    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entró es el jugador
        if (other.CompareTag("Player"))
        {
            // Aplicar curación al jugador (daño negativo restaura salud)
            other.gameObject.SendMessage("ApplyDamage", -POWER);

            // Reproducir el sonido del power-up en la posición actual
            AudioSource.PlayClipAtPoint(clip, transform.position);
            
            // Destruir el GameObject del power-up
            Destroy(gameObject);
        }
    }
}
