using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasosProcedurales : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 5f;
    public LayerMask capaSuelo;
    public Transform chequeoSuelo;
    public float radioChequeo = 0.3f;

    [Header("Pasos")]
    public AudioSource audioSource;
    public AudioClip[] sonidosPasos;
    public Vector2 rangoPitch = new Vector2(0.9f, 1.1f);
    public float distanciaEntrePasos = 0.5f;
    public float volumen = 1f;

    private Rigidbody rb;
    private Vector3 posicionAnterior;
    private float acumuladorDistancia;
    private int ultimoIndice = -1;

    private bool EnSuelo()
    {
        return Physics.CheckSphere(chequeoSuelo.position, radioChequeo, capaSuelo);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posicionAnterior = transform.position;
    }

    void FixedUpdate() // usamos FixedUpdate para física más precisa
    {
        // Movimiento
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direccion = new Vector3(h, 0, v).normalized;
        Vector3 nuevaVelocidad = new Vector3(direccion.x * velocidad, rb.velocity.y, direccion.z * velocidad);
        rb.velocity = nuevaVelocidad;

        // Salto
        if (Input.GetButtonDown("Jump") && EnSuelo())
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }

        // Acumulación de distancia
        float distancia = Vector3.Distance(transform.position, posicionAnterior);
        acumuladorDistancia += distancia;

        if (acumuladorDistancia >= distanciaEntrePasos && EnSuelo() && direccion.magnitude > 0.1f)
        {
            ReproducirPaso();
            acumuladorDistancia = 0f;
        }

        posicionAnterior = transform.position;
    }

    void ReproducirPaso()
    {
        if (sonidosPasos.Length == 0) return;

        int nuevoIndice;
        do
        {
            nuevoIndice = Random.Range(0, sonidosPasos.Length);
        } while (nuevoIndice == ultimoIndice && sonidosPasos.Length > 1);

        ultimoIndice = nuevoIndice;

        AudioClip clip = sonidosPasos[nuevoIndice];
        audioSource.pitch = Random.Range(rangoPitch.x, rangoPitch.y);
        audioSource.PlayOneShot(clip, volumen);
    }
}
