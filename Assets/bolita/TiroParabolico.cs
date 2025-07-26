using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class TiroParabolico : MonoBehaviour
{
    public GameObject puntoPrefab;

    public LayerMask capaSuelo; 

    public float minVelComponentX = 7f;
    public float minVelComponentY = 1f;
    public float startDotScale = 0.15f;   
    public float endDotScale = 0.08f;   

    public int cantidadPuntos = 6;
    public float intervaloTiempo = 0.1f;
    public float c = 1.5f;
    public float m = 3.8f;
    public float g = 9.8f;
    public float multiplicadorFuerza = 6f;

    public bool entradaHabilitada = true;  

    private List<GameObject> puntos = new List<GameObject>();
    private Vector3 puntoInicio;
    private Camera cam;
    private bool lanzado = false;
    public Vector2 velocidad;
    private float dt;

    private Vector3 ballCenterOffset;

    private SpriteRenderer sr;

    private Vector3 posicionOriginal;
    public int vidasLanzamiento = 6; 

    
    private int lanzamientosRestantes;
    public float delayAntesDePaneo = 2f;

    public List<GameObject> iconosVidas; 

    public PantallaFinalManager pantallaFinalManager;

    private bool intentoValido = false;

    public bool estaApuntandoValido = false;

    [HideInInspector]
    public bool bloqueoReinicioTemporal = false;

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

                // Solo permitimos si empieza a la izquierda del personaje
                intentoValido = puntoInicio.x <= transform.position.x;
            }


            if (Input.GetMouseButton(0))
            {
                if (!intentoValido)
                {
                    OcultarTrayectoria();
                    estaApuntandoValido = false;
                    return;
                }

                Vector3 puntoFin = cam.ScreenToWorldPoint(Input.mousePosition);

                // Cancela si el arrastre va hacia la derecha del personaje
                if (puntoFin.x > transform.position.x)
                {
                    OcultarTrayectoria();
                    estaApuntandoValido = false;
                    return;
                }

                Vector2 direccion = puntoInicio - puntoFin;
                float v0 = direccion.magnitude * multiplicadorFuerza;
                float angulo = Mathf.Atan2(direccion.y, direccion.x);

                velocidad = new Vector2(
                    v0 * Mathf.Cos(angulo),
                    v0 * Mathf.Sin(angulo)
                );

                // Asegurar componentes mínimos
                velocidad.x = Mathf.Sign(velocidad.x) * Mathf.Max(Mathf.Abs(velocidad.x), minVelComponentX);
                velocidad.y = Mathf.Sign(velocidad.y) * Mathf.Max(Mathf.Abs(velocidad.y), minVelComponentY);

                if (velocidad.y <= 0f)
                {
                    OcultarTrayectoria();
                    estaApuntandoValido = false;
                    return;
                }

                MostrarTrayectoria(transform.position, velocidad);
                estaApuntandoValido = true;
            }
            else
            {
                estaApuntandoValido = false;
            }


            if (Input.GetMouseButtonUp(0) && intentoValido)
            {
                sr.sortingOrder = 3;
                lanzado = true;
                OcultarTrayectoria();

                lanzamientosRestantes--;
                Debug.Log("Lanzamientos restantes: " + lanzamientosRestantes);
                ActualizarVidasUI();
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

       
        Vector2 puntoCentro = transform.position;
        float radio = 0.1f;

        Collider2D colision = Physics2D.OverlapCircle(puntoCentro, radio, capaSuelo);

        if (colision != null)
        {
            if (bloqueoReinicioTemporal)
            {
                // Si estamos esperando después de golpear un barril, no hacemos nada
                return;
            }

            ReiniciarPelota(true);

            bool todosCaidos = GameObject.FindObjectOfType<ColisionBarrilesConRebote>().TodosBarrilesCaidos();

            if (todosCaidos)
            {
                entradaHabilitada = false;
                if (pantallaFinalManager != null)
                {
                    pantallaFinalManager.MostrarGanaste();
                }
            }
            else if (lanzamientosRestantes <= 0)
            {
                entradaHabilitada = false;
                if (pantallaFinalManager != null)
                {
                    pantallaFinalManager.MostrarPerdiste();
                }
            }

            return;
        }

        // Limites del mapa
        if (transform.position.y < -6f || transform.position.y > 30f ||
            transform.position.x < -30f || transform.position.x > 80f)
        {
            Debug.LogWarning("¡Pelota fuera de límites! Reiniciando...");

            ReiniciarPelota(true);
            return;
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

        
        int index = vidasLanzamiento - lanzamientosRestantes - 1;

        if (index >= 0 && index < iconosVidas.Count)
        {
            iconosVidas[index].SetActive(false);
        }
    }

    void ReiniciarPelota(bool revisarFinDelJuego)
    {
        transform.position = posicionOriginal;
        velocidad = Vector2.zero;
        lanzado = false;
        sr.sortingOrder = -20;

        if (revisarFinDelJuego)
        {
            bool todosCaidos = GameObject.FindObjectOfType<ColisionBarrilesConRebote>().TodosBarrilesCaidos();

            if (todosCaidos)
            {
                entradaHabilitada = false;
                if (pantallaFinalManager != null)
                    pantallaFinalManager.MostrarGanaste();
            }
            else if (lanzamientosRestantes <= 0)
            {
                entradaHabilitada = false;
                if (pantallaFinalManager != null)
                    pantallaFinalManager.MostrarPerdiste();
            }
        }
    }



}
