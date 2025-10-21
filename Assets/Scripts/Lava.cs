using UnityEngine;

public class Lava : MonoBehaviour
{
    //PARA VERIFICAR LA COLISSION DEL PLAYER CON LA LAVA Y REINICIARLO AL SPAWN
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ControladorJuego.Instancia != null)
        {
            ControladorJuego.Instancia.ReiniciarNivel();
        }
    }
}