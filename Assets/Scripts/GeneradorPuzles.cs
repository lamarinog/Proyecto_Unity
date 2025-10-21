using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GeneradorPuzles : MonoBehaviour, IPuzleGestor
{
    [Header("UI y Puzles")]
    public GameObject panelPuzles;
    public TextMeshProUGUI textoPregunta;
    public Transform contenedorBotones;
    public GameObject botonRespuestaPrefab;

    [Header("Configuración")]
    public int puzleIndex;
    public bool yaResuelto = false;
    public GameObject ObjetoLlaveAsociada;

    //ESTRUCTURA DE DATOS PARA GUARDAR VALORES STRING, INT
    private struct Problema
    {
        public string Pregunta;
        public int RespuestaCorrecta;
    }

    //SE CREA UNA LISTA DE 3 STRUCT PROBLEMAS
    private List<Problema> problemasFijos = new List<Problema>
    {
        new Problema { Pregunta = "Desafio: ¿2 + 2 * -2?", RespuestaCorrecta = -2 },
        new Problema { Pregunta = "Desafio: ¿10 - 3 * 2?", RespuestaCorrecta = 4 },
        new Problema { Pregunta = "Desafio: ¿(5 + 5) * 5?", RespuestaCorrecta = 50 }
    };

    private Problema problemaActual;

    //SE ACTIVA EL PANEL DE PUZLE
    void Start()
    {
        if (panelPuzles != null)
        {
            panelPuzles.SetActive(false);
        }
    }

    //SI SE TOCA EL COLLIDER DE LA LLAVE SE INICIA
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaResuelto)
        {
            IniciarPuzle();
        }
    }

    //SE INICIA EL PUZLE EN PANTALLA
    public void IniciarPuzle()
    {
        if (yaResuelto) return;
        if (puzleIndex < 0 || puzleIndex >= problemasFijos.Count)
        {
            Debug.LogError($"GeneradorPuzles: El puzleIndex {puzleIndex} está fuera de rango. VERIFICA EL INSPECTOR.");
            return;
        }
        if (panelPuzles == null || textoPregunta == null)
        {
            Debug.LogError("GeneradorPuzles: Las referencias de UI son nulas. Asigna el PanelPuzles del MUNDO ABIERTO en el Inspector.");
            return;
        }
        problemaActual = problemasFijos[puzleIndex];
        panelPuzles.SetActive(true);
        if (GestorSonido.Instancia != null)
        {
            GestorSonido.Instancia.EstablecerMusica(false);
        }
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        textoPregunta.text = problemaActual.Pregunta;
        GenerarOpciones(problemaActual.RespuestaCorrecta);
    }

    //SE GENERAN LOS 4 BOTONES Y LOS ASIGNA AL ARREGLO DE BOTONES CON LAS OPCIONES Y VALORES
    private void GenerarOpciones(int respuestaCorrecta)
    {
        if (contenedorBotones == null || botonRespuestaPrefab == null)
        {
            Debug.LogError("GeneradorPuzles: 'contenedorBotones' o 'botonRespuestaPrefab' son nulos.");
            return;
        }
        foreach (Transform child in contenedorBotones)
        {
            Destroy(child.gameObject);
        }
        List<int> opciones = new List<int> { respuestaCorrecta };
        while (opciones.Count < 4)
        {
            int opcionIncorrecta = respuestaCorrecta + Random.Range(-20, 20);
            if (opcionIncorrecta != respuestaCorrecta && !opciones.Contains(opcionIncorrecta))
            {
                opciones.Add(opcionIncorrecta);
            }
        }
        opciones.Shuffle();
        foreach (int opcion in opciones)
        {
            GameObject botonObj = Instantiate(botonRespuestaPrefab, contenedorBotones);
            BotonRespuesta botonScript = botonObj.GetComponent<BotonRespuesta>();
            if (botonScript != null)
            {
                bool esCorrecta = opcion == respuestaCorrecta;
                botonScript.Configurar(opcion, esCorrecta, this);
            }
        }
    }

    //SE VERIFICA LA RESPUESTA CUANDO SE DA EL CLIC EN EL BOTON RESPUESTA
    public void ProcesarRespuesta(int valor, bool esCorrecta)
    {
        FinalizarPuzle();
        if (esCorrecta)
        {
            yaResuelto = true;
            if (ControladorJuego.Instancia != null)
            {
                ControladorJuego.Instancia.RecogerLlave();
            }
            if (GestorSonido.Instancia != null)
            {
                GestorSonido.Instancia.SecuenciaLlaveYMusica();
            }
            if (ObjetoLlaveAsociada != null)
            {
                Destroy(ObjetoLlaveAsociada);
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Respuesta incorrecta. Intenta de nuevo.");
        }
    }

    //SE FINALIZA EL PUZLE ACTUAL
    void FinalizarPuzle()
    {
        if (panelPuzles != null) panelPuzles.SetActive(false);
        if (ControladorJuego.Instancia != null)
        {
            ControladorJuego.Instancia.IniciarJuego();
        }
    }
}