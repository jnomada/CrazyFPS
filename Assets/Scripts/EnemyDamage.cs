using UnityEngine;

// Clase EnemyDamage
// Gestiona el sistema de salud del enemigo basado en impactos de balas.
// El enemigo muere después de recibir un número determinado de impactos.
// Se adjunta a los GameObjects de enemigos.
//
// Campos:
// - HITS_TO_DIE: número de impactos necesarios para destruir al enemigo (constante de 3)
// - hitCount: contador de impactos recibidos hasta el momento
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int HITS_TO_DIE = 3;    // Número de impactos necesarios para morir
    int hitCount;                  // Contador de impactos recibidos

    // Método OnCollisionEnter
    // Se ejecuta cuando el enemigo colisiona con otro objeto.
    // Cuenta los impactos de balas y destruye al enemigo cuando alcanza el límite.
    // Parámetros:
    // - collision: información sobre la colisión que acaba de ocurrir


    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto que colisionó es una bala
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Incrementar el contador de impactos
            hitCount++;

            // Verificar si se alcanzó el número de impactos necesarios para morir
            if (hitCount == HITS_TO_DIE)
            {
                // Destruir el GameObject del enemigo
                Destroy(gameObject);
            }
        }
    }
}
