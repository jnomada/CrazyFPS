---
title: CrazyZombie - Project [Code Rev]
weight: 7
author: Fran Montoiro
draft: false
---

#### 💻 Fragmentos de código relevantes

Este documento revisa con más detalle fragmentos de código clave del proyecto CrazyZombie.

---

##### 1. Sistema de Detección de Suelo con Raycast ⭐⭐⭐

**Ubicación:** `PlayerMovement.cs` - método `IsGrounded()`

**Descripción:**
Sistema preciso de detección de suelo que utiliza raycasting desde el centro del jugador hacia abajo, calculando dinámicamente la distancia basándose en las dimensiones del CapsuleCollider.

**Código:**

```csharp
bool IsGrounded()
{
    // Lanzar un rayo desde la posición del jugador hacia abajo
    // La distancia es la mitad de la altura del collider más un pequeño margen (0.1f)
    return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
}

// En Update(), validación antes de permitir salto
if (IsGrounded() && Input.GetButtonDown("Jump"))
{
    // Aplicar una fuerza hacia arriba de forma instantánea
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
}
```

**Lo interesante:**

- **Cálculo dinámico de distancia:** Usa `col.bounds.extents.y` (mitad de la altura del collider) en lugar de valores fijos, adaptándose automáticamente a cambios en el tamaño del personaje
- **Margen de tolerancia:** El `+ 0.1f` adicional previene problemas de floating-point y asegura detección confiable incluso con pequeñas irregularidades del terreno
- **Prevención de saltos múltiples:** La validación `IsGrounded()` antes de `AddForce` garantiza que el jugador solo puede saltar cuando está tocando el suelo
- **Uso de ForceMode.Impulse:** Aplica una fuerza instantánea que simula correctamente la física de un salto sin afectar la masa del objeto

**Impacto en gameplay:** Proporciona controles de salto responsivos y predecibles, fundamentales para un shooter en primera persona donde el movimiento vertical es crítico para evadir enemigos.

---

##### 2. Control de Cámara con Rotación Suavizada Dual ⭐⭐⭐

**Ubicación:** `MouseLook.cs` - método `Update()`

**Descripción:**
Sistema que combina rotación horizontal instantánea del jugador con rotación vertical suavizada de la cámara, implementando límites de visión y prevención de mareo.

**Código:**

```csharp
void Update()
{
    // Rotación horizontal del jugador (alrededor del eje Y)
    // Rotar el jugador alrededor de la posición de la cámara usando el eje vertical (Y)
    // Se multiplica por la sensibilidad para ajustar la velocidad de rotación
    player.transform.RotateAround(transform.position, Vector3.up, 
        Input.GetAxis("Mouse X") * lookSensitivity);

    // Rotación vertical de la cámara (mirar arriba/abajo)
    // Acumular el movimiento vertical del mouse
    rotation.y += Input.GetAxis("Mouse Y");
    
    // Limitar la rotación vertical entre los valores mínimo y máximo
    rotation.y = Mathf.Clamp(rotation.y, CLAMP_MIN, CLAMP_MAX);
    
    // Suavizar la rotación para evitar movimientos bruscos
    smoothRot.y = Mathf.SmoothDamp(smoothRot.y, rotation.y, ref velRot.y, 0.1f);
    
    // Aplicar la rotación suavizada al eje X local (negativo para invertir el movimiento)
    transform.localEulerAngles = new Vector3(-smoothRot.y, 0, 0);
}
```

**Lo interesante:**

- **Rotación dual asíncrona:** El eje horizontal rota instantáneamente (`RotateAround`) mientras el vertical usa `SmoothDamp`, creando sensación natural de inercia solo en el pitch
- **Clamp de ángulos fisiológicos:** Límites de -45° a +45° imitan las restricciones naturales del cuello humano, evitando desorientación
- **RotateAround como pivote:** Gira el jugador completo alrededor de la posición de la cámara, no del centro del cuerpo, manteniendo la vista estable
- **Triple variable para interpolación:** Usa `rotation` (objetivo), `smoothRot` (actual) y `velRot` (velocidad) para suavizado matemáticamente correcto con SmoothDamp

