using System.Collections;
using UnityEngine;

// Clase PowerUpSpawner
// Genera power-ups de forma periódica en posiciones aleatorias del mapa.
// Solo mantiene un power-up activo a la vez. Cuando se recoge, genera uno nuevo después del delay.
// Se coloca en una posición central del nivel.
//
// Campos:
// - prefab: prefab del power-up a instanciar
// - spawnPoints: array de transforms que marcan las posiciones posibles de spawn
// - delay: tiempo en segundos entre spawns
// - powerUp: referencia al power-up actualmente activo en el nivel
public class PowerUpSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject prefab;           // Prefab del power-up a generar
    [SerializeField] Transform[] spawnPoints;     // Puntos posibles donde generar power-ups

    [Header("Settings")]
    [SerializeField] float delay = 5f;                 // Tiempo entre spawns en segundos

    GameObject powerUp;                           // Referencia al power-up actual

    // Método Start
    // Se ejecuta al iniciar. Inicia la corrutina de generación de power-ups.
    void Start()
    {
        // Iniciar la corrutina que genera power-ups
        StartCoroutine(Spawn());
    }

    // Método Spawn (Corrutina)
    // Genera power-ups de forma continua en posiciones aleatorias.
    // Solo genera un nuevo power-up cuando el anterior ha sido recogido o destruido.
    // Retorna: IEnumerator para ejecutarse como corrutina
    IEnumerator Spawn()
    {
        // Bucle infinito de generación
        while (true)
        {
            // Verificar si no hay un power-up activo en el nivel
            if (powerUp == null)
            {
                // Esperar el tiempo de delay antes de generar
                yield return new WaitForSeconds(delay);
                
                // Seleccionar una posición aleatoria del array de spawn points
                Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                
                // Instanciar el power-up en la posición seleccionada sin rotación
                powerUp = Instantiate(prefab, position, Quaternion.identity);
            }

            // Esperar medio segundo antes de la siguiente verificación
            yield return new WaitForSeconds(0.5f);
        }
    }
}
