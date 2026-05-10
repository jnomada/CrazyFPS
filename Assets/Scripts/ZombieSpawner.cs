using System.Collections;
using UnityEngine;

// Clase ZombieSpawner
// Genera zombies de forma periódica en la posición del spawner.
// Controla la cantidad máxima de zombies y el intervalo entre spawns.
// Se coloca en posiciones estratégicas del nivel donde deben aparecer zombies.
//
// Campos:
// - zombie: prefab del zombie a instanciar
// - spawnDelay: tiempo en segundos entre cada spawn de zombie
// - zombieMax: número máximo de zombies que puede generar este spawner
// - numZombies: contador de zombies generados hasta el momento
public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject[] zombie;       // Prefab del zombie a generar

    [Header("Settings")]
    [SerializeField] float spawnDelay;        // Tiempo entre spawns en segundos
    [SerializeField] int zombieMax;           // Máximo de zombies a generar

    int numZombies = 0;                       // Contador de zombies generados

    // Método Start
    // Se ejecuta al iniciar. Inicia la corrutina que genera zombies periódicamente.
    void Start()
    {
        // Iniciar la corrutina de generación de zombies
        StartCoroutine(SpawnZombie());
    }
    
    // Método SpawnZombie (Corrutina)
    // Genera zombies de forma periódica hasta alcanzar el máximo permitido.
    // Retorna: IEnumerator para ejecutarse como corrutina
    IEnumerator SpawnZombie()
    {
        // Esperar el doble del delay inicial antes de comenzar a generar
        yield return new WaitForSeconds(spawnDelay * 2);
        
        // Bucle que genera zombies hasta alcanzar el máximo
        while (numZombies < zombieMax)
        {
            // Escoger de forma aleatoria
            
            // Instanciar un zombie en la posición del spawner sin rotación
            Instantiate(zombie[Random.Range(0, zombie.Length)], transform.position, Quaternion.identity);

            // Incrementar el contador de zombies generados
            numZombies++;
            
            // Esperar el tiempo de delay antes de generar el siguiente zombie
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
