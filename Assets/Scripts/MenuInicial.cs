using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicial : MonoBehaviour
{
    private string nombreEscenaMundoAbierto = "MundoAbierto";
    private string nombreEscenaMazmorra = "Mazmorra";
    private const string NivelInicioKey = "NivelInicio";

    public GameObject panelOpciones;
    public GameObject panelPrincipal;
    public Toggle toggleMusica;
    public Dropdown dropdownNivelInicio;

    //SE CARGA EL MENU PRINCIPAL
    void Start()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
        CargarOpcionesIniciales();
    }
    
    //SE INICIA EL JUEGO DESDE DONDE SE LE DIGA EN OPCIONES, SE GUARDA EL NIVEL EN QUE QUEDA EN OPCIONES
    public void IniciarJuego()
    {
        int nivelGuardado = PlayerPrefs.GetInt(NivelInicioKey, 0); 
        string escenaAIniciar = (nivelGuardado == 0) ? nombreEscenaMundoAbierto : nombreEscenaMazmorra;
        SceneManager.LoadScene(escenaAIniciar);
    }

    //SE ACTIVA EL PANEL DE OPCIONES Y SE DESACTIVA EL INICIAL
    public void OpcionesDelJuego()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(false);
        }
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
        }
    }

    //SE DESACTIVA EL PANEL DE OPCIONES Y SE ACTIVA EL INICIAL
    public void CerrarOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(true);
        }
    }

    //PARA TERMINAR EL JUEGO
    public void SalirDelJuego()
    {
        Application.Quit();
    }

    //PARA CARGAR LAS CONFIGURACIONES GUARDADAS EN OPCIONES
    private void CargarOpcionesIniciales()
    {
        if (toggleMusica != null && GestorSonido.Instancia != null)
        {
            toggleMusica.isOn = GestorSonido.Instancia.EstaMusicaActivada();
        }
        int nivelInicio = PlayerPrefs.GetInt(NivelInicioKey, 0);
        if (dropdownNivelInicio != null)
        {
            dropdownNivelInicio.value = nivelInicio;
        }
    }

    //PARA ESTABLECER LA MUSICA DE GESTOR DE AUDIO
    public void EstablecerMusica(bool activada)
    {
        if (GestorSonido.Instancia != null)
        {
            GestorSonido.Instancia.EstablecerMusica(activada);
        }
    }

    //PARA GUARDAR EL NIVEL QUE SE ELIJA DESDE OPCIONES
    public void EstablecerNivelInicio(int indice)
    {
        PlayerPrefs.SetInt(NivelInicioKey, indice);
        PlayerPrefs.Save();
    }
}