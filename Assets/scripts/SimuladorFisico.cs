using UnityEngine;
using System.Collections.Generic;

public class SimuladorFisico : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject prefabProyectil;
    public GameObject prefabCaja;
    public int numCajas = 5;
    public float gravedad = 9.8f;

    private GameObject proyectil;
    private List<GameObject> cajas = new List<GameObject>();
    private Vector2 velocidadProyectil;
    private bool esperandoInput = true;
    private string inputBuffer = "";
    private Vector2 posicionInicial;

    void Start()
    {
        InicializarEscena();
        Debug.Log("Ingrese ángulo (grados) y fuerza (ej: '45 10'):");
    }

    void InicializarEscena()
    {
        proyectil = Instantiate(prefabProyectil);
        posicionInicial = new Vector2(-8.3f, 0f);
        proyectil.transform.position = posicionInicial;

        for (int i = 0; i < numCajas; i++)
        {
            GameObject caja = Instantiate(prefabCaja);
            caja.transform.position = new Vector2(4.5f, i-0.8f * 1.2f);
            cajas.Add(caja);
        }
    }

    void Update()
    {
        ManejarEntrada();
        ActualizarFisica();
    }

    void ManejarEntrada()
    {
        if (!esperandoInput) return;

        foreach (char c in Input.inputString)
        {
            if (c == '\r' || c == '\n')
            {
                ProcesarInput();
            }
            else
            {
                inputBuffer += c;
            }
        }
    }

    void ProcesarInput()
    {
        string[] valores = inputBuffer.Trim().Split(' ');
        inputBuffer = "";

        if (valores.Length == 2
            && float.TryParse(valores[0], out float angulo)
            && float.TryParse(valores[1], out float fuerza))
        {
            Debug.Log($"Recibido ángulo={angulo}, fuerza={fuerza}");
            velocidadProyectil = new Vector2(
                Mathf.Cos(angulo * Mathf.Deg2Rad) * fuerza,
                Mathf.Sin(angulo * Mathf.Deg2Rad) * fuerza
            );
            esperandoInput = false;
        }
        else
        {
            Debug.Log("Entrada inválida. Use formato: '45 10'");
        }
    }

    void ActualizarFisica()
    {
        if (esperandoInput) return;

        proyectil.transform.position += (Vector3)velocidadProyectil * Time.deltaTime;
        velocidadProyectil.y -= gravedad * Time.deltaTime;

        foreach (GameObject caja in cajas)
        {
            if (caja.activeSelf && DetectarColision(proyectil.transform.position, caja.transform.position))
            {
                Vector2 normal = ((Vector2)proyectil.transform.position - (Vector2)caja.transform.position).normalized;
                velocidadProyectil = Vector2.Reflect(velocidadProyectil, normal);
                StartCoroutine(MoverCaja(caja));
            }
        }

        if (proyectil.transform.position.y < -10)
        {
            ReiniciarSimulacion();
        }
    }

    bool DetectarColision(Vector2 pos1, Vector2 pos2)
    {
        float radio = proyectil.transform.localScale.x / 2f;
        float tamanoCaja = prefabCaja.transform.localScale.x / 2f;
        return Vector2.Distance(pos1, pos2) < (radio + tamanoCaja);
    }

    System.Collections.IEnumerator MoverCaja(GameObject caja)
    {
        Vector2 velocidadCaja = velocidadProyectil * 0.3f;
        while (caja.activeSelf)
        {
            velocidadCaja.y -= gravedad * Time.deltaTime;
            caja.transform.position += (Vector3)velocidadCaja * Time.deltaTime;

            if (caja.transform.position.y < -10)
                caja.SetActive(false);

            yield return null;
        }
    }

    void ReiniciarSimulacion()
    {
        proyectil.transform.position = posicionInicial;
        velocidadProyectil = Vector2.zero;

        foreach (GameObject caja in cajas)
        {
            caja.SetActive(true);
            caja.transform.position = new Vector2(5, cajas.IndexOf(caja) * 1.2f);
        }

        esperandoInput = true;
        Debug.Log("Ingrese nuevos valores (ángulo fuerza):");
    }
}
