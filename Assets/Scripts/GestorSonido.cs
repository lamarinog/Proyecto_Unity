using UnityEngine;
using System.Collections;

public class GestorSonido : MonoBehaviour
{
    public static GestorSonido Instancia;
    public AudioSource audioSource;
    public AudioClip clipRecogerLlave;

    //SE CREA EL SINGLETONE DEL SONIDO
    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //SE ESTABLECE LA MUSICA CON EL VALOR DEL TOOGLE ASIGNADO EN LA ESCENA.
    public void EstablecerMusica(bool activada)
    {
        if (audioSource == null) return;
        if (activada)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Pause();
        }
    }

    public void ReproducirSonidoRecogerLlaveCorto()
    {
        // Verifica que la fuente de audio y el clip estén asignados.
        if (audioSource != null && clipRecogerLlave != null)
        {
            // PlayOneShot permite reproducir sonidos de efectos por encima de la música.
            audioSource.PlayOneShot(clipRecogerLlave);
        }
        else
        {
            Debug.LogWarning("GestorSonido: No se pudo reproducir el sonido de la llave. Verifica que 'audioSource' y 'clipRecogerLlave' estén asignados en el Inspector.");
        }
    }

    public void SecuenciaLlaveYMusica()
    {
        StartCoroutine(ReproducirSonidoRecogerLlave());
    }

    private IEnumerator ReproducirSonidoRecogerLlave()
    {
        // 1. Reproducir el Sonido de la Llave
        if (audioSource != null && clipRecogerLlave != null)
        {
            float duracionClip = clipRecogerLlave.length;
            audioSource.PlayOneShot(clipRecogerLlave);

            // 2. Esperar la duración del clip (para que termine de sonar)
            yield return new WaitForSecondsRealtime(duracionClip);
            // Usamos Realtime porque el juego está pausado (Time.timeScale = 0)
        }
        else
        {
            Debug.LogWarning("GestorSonido: Clip de llave no asignado. Reanudando música de inmediato.");
        }

        // 3. Reanudar la Música
        EstablecerMusica(true); // true = reanudar (Play)
    }

    //METODO PARA VER SI LA MUSICA ESTA ON
    public bool EstaMusicaActivada()
    {
        return audioSource.isPlaying;
    }
}