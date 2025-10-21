using UnityEngine;
using UnityEngine.AI; // ¡ESENCIAL!

public class Enemigo_IA : MonoBehaviour
{
    // --- Variables de Navegación y AI (Obligatorias) ---
    public NavMeshAgent agente;
    public GameObject target;
    public float rangoPersecucion = 5f; // Distancia para empezar a perseguir

    // Velocidades
    public float velocidadPatrulla = 1.5f;
    public float velocidadPersecucion = 4.5f;

    // --- Variables de Animación y Rutina ---
    public Animator ani;
    public int rutina;
    public float cronometro;

    // --- Variables de Spawn ---
    public Transform PlayerSpawnPoint;
    public CharacterController PlayerController;

    void Start()
    {
        // 1. Inicializar Componentes
        agente = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        agente.enabled = true; // Asegurar que esté activo

        // 2. Asignación de Target y Spawn
        target = GameObject.FindWithTag("Player");
        GameObject spawnObject = GameObject.Find("Spawn");
        if (spawnObject != null)
        {
            PlayerSpawnPoint = spawnObject.transform;
        }

        // 3. Configuración Inicial del NavMesh
        if (agente != null)
        {
             agente.speed = velocidadPatrulla;
             agente.isStopped = false;
        }
    }

    void Update()
    {
        // Comprobación de seguridad
        if (target == null || agente == null || !agente.enabled) return;
        comportamientoEnemigo();
    }

    public void comportamientoEnemigo()
    {
        float distancia = Vector3.Distance(transform.position, target.transform.position);

        if (distancia > rangoPersecucion)
        {
            // --- PATRULLA CONSTANTE (Movimiento solo sobre NavMesh) ---
            agente.speed = velocidadPatrulla;
            agente.isStopped = false; 
            cronometro += Time.deltaTime; 
            
            if (cronometro >= 2) 
            {
                rutina = Random.Range(1, 3); // Decide si Cambia de Ruta (1) o sigue (2)
                cronometro = 0;
            }

            switch (rutina)
            {
                case 1: // DECIDIR NUEVA RUTA
                    // Calcular un punto aleatorio dentro de 10 unidades.
                    Vector3 puntoAleatorio = Random.insideUnitSphere * 10f;
                    puntoAleatorio += transform.position;
                    NavMeshHit hit;
                    
                    // Buscar el punto más cercano en el NavMesh
                    if (NavMesh.SamplePosition(puntoAleatorio, out hit, 10f, NavMesh.AllAreas))
                    {
                        agente.SetDestination(hit.position);
                    }
                    rutina++;
                    break;
                case 2: // MOVER AL DESTINO
                    // Ya tiene un destino, solo camina.
                    ani.SetBool("walk", true);
                    break;
            }
            
            // Si llega al destino, decide la siguiente rutina más rápido
            if (agente.remainingDistance <= agente.stoppingDistance)
            {
                rutina = 1; // Forzar cambio de ruta
            }
        }
        else
        {
            // --- PERSECUCIÓN (Movimiento solo sobre NavMesh) ---
            agente.isStopped = false;
            agente.speed = velocidadPersecucion;
            agente.SetDestination(target.transform.position); 
            ani.SetBool("walk", true); // Correr/Caminar
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController != null && PlayerSpawnPoint != null)
            {
                PlayerController.enabled = false;
                other.transform.position = PlayerSpawnPoint.position;
                PlayerController.enabled = true;
            }
        }
    }
}