**Impacto en gameplay:** Crea controles de cámara fluidos que se sienten profesionales, eliminando el efecto "robótico" de rotaciones instantáneas mientras mantiene la precisión de apuntado necesaria para un FPS.

---

##### 3. Sistema de Spawning con Corrutinas y Control de Población ⭐⭐⭐

**Ubicación:** `ZombieSpawner.cs` - corrutina `SpawnZombie()`

**Descripción:**
Generador de enemigos que usa corrutinas para controlar el ritmo de aparición, implementando un retardo inicial estratégico y límites de población.

**Código:**

```csharp
void Start()
{
    // Iniciar la corrutina de generación de zombies
    StartCoroutine(SpawnZombie());
}

IEnumerator SpawnZombie()
{
    // Esperar el doble del delay inicial antes de comenzar a generar
    yield return new WaitForSeconds(spawnDelay * 2);
    
    // Bucle que genera zombies hasta alcanzar el máximo
    while (numZombies < zombieMax)
    {
        // Instanciar un zombie en la posición del spawner sin rotación
        Instantiate(zombie, transform.position, Quaternion.identity);

        // Incrementar el contador de zombies generados
        numZombies++;
        
        // Esperar el tiempo de delay antes de generar el siguiente zombie
        yield return new WaitForSeconds(spawnDelay);
    }
}
```

**Lo interesante:**

- **Retardo inicial estratégico:** Multiplica por 2 el primer delay (`spawnDelay * 2`) dando tiempo al jugador para explorar antes de enfrentar enemigos
- **Control de población determinista:** El contador `numZombies` con límite `zombieMax` previene spawn infinito y permite diseño de oleadas específicas
- **Corrutinas vs Update:** Evita polling cada frame, ejecutándose solo cuando es necesario, reduciendo overhead computacional
- **Quaternion.identity:** Spawn sin rotación, dejando que el NavMeshAgent del zombie determine su orientación basándose en el pathfinding

**Impacto en gameplay:** Crea presión gradual que escala de forma predecible, permitiendo al diseñador controlar exactamente la curva de dificultad sin sobrecargar la escena.

---

##### 4. Máquina de Estados Basada en Colisiones para Animaciones ⭐⭐⭐

**Ubicación:** `ZombieAnim.cs` - métodos `OnCollisionEnter()` y `OnCollisionExit()`

**Descripción:**
Sistema que coordina animaciones, navegación y combate del zombie usando eventos de colisión física como disparadores de transiciones de estado.

**Código:**

```csharp
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

void ResumeAgent()
{
    // Reactivar el agente para que continúe persiguiendo al jugador
    agent.isStopped = false;
}
```

**Lo interesante:**

- **Transición física-animación:** Las colisiones del motor de física se convierten directamente en cambios de estado del Animator, sin necesidad de flags intermedias
- **Detención elegante del NavMeshAgent:** Usa `SetDestination(transform.position)` antes de `isStopped = true` para evitar deslizamiento residual
- **Cooldown de persecución:** El `Invoke("ResumeAgent", 3f)` tras perder contacto impide que el zombie vuelva a perseguir instantáneamente, dando ventana de escape al jugador
- **Validación de estado:** Comprueba `!agent.isStopped` antes de detener, previniendo llamadas redundantes que podrían causar bugs

**Impacto en gameplay:** Crea enemigos con comportamiento orgánico que reaccionan naturalmente al jugador, alternando entre persecución agresiva y combate cuerpo a cuerpo sin necesidad de FSM explícita.

---

##### 5. Sistema de Power-Ups con Comunicación por Mensajes ⭐⭐

**Ubicación:** `PowerUpApply.cs` - método `OnTriggerEnter()`

**Descripción:**
Implementación de power-ups que usa el sistema de mensajes de Unity para aplicar efectos sin acoplamiento fuerte entre scripts, combinado con audio posicional.

**Código:**

```csharp
void OnTriggerEnter(Collider other)
{
    // Verificar si el objeto que entró es el jugador
    if (other.CompareTag("Player"))
    {
        // Aplicar curación al jugador (daño negativo restaura salud)
        other.gameObject.SendMessage("ApplyDamage", -POWER);

        // Reproducir el sonido del power-up en la posición actual
        AudioSource.PlayClipAtPoint(clip, transform.position);
        
        // Destruir el GameObject del power-up
        Destroy(gameObject);
    }
}
```

