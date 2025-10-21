using UnityEngine;
using UnityEngine.SceneManagement;

public class CofreMazmorra : MonoBehaviour
{
    private string nombreEscenaSiguiente = "Mazmorra";
    private int llavesRequeridas = 3;
    private bool puedeActivar = false;
    private bool cofreAbierto = false;

    //SI EL PLAYER HACE COLLISSION PUEDE INTERACTUAR CON EL COFRE
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puedeActivar = true;
        }
    }

    //SI SE SALE DEL COLLIDER NO PUEDE INTERACTUAR CON EL COFRE
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puedeActivar = false;
        }
    }

    //SI ESTA DENTRO DEL COLLIDER Y EL COFRE NO HA SIDO USADO SE PUEDE APLASTAR "E" PARA INTERACTUAR
    void Update()
    {
        if (puedeActivar && !cofreAbierto && Input.GetKeyDown(KeyCode.E))
        {
            IntentarAbrirCofre();
        }
    }

    //SI LAS LLAVES SON 3 INTERACTUA CON EL COFRE LO CUAL LLEVA AL SIGUIENTE NIVEL
    void IntentarAbrirCofre()
    {
        if (ControladorJuego.Instancia != null)
        {
            if (ControladorJuego.Instancia.LlavesRecogidas == llavesRequeridas)
            {
                cofreAbierto = true;
                Time.timeScale = 1f;
                //ControladorJuego.Instancia.IniciarNivelMazmorra();
                SceneManager.LoadScene(nombreEscenaSiguiente);
            }
        }
        else
        {
            Debug.LogError("CofreMazmorra: El ControladorJuego.Instancia es NULL. ¡El juego no puede continuar!");
        }
    }
}