using UnityEngine;
using UnityEngine.AI;

// Clase ZombieAnim
// Controla las animaciones del zombie basadas en su estado (corriendo o atacando).
// Maneja la transición entre animaciones cuando el zombie entra en contacto con el jugador.
// Requiere un Animator y un NavMeshAgent. Se adjunta a GameObjects de zombies.
//
// Campos:
// - anim: componente Animator para controlar las animaciones del zombie
// - agent: componente NavMeshAgent para controlar el movimiento y detención del zombie
public class ZombieAnim : MonoBehaviour
{
    [SerializeField] Animator anim;    // Controlador de animaciones del zombie
    NavMeshAgent agent;                // Agente de navegación del zombie

    // Método Start
    // Se ejecuta al iniciar. Configura la animación inicial de correr y obtiene el NavMeshAgent.
    void Start()
    {
        // Activar la animación de correr al inicio
        anim.SetBool("IsRunning", true);
        
        // Obtener el componente NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
    }

    // Método OnCollisionEnter
    // Se ejecuta cuando el zombie colisiona con otro objeto.
    // Si colisiona con el jugador, detiene el movimiento y activa la animación de ataque.
    // Parámetros:
    // - collision: información sobre la colisión que acaba de ocurrir
    void OnCollisionEnter(Collision collision)
    {
        // Verificar si colisionó con el jugador
        if (collision.gameObject.tag == "Player")
        {
            // Detener el agente de navegación si no estaba ya detenido
            if (!agent.isStopped)
            {
                // Establecer la posición actual como destino (detener movimiento)
                agent.SetDestination(transform.position);
                
                // Marcar el agente como detenido
                agent.isStopped = true;
            }

            // Activar la animación de ataque
            anim.SetBool("IsAttacking", true);
        }
    }

    // Método OnCollisionExit
    // Se ejecuta cuando el zombie deja de colisionar con otro objeto.
    // Si el jugador se aleja, desactiva la animación de ataque y reanuda el movimiento.
    // Parámetros:
    // - collision: información sobre la colisión que terminó
    void OnCollisionExit(Collision collision)
    {
        // Verificar si dejó de colisionar con el jugador
        if (collision.gameObject.tag == "Player")
        {
            // Desactivar la animación de ataque
            anim.SetBool("IsAttacking", false);

            // Programar la reanudación del movimiento después de 3 segundos
            Invoke("ResumeAgent", 3f);
        }
    }

    // Método ResumeAgent
    // Reactiva el movimiento del agente de navegación.
    // Es llamado por Invoke después de un retraso.
    void ResumeAgent()
    {
        // Reactivar el agente para que continúe persiguiendo al jugador
        agent.isStopped = false;
    }

}
