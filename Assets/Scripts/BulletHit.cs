using UnityEngine;

// Clase BulletHit
// Maneja el efecto visual y destrucción de la bala cuando impacta con un objeto.
// Instancia un efecto de partículas en el punto de impacto y desactiva la bala.
// Se adjunta a los GameObjects de proyectiles/balas.
//
// Campos:
// - particle: prefab del efecto de partículas a instanciar en el punto de impacto
public class BulletHit : MonoBehaviour
{
    [SerializeField] GameObject enemyHitParticle;    // Efecto de partículas de impacto
    [SerializeField] GameObject hitParticle;
    GameObject particle;

    // Método OnCollisionEnter
    // Se ejecuta cuando la bala colisiona con cualquier objeto.
    // Genera un efecto visual de impacto y desactiva la bala.
    // Parámetros:
    // - collision: información sobre la colisión que acaba de ocurrir
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            particle = enemyHitParticle;
        }
        else
        {
            particle = hitParticle;
        }

        // Instanciar el efecto de partículas en la posición del impacto
        // Se utiliza Quaternion.identity para no aplicar rotación
        Instantiate(particle, transform.position, Quaternion.identity);
        
        // Desactivar el GameObject de la bala
        // Se usa SetActive en lugar de Destroy para posible reutilización
        gameObject.SetActive(false);
    }
}
