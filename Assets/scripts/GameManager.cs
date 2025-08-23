using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Personajes> personajes;

    public int puntos = 0;
    public int totalBarriles = 0;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔹 Reiniciar puntos al comenzar cada nivel
    public void ReiniciarPuntos()
    {
        puntos = 0;
    }

    // 🔹 Sumar puntos (cuando cae un barril)
    public void SumarPunto()
    {
        puntos++;
    }
}
