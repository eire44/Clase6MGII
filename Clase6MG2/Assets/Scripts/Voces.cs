using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Voces : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoBase;
    public float intervalo = 0.05f;
    public Vector2 rangoPitch = new Vector2(0.9f, 1.2f);
    public string texto;


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


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Jugador"))
        {
            StopAllCoroutines();
            StartCoroutine(ProcesarTexto(texto));
        }
        
    }
}
