using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaAmbiente : MonoBehaviour
{

    public AudioSource musicaSource;        // Loop ON
    public AudioSource transicionSource;    // Solo para transición (PlayOneShot)

    [Header("Clips por zona")]
    public AudioClip musicaBosque;
    public AudioClip musicaCueva;
    public AudioClip clipTransicion;

    private string zonaActual = "";
    private Coroutine rutinaCambio;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bosque") && zonaActual != "Bosque")
        {
            CambiarMusicaZona(musicaBosque, "Bosque");
        }
        else if (other.CompareTag("Cueva") && zonaActual != "Cueva")
        {
            CambiarMusicaZona(musicaCueva, "Cueva");
        }
    }

    void CambiarMusicaZona(AudioClip nuevaMusica, string nuevaZona)
    {
        if (rutinaCambio != null)
        {
            StopCoroutine(rutinaCambio);
        }
        rutinaCambio = StartCoroutine(CambiarMusica(nuevaMusica, nuevaZona));
    }

    IEnumerator CambiarMusica(AudioClip nuevaMusica, string nuevaZona)
    {
        // Cortamos música actual con fade
        yield return StartCoroutine(FadeOut(musicaSource, 1f));
        musicaSource.Stop(); // Cortamos por si acaso

        // Transición
        if (clipTransicion != null)
        {
            transicionSource.PlayOneShot(clipTransicion);
            yield return new WaitForSeconds(clipTransicion.length);
        }

        // Asignamos nueva música, actualizamos zona
        musicaSource.clip = nuevaMusica;
        zonaActual = nuevaZona;

        musicaSource.Play();
        yield return StartCoroutine(FadeIn(musicaSource, 1f));
    }

    IEnumerator FadeOut(AudioSource source, float duracion)
    {
        float tiempo = 0f;
        float volumenInicial = source.volume;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            source.volume = Mathf.Lerp(volumenInicial, 0f, tiempo / duracion);
            yield return null;
        }

        source.volume = 0f;
    }

    IEnumerator FadeIn(AudioSource source, float duracion)
    {
        float tiempo = 0f;
        float volumenFinal = 1f;
        source.volume = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, volumenFinal, tiempo / duracion);
            yield return null;
        }

        source.volume = volumenFinal;
    }
}