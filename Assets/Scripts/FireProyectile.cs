using UnityEngine;

// Clase Fireproyectile
// Maneja el disparo de proyectiles por parte del jugador.
// Instancia un proyectil en la posición y rotación del objeto cuando se presiona el botón de disparo.
// Se adjunta al objeto que dispara (típicamente la cámara o el arma del jugador).
//
// Campos:
// - proyectile: prefab del proyectil a instanciar
// - delay: tiempo en segundos antes de destruir el proyectil automáticamente
public class Fireproyectile : MonoBehaviour
{
    [SerializeField] GameObject proyectile;   // Prefab del proyectil a disparar
    [SerializeField] float delay;             // Tiempo antes de destruir el proyectil
    [SerializeField] GameObject fireEffect;
    
    // Método Update
    // Se ejecuta cada frame. Detecta cuando se presiona el botón de disparo y crea
    // un nuevo proyectil.
    void Update()
    {
        // Detectar si se presiona el botón de disparo (click izquierdo del mouse)
        if (Input.GetButtonDown("Fire1"))
        {
            // Instanciar el proyectil en la posición y rotación actual del objeto
            GameObject clone = Instantiate(proyectile, transform.position, transform.rotation);
            Instantiate(fireEffect, transform.position, transform.rotation);
            // Destruir el proyectil después del tiempo especificado
            Destroy(clone, delay);
        }
    }
}
