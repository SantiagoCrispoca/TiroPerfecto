using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // Necesario para manejar imágenes de UI


public class TiroParabolico : MonoBehaviour
{
    public GameObject puntoPrefab;

    public LayerMask capaSuelo; // asigna "Ground" o la capa de tu Tilemap

    public float minVelComponentX = 7f;
    public float minVelComponentY = 1f;
    public float startDotScale = 0.15f;   // escala del primer punto
    public float endDotScale = 0.08f;   // escala del último punto

    public int cantidadPuntos = 6;
    public float intervaloTiempo = 0.1f;
    public float c = 1.5f;
    public float m = 3.8f;
    public float g = 9.8f;
    public float multiplicadorFuerza = 3f;

    public bool entradaHabilitada = true;  // se usará para permitir o bloquear disparos

    private List<GameObject> puntos = new List<GameObject>();
    private Vector3 puntoInicio;
    private Camera cam;
    private bool lanzado = false;
    public Vector2 velocidad;
    private float dt;

    private Vector3 ballCenterOffset;

    private SpriteRenderer sr;

    private Vector3 posicionOriginal;
    public int vidasLanzamiento = 6; // puedes cambiar el número aquí

    // Opcional: para mostrar vidas en consola
    private int lanzamientosRestantes;
    public float delayAntesDePaneo = 2f;

    public List<GameObject> iconosVidas; // Asigna los 4 íconos de la bolita en el Inspector

    public PantallaFinalManager pantallaFinalManager;



    void Start()
    {
        cam = Camera.main;
        dt = Time.fixedDeltaTime;

        float halfH = GetComponent<SpriteRenderer>().bounds.extents.y;
        ballCenterOffset = new Vector3(0, halfH, 0);

        for (int i = 0; i < cantidadPuntos; i++)
        {
            GameObject punto = Instantiate(puntoPrefab);
            punto.SetActive(false);
            puntos.Add(punto);
        }

        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = -20;

        posicionOriginal = transform.position;
        lanzamientosRestantes = vidasLanzamiento;
    }

    void Update()
    {
        if (!entradaHabilitada || lanzado || lanzamientosRestantes <= 0)
            return;


        if (!lanzado)
        {
            if (Input.GetMouseButtonDown(0))
            {
                puntoInicio = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 puntoFin = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direccion = puntoInicio - puntoFin;
                float v0 = direccion.magnitude * multiplicadorFuerza;
                float angulo = Mathf.Atan2(direccion.y, direccion.x);

                velocidad = new Vector2(
                    v0 * Mathf.Cos(angulo),
                    v0 * Mathf.Sin(angulo)
                );

                velocidad.x = Mathf.Sign(velocidad.x) * Mathf.Max(Mathf.Abs(velocidad.x), minVelComponentX);
                velocidad.y = Mathf.Sign(velocidad.y) * Mathf.Max(Mathf.Abs(velocidad.y), minVelComponentY);

                if (velocidad.y <= 0f)
                {
                    // No dispares
                    return;
                }

                MostrarTrayectoria(transform.position, velocidad);
            }

            if (Input.GetMouseButtonUp(0))
            {
                sr.sortingOrder = 3;
                lanzado = true;
                OcultarTrayectoria();

                lanzamientosRestantes--;
                Debug.Log("Lanzamientos restantes: " + lanzamientosRestantes);
                ActualizarVidasUI(); // <-- Llama a la función para actualizar los íconos
            }

        }
    }

    void FixedUpdate()
    {
        if (!lanzado) return;

        Vector2 velocidadAnterior = velocidad;
        float ax = -(c / m) * velocidad.x;
        float ay = -g - (c / m) * velocidad.y;

        velocidad.x += ax * dt;
        velocidad.y += ay * dt;

        Vector2 desplazamiento = dt * 0.5f * (velocidadAnterior + velocidad);
        transform.position += new Vector3(desplazamiento.x, desplazamiento.y, 0);

        // Verificar colisión con el suelo (Tilemap)
        Vector2 puntoCentro = transform.position;
        float radio = 0.1f;

        Collider2D colision = Physics2D.OverlapCircle(puntoCentro, radio, capaSuelo);

        if (colision != null)
        {

            // Siempre vuelve al inicio
            transform.position = posicionOriginal;
            velocidad = Vector2.zero;
            lanzado = false;
            sr.sortingOrder = -20;

            if (lanzamientosRestantes <= 0)
            {
                entradaHabilitada = false;
                Debug.Log("Se acabaron los lanzamientos.");

                if (pantallaFinalManager != null)
                {
                    bool gano = GameObject.FindObjectOfType<ColisionBarrilesConRebote>().TodosBarrilesDerribados();

                    if (gano)
                    {
                        pantallaFinalManager.MostrarGanaste();
                    }
                   
                }

            }

            return;

        }


            if (pantallaFinalManager != null &&
                GameObject.FindObjectOfType<ColisionBarrilesConRebote>().TodosBarrilesDerribados())
            {
                entradaHabilitada = false; // Ya no permitir más disparos
                pantallaFinalManager.MostrarGanaste();
            }
        



    }
    void MostrarTrayectoria(Vector3 posicionInicial, Vector2 velocidadInicial)
    {
        Vector3 pos = posicionInicial + ballCenterOffset;
        Vector2 v = velocidadInicial;

        for (int i = 0; i < cantidadPuntos; i++)
        {
            float ax = -(c / m) * v.x;
            float ay = -g - (c / m) * v.y;

            v.x += ax * intervaloTiempo;
            v.y += ay * intervaloTiempo;

            Vector2 desplazamiento = intervaloTiempo * 0.5f * v;
            pos += new Vector3(desplazamiento.x, desplazamiento.y, 0);

            puntos[i].transform.position = pos;
            puntos[i].SetActive(true);

            // escalamos para que decrezcan desde startDotScale hasta endDotScale
            float t = (float)i / (cantidadPuntos - 1);
            float s = Mathf.Lerp(startDotScale, endDotScale, t);
            puntos[i].transform.localScale = Vector3.one * s;
        }
    }

    void OcultarTrayectoria()
    {
        foreach (GameObject punto in puntos)
        {
            punto.SetActive(false);
        }
    }
    public void ActualizarVidasUI()
    {
        if (iconosVidas == null || iconosVidas.Count == 0)
            return;

        // Desactiva íconos desde el final
        int index = vidasLanzamiento - lanzamientosRestantes - 1;

        if (index >= 0 && index < iconosVidas.Count)
        {
            iconosVidas[index].SetActive(false);
        }
    }


}
