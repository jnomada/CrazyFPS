using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Clase PlayerDamage
// Gestiona la salud del jugador y muestra la información en la interfaz.
// Recibe daño a través del método ApplyDamage que puede ser llamado por otros scripts.
// Se adjunta al GameObject del jugador.
//
// Campos:
// - MAX_LIFE: vida máxima del jugador (constante de 100)
// - txtHealth: referencia al elemento de texto UI que muestra la salud actual
// - health: salud actual del jugador
public class PlayerDamage : MonoBehaviour
{
    const int MAX_LIFE = 100;               // Vida máxima del jugador
    [SerializeField] TMP_Text txtHealth;        // Texto UI para mostrar la salud
    
    int health = MAX_LIFE;                  // Salud actual del jugador
    
    // Método Start
    // Se ejecuta al iniciar. Inicializa la visualización de la salud aplicando 0 de daño.
    void Start()
    {
        // Aplicar 0 de daño para actualizar el texto de salud inicial
        ApplyDamage(0);
    }

    // Método ApplyDamage
    // Aplica daño al jugador y actualiza el texto de salud en la UI.
    // Parámetros:
    // - damage: cantidad de daño a aplicar (puede ser negativo para curar)
    void ApplyDamage(int damage)
    {
        // Solo aplicar daño si el jugador está vivo
        if (health > 0)
        {
            // Reducir la salud por la cantidad de daño
            health -= damage;
            
            // Actualizar el texto de la UI con la salud actual
            txtHealth.text = health.ToString();
        }        
    }
}
