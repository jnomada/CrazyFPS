using UnityEngine;

// Clase EnemyBite
// Aplica daño continuo al jugador mientras el enemigo está en contacto con él.
// El daño se aplica en cada frame mientras la colisión persiste.
// Se adjunta a los GameObjects de enemigos.
//
// Campos:
// Ninguno - esta clase no tiene campos configurables
public class EnemyBite : MonoBehaviour
{
    // Método OnCollisionStay
    // Se ejecuta cada frame mientras el enemigo está colisionando con otro objeto.
    // Aplica daño continuo al jugador mientras mantiene el contacto.
    // Parámetros:
    // - other: información de la colisión que está ocurriendo
    void OnCollisionStay(Collision other)
    {
        // Verificar si el objeto en colisión es el jugador
        if (other.gameObject.CompareTag("Player"))
        {
            // Aplicar 1 punto de daño al jugador
            other.gameObject.SendMessage("ApplyDamage", 1);
        }
    }
    
}