**Lo interesante:**

- **Desacoplamiento mediante SendMessage:** No requiere referencia directa a `PlayerDamage`, permitiendo reutilizar el script en múltiples tipos de power-ups
- **Curación como daño negativo:** Reutiliza el método `ApplyDamage()` existente pasando `-POWER` en lugar de crear un método `Heal()` separado, siguiendo el principio DRY
- **Audio espacial con PlayClipAtPoint:** Reproduce el sonido en la posición del power-up antes de destruirlo, creando feedback auditivo posicional correcto
- **Trigger vs Collider:** Usa `OnTriggerEnter` con trigger collider, permitiendo recoger el power-up sin afectar la física del movimiento del jugador

**Impacto en gameplay:** Proporciona feedback multi-sensorial (visual + auditivo) instantáneo al recoger power-ups, reforzando la sensación de recompensa y permitiendo al jugador localizarlos acústicamente.

---

##### 6. Spawner Inteligente con Control de Existencia ⭐⭐⭐

**Ubicación:** `PowerUpSpawner.cs` - corrutina `Spawn()`

**Descripción:**
Sistema de generación que mantiene exactamente un power-up activo en el mapa, reemplazándolo automáticamente cuando es recogido, usando comprobaciones de referencia null.

**Código:**

```csharp
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
```

**Lo interesante:**

- **Singleton de power-up:** Mantiene la referencia `powerUp` para asegurar que solo existe uno a la vez, previniendo saturación del mapa
- **Detección automática de destrucción:** Cuando el power-up es recogido y destruido, la referencia se vuelve `null` automáticamente, disparando el respawn
- **Randomización de posición:** Usa `Random.Range` con array de `Transform[]` predefinidos, permitiendo diseñar manualmente puntos estratégicos de spawn
- **Polling optimizado:** Verifica cada 0.5 segundos en lugar de cada frame, balanceando responsividad con eficiencia

**Impacto en gameplay:** Crea un sistema de recursos escasos que fuerza al jugador a moverse por el mapa estratégicamente, aumentando el riesgo-recompensa de buscar curaciones.

---

##### 7. Sistema de Disparo con Auto-Destrucción Temporizada ⭐⭐

**Ubicación:** `Fireproyectile.cs` - método `Update()`

**Descripción:**
Implementación minimalista de sistema de disparo que combina instanciación de proyectiles con limpieza automática para prevenir acumulación de objetos.

**Código:**

```csharp
void Update()
{
    // Detectar si se presiona el botón de disparo (click izquierdo del mouse)
    if (Input.GetButtonDown("Fire1"))
    {
        // Instanciar el proyectil en la posición y rotación actual del objeto
        GameObject clone = Instantiate(proyectile, transform.position, transform.rotation);

        // Destruir el proyectil después del tiempo especificado
        Destroy(clone, delay);
    }
}
```

**Lo interesante:**

- **Limpieza automática de memoria:** `Destroy(clone, delay)` programa la destrucción en el momento de creación, previniendo memory leaks sin necesidad de scripts adicionales
- **Herencia de rotación:** Usa `transform.rotation` del punto de disparo (típicamente la cámara), haciendo que el proyectil viaje en la dirección de la mira
- **GetButtonDown para single-shot:** Usa `GetButtonDown` en lugar de `GetButton`, requiriendo soltar y presionar de nuevo para cada disparo, previniendo spam involuntario
- **Simplicidad mediante composición:** Delega el movimiento a `ProyectileMovement.cs` adjunto al prefab, siguiendo el principio de responsabilidad única

**Impacto en gameplay:** Permite disparos rápidos y responsivos sin sobrecarga de CPU por proyectiles fuera de pantalla, manteniendo el framerate estable en combates intensos.

---

##### 8. Sistema de Salud con Contador de Impactos ⭐⭐

**Ubicación:** `EnemyDamage.cs` - método `OnCollisionEnter()`

**Descripción:**
Sistema simplificado de salud que cuenta impactos discretos en lugar de usar puntos de vida numéricos, optimizado para enemigos básicos con feedback claro.

**Código:**

