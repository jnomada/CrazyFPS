using UnityEngine;
using UnityEngine.AI;

// Clase MoveToPosition
// Controla el movimiento de un enemigo hacia el jugador usando el sistema NavMesh.
// El agente sigue al jugador automáticamente mientras no esté detenido.
// Requiere un NavMeshAgent y se adjunta a GameObjects enemigos.
//
// Campos:
// - target: Transform del jugador que será perseguido
// - agent: componente NavMeshAgent para navegación por el NavMesh
public class MoveToPosition : MonoBehaviour
{
    Transform target;        // Transform del objetivo a seguir (jugador)
    NavMeshAgent agent;      // Agente de navegación para movimiento por NavMesh
    
    // Método Start
    // Se ejecuta al iniciar. Encuentra al jugador y obtiene el componente NavMeshAgent.
    void Start()
    {
        // Buscar el GameObject con tag "Player" y obtener su Transform
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Obtener el componente NavMeshAgent del enemigo
        agent = GetComponent<NavMeshAgent>();
    }
    
    // Método Update
    // Se ejecuta cada frame. Actualiza el destino del agente hacia la posición del jugador
    // si el agente no está detenido.
    void Update()
    {
        // Verificar que el agente no esté detenido antes de actualizar el destino
        if (!agent.isStopped)
        {
            // Establecer la posición del jugador como destino del agente
            agent.SetDestination(target.position);
        }
    }
}
