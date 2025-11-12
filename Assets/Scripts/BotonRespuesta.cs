using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonRespuesta : MonoBehaviour
{
    private Button botonUI;
    private TextMeshProUGUI textoRespuesta;

    //CLASE AUXILIAR PARA ASIGNAR VALORES A BOTONES CON ATRIBUTOS
    private class BotonData
    {
        public int Valor;
        public bool EsCorrecta;
        public IPuzleGestor GestorPuzle;
    }

    private BotonData data;

    void Awake()
    {
        botonUI = GetComponent<Button>();
        textoRespuesta = GetComponentInChildren<TextMeshProUGUI>();
        data = new BotonData();
        if (botonUI != null)
        {
            botonUI.onClick.AddListener(OnBotonClick);
        }
    }

    //ASIGNA VALORES A LOS BOTONES PARA LOS PUZZLES
    public void Configurar(int val, bool correcta, IPuzleGestor gestor)
    {
        data.Valor = val;
        data.EsCorrecta = correcta;
        data.GestorPuzle = gestor;
        if (textoRespuesta != null)
        {
            textoRespuesta.text = data.Valor.ToString();
        }
    }

    //LLAMA A PROCESARRESPUESTA PARA CONFIRMAR SI ES CORRECTO EL BOTON PRESIONADO O NO
    public void OnBotonClick()
    {
        if (data.GestorPuzle != null)
        {
            data.GestorPuzle.ProcesarRespuesta(data.Valor, data.EsCorrecta);
        }
        else
        {
            Debug.LogError("BotonRespuesta: El GestorPuzle no ha sido asignado. Revisa la función GenerarOpciones en el script que crea este botón.");
        }
    }
}