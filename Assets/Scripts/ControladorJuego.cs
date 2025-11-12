using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControladorJuego : MonoBehaviour
{
    public static ControladorJuego Instancia;

    [Header("Configuración de Escenas")]
    public string NombreEscenaDerrota = "Derrota";
    public string NombreEscenaVictoria = "Victoria";
    public string NombreEscenaMenuPrincipal = "MenuPrincipal";

    [Header("Referencias del Jugador")]
    public Transform PlayerSpawnPoint;
    public GameObject PlayerArmature;
    public CharacterController PlayerController;

    [Header("Referencias de UI Persistentes")]
    public GameObject PanelHUD;
    public GameObject PanelMenuPausa;
    public GameObject PanelInstrucciones;
    public TextMeshProUGUI TextoTiempo;
    public GameObject PanelPuzle;

    [Header("Gestión de Vidas (Mazmorra)")]
    public TextMeshProUGUI TextoVidas;
    public int VidasMaximas = 2;
    public int VidasActuales;

    [Header("Gestión de Llaves")]
    public TextMeshProUGUI TextoLlaves;
    public int LlavesRecogidas = 0;
    public int TotalLlaves = 3;

    [Header("Gestión de Tiempo")]
    public float TiempoLimite = 120f;
    public float tiempoRestante;
    public bool juegoActivo = false;

    //SINGLETONE DE LA ESTANCIA GESTORJUEGO
    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //VERIFICA EL TIEMPO Y LO ACTUALIZA
    void Update()
    {
        if (juegoActivo && tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarTiempoUI();

            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                FinalizarJuego(false);
            }
        }
    }

    //INICIA LAS INSTRUCCIONES DEL NIVEL 1
    public void IniciarNivelMundoAbierto()
    {
        instrucciones();
    }

    //INICIA LAS INSTRUCCIONES DEL NIVEL 2 Y ASIGNA VIDAS
    public void IniciarNivelMazmorra()
    {
        instrucciones();
        VidasActuales = VidasMaximas;
    }

    //MUESTRA LAS INSTRUCCIONES Y CONGELA EL HUD
    public void instrucciones()
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        Cursor.visible = true;
        tiempoRestante = TiempoLimite;
        Cursor.lockState = CursorLockMode.None;
        if (PanelInstrucciones != null) PanelInstrucciones.SetActive(true);
        if (PanelHUD != null) PanelHUD.SetActive(true);
        ActualizarHUD();
    }

    //HACE QUE EL TIEMPO SE ACTUALICE, PONE EL JUEGO A INICIO Y OCULTA PANELES QUE NO SE USAN
    public void IniciarJuego()
    {
        juegoActivo = true;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }
        if (PanelInstrucciones != null) PanelInstrucciones.SetActive(false);
        if (PanelMenuPausa != null) PanelMenuPausa.SetActive(false);
        if (PanelHUD != null) PanelHUD.SetActive(true);
    }

    //VERIFICA SI SE GANA O SE PIERDE
    public void FinalizarJuego(bool victoria)
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (victoria)
        {
            SceneManager.LoadScene(NombreEscenaVictoria);
        }
        else
        {
            SceneManager.LoadScene(NombreEscenaDerrota);
        }
    }

    //METODO SI SE RECOGE LLAVE, ACTUALIZA EN +1
    public void RecogerLlave()
    {
        LlavesRecogidas++;
        ActualizarHUD();
    }

    //SI SE COMETE UN ERROR EN PUZLE DE MAZMORRA SE RESTA 1 VIDA
    public void RestarVida()
    {
        if (VidasActuales > 0)
        {
            VidasActuales--;
            ActualizarVidas();
            if (VidasActuales == 0)
            {
                FinalizarJuego(false);
            }
        }
    }

    //ACTUALIZA LLAVES, TIEMPO, VIDA
    public void ActualizarHUD()
    {
        ActualizarLlaves();
        ActualizarVidas();
        ActualizarTiempoUI();
        if (PanelHUD != null) PanelHUD.SetActive(true);
    }

    //ACTUALIZA EL TEXTO DE LLAVES
    private void ActualizarLlaves()
    {
        if (TextoLlaves != null)
        {
            TextoLlaves.text = $"Llaves: {LlavesRecogidas}/{TotalLlaves}";
        }
    }

    //ACTUALIZA EL TEXTO DE VIDAS
    private void ActualizarVidas()
    {
        if (TextoVidas != null)
        {
            TextoVidas.text = $"Vidas: {VidasActuales}";
        }
    }

    //ACTUALIZA EL TEXTO DE TIEMPO
    private void ActualizarTiempoUI()
    {
        if (TextoTiempo != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            TextoTiempo.text = $"Tiempo: {minutos:00}:{segundos:00}";
        }
    }

    //SI SE CAE A LA LAVA/KILLZONE SE RESTABLECE AL SPAWN ASIGNADO
    public void ReiniciarNivel()
    {
        if (PlayerArmature != null && PlayerSpawnPoint != null && SceneManager.GetActiveScene().name == "MundoAbierto")
        {
            if (PlayerController != null)
            {
                PlayerController.enabled = false;
                PlayerArmature.transform.position = PlayerSpawnPoint.position;
                PlayerController.enabled = true;
            }
            IniciarJuego();
        }
    }

    //SE PAUSA EL JUEGO CON LA TECLA ASIGNADA DESDE TERCERA PERSONA SCRIPT
    public void PausarJuego()
    {
        if (juegoActivo)
        {
            juegoActivo = false;
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (PanelPuzle != null) PanelPuzle.SetActive(false);
            if (PanelMenuPausa != null) PanelMenuPausa.SetActive(true);
            if (PanelHUD != null) PanelHUD.SetActive(false);
        }
    }
}