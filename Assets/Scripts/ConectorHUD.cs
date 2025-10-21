using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ConectorHUD : MonoBehaviour
{
    [Header("Referencias Locales del HUD")]
    public GameObject PanelHUDLocal;
    public TextMeshProUGUI TextoTiempoLocal;
    public TextMeshProUGUI TextoLlavesLocal;
    public GameObject PanelMenuPausaLocal;
    public GameObject PanelPuzleLocal;

    [Header("Referencias Locales de Vidas e Instrucciones")]
    public TextMeshProUGUI TextoVidasLocal;
    public GameObject PanelInstruccionesLocal;

    [Header("Referencias Locales del Jugador (Mundo Abierto)")]
    public Transform PlayerSpawnPointLocal;
    public GameObject PlayerArmatureLocal;
    public CharacterController PlayerControllerLocal;

    //CARGA TODAS LAS VARIABLES QUE SE USAN EN EL SINGLETONE DE GESTORJUEGO, PARA PODER CONECTAR EN LAS OTRAS ESCENAS
    void Start()
    {
        if (ControladorJuego.Instancia != null)
        {
            ControladorJuego.Instancia.PanelHUD = PanelHUDLocal;
            ControladorJuego.Instancia.PanelMenuPausa = PanelMenuPausaLocal;
            ControladorJuego.Instancia.TextoTiempo = TextoTiempoLocal;
            ControladorJuego.Instancia.TextoLlaves = TextoLlavesLocal;
            ControladorJuego.Instancia.TextoVidas = TextoVidasLocal;
            ControladorJuego.Instancia.PanelInstrucciones = PanelInstruccionesLocal;
            ControladorJuego.Instancia.PlayerSpawnPoint = PlayerSpawnPointLocal;
            ControladorJuego.Instancia.PlayerArmature = PlayerArmatureLocal;
            ControladorJuego.Instancia.PlayerController = PlayerControllerLocal;
            ControladorJuego.Instancia.PanelPuzle = PanelPuzleLocal;
            string nombreEscena = SceneManager.GetActiveScene().name;
            if (nombreEscena == "MundoAbierto")
            {
                ControladorJuego.Instancia.IniciarNivelMundoAbierto();
            }
            else if (nombreEscena == "Mazmorra")
            {
                ControladorJuego.Instancia.IniciarNivelMazmorra();
            }
            else
            {
                ControladorJuego.Instancia.ActualizarHUD();
            }
        }
        else
        {
            Debug.LogError("ConectorHUD: ControladorJuego.Instancia es NULL. ¡Revisa que el GestorJuego esté en la escena MenuPrincipal!");
        }
    }
}