```csharp
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
```

**Lo interesante:**

- **Salud discreta vs continua:** Usa contador de hits (`hitCount`) en lugar de HP numérico, simplificando la lógica y haciendo el comportamiento más predecible para el jugador
- **Constante de balance:** `HITS_TO_DIE` definida como `const int` facilita el tweaking de dificultad sin tocar lógica
- **Sin validación de estado:** No comprueba si ya está muerto antes de incrementar, asumiendo que `Destroy(gameObject)` lo maneja, reduciendo complejidad
- **Comparación exacta:** Usa `==` en lugar de `>=`, permitiendo potencialmente implementar overkill o efectos especiales en impactos excesivos

**Impacto en gameplay:** Crea feedback tangible donde cada disparo cuenta visualmente, comunicando al jugador cuántos hits más necesita sin necesidad de barras de vida.

---

##### 9. Efectos de Impacto con Object Pooling Implícito ⭐⭐

**Ubicación:** `BulletHit.cs` - método `OnCollisionEnter()`

**Descripción:**
Sistema de efectos visuales que usa `SetActive(false)` en lugar de `Destroy()` para las balas, preparando el código para implementar object pooling sin modificaciones adicionales.

**Código:**

```csharp
void OnCollisionEnter(Collision collision)
{
    // Instanciar el efecto de partículas en la posición del impacto
    // Se utiliza Quaternion.identity para no aplicar rotación
    Instantiate(particle, transform.position, Quaternion.identity);
    
    // Desactivar el GameObject de la bala
    // Se usa SetActive en lugar de Destroy para posible reutilización
    gameObject.SetActive(false);
}
```

**Lo interesante:**

- **SetActive vs Destroy:** Desactiva la bala en lugar de destruirla, permitiendo implementar object pooling sin refactorizar (las balas inactivas pueden reactivarse desde un pool)
- **Efecto instantáneo:** Usa `Instantiate` para las partículas con `Quaternion.identity`, creando efectos orientados hacia arriba sin importar el ángulo de impacto
- **Separación de responsabilidades:** Las partículas instanciadas manejan su propia destrucción vía `DestroyEffect.cs`, desacoplando la lógica
- **Colisión indiscriminada:** No verifica contra qué colisionó, generando efectos visuales en cualquier impacto (terreno, enemigos, props)

**Impacto en gameplay:** Proporciona feedback visual instantáneo de cada disparo, ayudando al jugador a confirmar que sus balas están conectando incluso antes de ver el efecto en el enemigo.

---

##### 10. Navegación Automática con NavMesh Agent ⭐⭐

**Ubicación:** `MoveToPosition.cs` - método `Update()`

**Descripción:**
Sistema de persecución enemiga que aprovecha el NavMesh de Unity para pathfinding automático, actualizando continuamente el destino hacia la posición del jugador.

**Código:**

```csharp
void Start()
{
    // Buscar el GameObject con tag "Player" y obtener su Transform
    target = GameObject.FindGameObjectWithTag("Player").transform;

    // Obtener el componente NavMeshAgent del enemigo
    agent = GetComponent<NavMeshAgent>();
}

void Update()
{
    // Verificar que el agente no esté detenido antes de actualizar el destino
    if (!agent.isStopped)
    {
        // Establecer la posición del jugador como destino del agente
        agent.SetDestination(target.position);
    }
}
```

**Lo interesante:**

- **Pathfinding automático:** Delega toda la navegación compleja al NavMeshAgent, que calcula rutas alrededor de obstáculos sin lógica adicional
- **Actualización continua de destino:** Llama a `SetDestination` cada frame con la posición actualizada del jugador, haciendo que los enemigos persigan en tiempo real
- **Búsqueda por tag en Start:** Encuentra al jugador dinámicamente en lugar de requerir asignación manual, permitiendo reutilizar el script en múltiples enemigos
- **Respeto del estado detenido:** La validación `!agent.isStopped` permite que otros scripts (como `ZombieAnim.cs`) interrumpan la persecución temporalmente

**Impacto en gameplay:** Crea enemigos inteligentes que navegan naturalmente por el entorno, proporcionando desafío constante sin necesidad de waypoints o comportamiento scripteado complejo.
