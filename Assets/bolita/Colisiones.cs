using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Colisiones : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject bola;
    public GameObject barrilesParent;

    [Header("Parámetros de Choque")]
    public float bolaMass = 1f;
    public float barrelMass = 2f;

    private float bolaRadio;
    private Dictionary<Transform, float> barrelRadio;
    private Dictionary<Transform, Vector2> barrelVel;
    private float dt;

    void Start()
    {
        dt = Time.fixedDeltaTime;
        bolaRadio = bola.transform.localScale.x * 0.5f;

        barrelRadio = new Dictionary<Transform, float>();
        barrelVel = new Dictionary<Transform, Vector2>();

        foreach (Transform b in barrilesParent.transform)
        {
            barrelRadio[b] = b.localScale.x * 0.5f;
            barrelVel[b] = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        Vector2 posBola = bola.transform.position;
        var v = bola.GetComponent<TiroParabolico>().velocidad; // haz pública la velocidad

        // Choque bola ? barriles
        foreach (var kv in barrelVel.ToList())
        {
            var barril = kv.Key;
            var velBar = barrelVel[barril];
            var posBar = (Vector2)barril.position;
            float sumR = bolaRadio + barrelRadio[barril];

            if (Vector2.Distance(posBola, posBar) <= sumR)
            {
                Vector2 n = (posBar - posBola).normalized;
                float v1n = Vector2.Dot(v, n);
                float v2n = Vector2.Dot(velBar, n);
                Vector2 v1t = v - v1n * n;
                Vector2 v2t = velBar - v2n * n;

                float v1nN = (v1n * (bolaMass - barrelMass) + 2 * barrelMass * v2n) / (bolaMass + barrelMass);
                float v2nN = (v2n * (barrelMass - bolaMass) + 2 * bolaMass * v1n) / (bolaMass + barrelMass);

                // aplica nuevas velocidades
                v = v1t + v1nN * n;
                velBar = v2t + v2nN * n;

                // guarda
                barrelVel[barril] = velBar;
            }

            // mueve el barril
            barril.position += (Vector3)(barrelVel[barril] * dt);
        }

        // actualiza la velocidad de la bola en su script
        var tp = bola.GetComponent<TiroParabolico>();
        tp.velocidad = v;
        bola.transform.position += (Vector3)(v * dt);
    }
}
