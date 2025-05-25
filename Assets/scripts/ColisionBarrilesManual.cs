using UnityEngine;

public class ColisionBarrilesConRebote : MonoBehaviour
{
    public GameObject[] barriles;         // Asigna en Inspector
    public float radioColision = 0.5f;    // Distancia para detectar colisión
    public float fuerzaCaida = 8f;        // Velocidad de caída de barril

    private bool[] barrilDerribado;
    private TiroParabolico tiroScript;

    void Start()
    {
        barrilDerribado = new bool[barriles.Length];
        tiroScript = GetComponent<TiroParabolico>();
    }

    void Update()
    {
        // Si la bolita aún está en movimiento, detectar colisión
        if (tiroScript != null && tiroScript.velocidad != Vector2.zero)
        {
            for (int i = 0; i < barriles.Length; i++)
            {
                if (barrilDerribado[i]) continue;

                float distancia = Vector2.Distance(transform.position, barriles[i].transform.position);
                if (distancia < radioColision)
                {
                    // Rebote
                    Vector2 direccion = (transform.position - barriles[i].transform.position).normalized;

                    if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
                    {
                        tiroScript.velocidad.x *= -1; // Rebote en X
                    }
                    else
                    {
                        tiroScript.velocidad.y *= -1; // Rebote en Y
                    }

                    // Marcar barril como derribado
                    barrilDerribado[i] = true;
                }
            }
        }

        // Esta parte SIEMPRE se ejecuta: hace que los barriles caigan si fueron tocados
        for (int i = 0; i < barriles.Length; i++)
        {
            if (barrilDerribado[i])
            {
                barriles[i].transform.position += Vector3.down * fuerzaCaida * Time.deltaTime;
            }
        }
    }
    public bool TodosBarrilesDerribados()
    {
        foreach (bool derribado in barrilDerribado)
        {
            if (!derribado)
                return false;
        }
        return true;
    }

}
