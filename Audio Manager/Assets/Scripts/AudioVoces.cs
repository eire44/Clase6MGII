using System.Collections;
using UnityEngine;

public class AudioVoces : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoBase;
    public float intervalo = 0.05f; // tiempo entre cada "letra"
    public Vector2 rangoPitch = new Vector2(0.9f, 1.2f);
    public string texto;

    public void ReproducirTexto()
    {
        StopAllCoroutines();
        StartCoroutine(ProcesarTexto(texto));
    }

    IEnumerator ProcesarTexto(string texto)
    {
        foreach (char letra in texto)
        {
            if (char.IsLetterOrDigit(letra))
            {
                audioSource.pitch = Random.Range(rangoPitch.x, rangoPitch.y);
                audioSource.PlayOneShot(sonidoBase);
            }
            yield return new WaitForSeconds(intervalo);
        }
    }
    
}