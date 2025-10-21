using UnityEngine;

public class BotonIniciar : MonoBehaviour
{
    //SCRIPT PARA INICIAR JUEGO EN BOTONES DE COMENZAR
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