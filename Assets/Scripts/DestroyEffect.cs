using UnityEngine;

// Clase DestroyEffect
// Destruye automáticamente el GameObject después de un tiempo determinado.
// Útil para efectos visuales temporales como partículas, explosiones o impactos.
// Se adjunta al GameObject que debe autodestruirse.
//
// Campos:
// - TIME: tiempo en segundos antes de destruir el objeto (constante de 1 segundo)
// - timer: temporizador que acumula el tiempo transcurrido
public class DestroyEffect : MonoBehaviour
{
    const float TIME = 1;    // Tiempo de vida del efecto en segundos
    float timer;             // Temporizador acumulativo
    
    // Método Update
    // Se ejecuta cada frame. Incrementa el temporizador y destruye el objeto
    // cuando se alcanza el tiempo límite.
    void Update()
    {
        // Incrementar el temporizador con el tiempo transcurrido desde el último frame
        timer += Time.deltaTime;
        
        // Verificar si se alcanzó el tiempo límite
        if (timer >= TIME)
        {
            // Destruir el GameObject
            Destroy(gameObject);
        }
    }
}
