using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasosPersonaje : MonoBehaviour
{
    float velocidad = 5f;
    Rigidbody rb;

    [Header("Pasos")]
    public AudioSource audioSource;
    public AudioClip[] sonidosPasos;
    public Vector2 rangoPitch = new Vector2(0.9f, 1.1f);
    float distanciaEntrePasos = 1.5f;
    public float volumen = 1f;


    private int paso1 = 0;
    private int paso2 = 0;

    private Vector3 posicionAnterior;
    private float acumuladorDistancia;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 direccion = new Vector3(x, 0, y).normalized;
        Vector3 nuevaVelocidad = new Vector3(direccion.x * velocidad, rb.velocity.y, direccion.z * velocidad);
        rb.velocity = nuevaVelocidad;


        float distancia = Vector3.Distance(transform.position, posicionAnterior);
        acumuladorDistancia += distancia;

        if (acumuladorDistancia >= distanciaEntrePasos && direccion.magnitude > 0.1f)
        {
            ReproducirPasos();
            acumuladorDistancia = 0f;
        }

        posicionAnterior = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pasto"))
        {
            paso1 = 0;
            paso2 = 1;
        }
        else if (collision.gameObject.CompareTag("Arena"))
        {
            paso1 = 2;
            paso2 = 3;
        }
        else if (collision.gameObject.CompareTag("Grava"))
        {
            paso1 = 4;
            paso2 = 5;
        }
        else if (collision.gameObject.CompareTag("Nieve"))
        {
            paso1 = 6;
            paso2 = 7;
        }
        Debug.Log(collision.gameObject.tag);
    }


    void ReproducirPasos()
    {
        if (sonidosPasos.Length == 0) return;

        int nuevoIndice = Random.Range(paso1, paso2);

        
        AudioClip clip = sonidosPasos[nuevoIndice];
        audioSource.pitch = Random.Range(rangoPitch.x, rangoPitch.y);
        audioSource.PlayOneShot(clip, volumen);
    }
}
