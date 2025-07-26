using UnityEngine;

public class ColisionBarrilesConRebote : MonoBehaviour
{
    public GameObject[] barriles;         
    public float radioColision = 0.9f;   
    public float fuerzaCaida = 8f;      

    private bool[] barrilDerribado;
    private TiroParabolico tiroScript;

    void Start()
    {
        barrilDerribado = new bool[barriles.Length];
        tiroScript = GetComponent<TiroParabolico>();
    }

    void Update()
    {
        
        if (tiroScript != null && tiroScript.velocidad != Vector2.zero)
        {
            for (int i = 0; i < barriles.Length; i++)
            {
                if (barrilDerribado[i]) continue;

                float distancia = Vector2.Distance(transform.position, barriles[i].transform.position);
                if (distancia < radioColision)
                {
                    Vector2 lineaImpacto = (barriles[i].transform.position - transform.position).normalized;
                    Vector2 velocidad = tiroScript.velocidad;

                    Vector2 velocidadNormal = Vector2.Dot(velocidad, lineaImpacto) * lineaImpacto;
                    Vector2 velocidadTangente = velocidad - velocidadNormal;

                    // Rebote: invierte la parte normal
                    Vector2 nuevaVelocidad = velocidadTangente - velocidadNormal;

                    tiroScript.velocidad = nuevaVelocidad;



                    barrilDerribado[i] = true;

                    StartCoroutine(DesactivarEntradaPorDelay(1.5f)); // 1.5 segundos de pausa

                }
            }
        }

       
        for (int i = 0; i < barriles.Length; i++)
        {
            if (barrilDerribado[i])
            {
                barriles[i].transform.position += Vector3.down * fuerzaCaida * Time.deltaTime;
            }
        }
    }

    private System.Collections.IEnumerator DesactivarEntradaPorDelay(float segundos)
    {
        if (tiroScript != null)
        {
            tiroScript.bloqueoReinicioTemporal = true; // Bloqueo temporal
            tiroScript.entradaHabilitada = false;

            yield return new WaitForSeconds(segundos);

            tiroScript.bloqueoReinicioTemporal = false; // Fin del bloqueo
            tiroScript.entradaHabilitada = true;
        }
    }


    public bool TodosBarrilesCaidos()
    {
        foreach (bool estado in barrilDerribado)
        {
            if (!estado) return false;
        }
        return true;
    }


}
