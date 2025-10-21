using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
public class JefeMazmorra : MonoBehaviour, IPuzleGestor
{
    public static JefeMazmorra InstanciaMazmorra;

    [Header("UI del Puzle")]
    public GameObject panelPuzle;
    public TextMeshProUGUI textoPregunta;
    public Transform contenedorBotones;
    public GameObject botonRespuestaPrefab;

    [Header("Configuración de Escenas")]
    private int puzlesResueltos = 0;

    //ESTRUCTURA PARA EL PUZLE 
    private struct Problema
    {
        public string pregunta;
        public int respuestaCorrecta;
    }

    private List<Problema> problemasFijos;
    private Problema problemaActual;

    //SE INICIAN LOS VALORES DE LA INSTANCIA
    void Awake()
    {
        if (InstanciaMazmorra == null)
        {
            InstanciaMazmorra = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        problemasFijos = new List<Problema>
        {
            new Problema { pregunta = "¿Cuál es el valor de 'x' en la siguiente ecuación: x + 6 * 9 = 2?", respuestaCorrecta = -52 },
            new Problema { pregunta = "Si x > 0, ¿cuál es el valor de 'x' en: x^2 - 6 - 2 * 5 = 0?", respuestaCorrecta = 4 },
            new Problema { pregunta = "En el sistema x + y = 10 y x - y = 2, ¿cuánto es x * y?", respuestaCorrecta = 24 }
        };
    }

    //SE LLAMA LOS VALORES DE LAS INSTRUCCIONES EN CONTROLADOR JUEGO
    void Start()
    {
        if (panelPuzle != null) panelPuzle.SetActive(false);
        if (ControladorJuego.Instancia != null)
        {
            ControladorJuego.Instancia.IniciarNivelMazmorra();
        }
        else
        {
            Debug.LogError("JefeMazmorra: ControladorJuego.Instancia es NULL.");
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //SI SE TOPA AL JEFE INICIA LOS DESAFIOS
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && puzlesResueltos < problemasFijos.Count)
        {
            if (ControladorJuego.Instancia != null && ControladorJuego.Instancia.juegoActivo)
            {
                IniciarPuzle();
            }
        }
    }

    //SE INICIA LOS DESAFIOS
    public void IniciarPuzle()
    {
        if (puzlesResueltos >= problemasFijos.Count)
        {
            FinalizarPuzle();
            return;
        }
        panelPuzle.SetActive(true);
        if (GestorSonido.Instancia != null)
        {
            GestorSonido.Instancia.EstablecerMusica(false);
        }
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        problemaActual = problemasFijos[puzlesResueltos];
        textoPregunta.text = $"Puzle {puzlesResueltos + 1} de {problemasFijos.Count}: {problemaActual.pregunta}";
        GenerarOpciones(problemaActual.respuestaCorrecta);
    }

    //SE GENERAN LOS BOTONES CON VALORES Y SE RECORRE LOS 3 PROBLEMAS CONTINUAMENTE
    private void GenerarOpciones(int respuestaCorrecta)
    {
        if (contenedorBotones == null || botonRespuestaPrefab == null)
        {
            Debug.LogError("JefeMazmorra: 'contenedorBotones' o 'botonRespuestaPrefab' son nulos. Revisa el Inspector.");
            return;
        }
        foreach (Transform child in contenedorBotones)
        {
            Destroy(child.gameObject);
        }
        List<int> opciones = new List<int> { respuestaCorrecta };
        while (opciones.Count < 4)
        {
            int opcionIncorrecta = respuestaCorrecta + Random.Range(-10, 10);
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
            else
            {
                Debug.LogError("JefeMazmorra: El Prefab del botón instanciado no tiene el script BotonRespuesta.");
            }
        }
    }

    //SE VERIFICA SI EL BOTON TIENE LA RESPUESTA CORRECTA, SE QUITA UNA VIDA SI SE RESPONDE MAL
    public void ProcesarRespuesta(int valorRespuesta, bool esCorrecta)
    {
        if (esCorrecta)
        {
            puzlesResueltos++;
            if (GestorSonido.Instancia != null)
            {
                GestorSonido.Instancia.ReproducirSonidoRecogerLlaveCorto();
            }
            if (puzlesResueltos == problemasFijos.Count)
            {
                if (GestorSonido.Instancia != null)
                {
                    GestorSonido.Instancia.EstablecerMusica(true); // Reanuda la música de fondo
                }
                FinalizarPuzle();
                if (ControladorJuego.Instancia != null)
                {
                    ControladorJuego.Instancia.FinalizarJuego(true);
                }
            }
            else
            {
                IniciarPuzle();
            }
        }
        else
        {
            if (ControladorJuego.Instancia != null)
            {
                ControladorJuego.Instancia.RestarVida();
            }
        }
    }

    //SE OCULTA EL PANEL DE PUZLE
    void FinalizarPuzle()
    {
        if (panelPuzle != null) panelPuzle.SetActive(false);
        if (ControladorJuego.Instancia != null)
        {
            ControladorJuego.Instancia.IniciarJuego();
        }
    }
}