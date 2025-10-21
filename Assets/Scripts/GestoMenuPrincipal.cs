using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorMenuPrincipal : MonoBehaviour
{
    //SE INICIA EL MOUSE Y RENAUDA EL TIEMPO
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //SE CARGA EL MENU PRINCIPAL, SE ASIGNA A BOTONES DE MENU PRINCIPAL
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}