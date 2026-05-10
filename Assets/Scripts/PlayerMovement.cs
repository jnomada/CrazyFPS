using UnityEngine;

// Clase PlayerMovement
// Controla el movimiento del jugador incluyendo desplazamiento horizontal, salto y detección de suelo.
// Se adjunta al GameObject del jugador y requiere componentes Rigidbody y CapsuleCollider.
//
// Campos:
// - speed: velocidad de desplazamiento del jugador
// - jumpForce: fuerza aplicada al saltar
// - rb: referencia al componente Rigidbody para aplicar físicas
// - col: referencia al CapsuleCollider para detectar colisiones con el suelo
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]

    [Header("Settings")]
    [SerializeField] float speed;         // Velocidad de movimiento del jugador
    [SerializeField] float jumpForce;     // Fuerza aplicada al saltar

    Rigidbody rb;           // Componente Rigidbody para aplicar fuerzas físicas
    CapsuleCollider col;    // Componente CapsuleCollider para detectar colisiones

    // Método Start
    // Se ejecuta al iniciar el juego. Configura el cursor bloqueado y obtiene referencias
    // a los componentes necesarios (Rigidbody y CapsuleCollider).
    void Start()
    {
        // Bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;

        // Obtener referencia al componente Rigidbody
        rb = GetComponent<Rigidbody>();
        
        // Obtener referencia al componente CapsuleCollider
        col = GetComponent<CapsuleCollider>();
    }

    // Método Update
    // Se ejecuta cada frame. Maneja el movimiento horizontal del jugador, el salto
    // y la liberación del cursor.
    void Update()
    {
        // Desplazamiento del jugador
        // Crear un vector para almacenar la entrada de movimiento
        Vector2 moveInput = Vector2.zero;
        
        // Capturar entrada horizontal (A/D o flechas izq/der) y aplicar velocidad
        moveInput.x = Input.GetAxis("Horizontal") * speed;
        
        // Capturar entrada vertical (W/S o flechas arr/abajo) y aplicar velocidad
        moveInput.y = Input.GetAxis("Vertical") * speed;
        
        // Ajustar la velocidad al tiempo transcurrido entre frames para movimiento consistente
        moveInput *= Time.deltaTime;
        
        // Aplicar el desplazamiento al transform del jugador
        transform.Translate(moveInput.x, 0, moveInput.y);

        // Salto del jugador
        // Verificar si el jugador está en el suelo y presiona el botón de salto
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            // Aplicar una fuerza hacia arriba de forma instantánea
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }     

        // Liberar el cursor al presionar la tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Desbloquear el cursor para que sea visible y móvil
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Método IsGrounded
    // Detecta si el jugador está tocando el suelo mediante un raycast hacia abajo.
    // Retorna: true si el jugador está en el suelo, false en caso contrario
    bool IsGrounded()
    {
        // Lanzar un rayo desde la posición del jugador hacia abajo
        // La distancia es la mitad de la altura del collider más un pequeño margen (0.1f)
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }
}
