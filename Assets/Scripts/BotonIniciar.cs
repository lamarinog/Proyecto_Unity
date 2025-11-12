using UnityEngine;

public class BotonIniciar : MonoBehaviour
{
    //SCRIPT PARA ASIGNAR INICIAR JUEGO EN BOTONES INSPECTOR
    public void ContinuarJuego()
    {
        if (ControladorJuego.Instancia != null)
        {
            ControladorJuego.Instancia.IniciarJuego(); 
        }
        else
        {
            Debug.LogError("Error: El ControladorJuego (Singleton) no se encontró. Revisa que esté en MenuPrincipal.");
        }
    }
}