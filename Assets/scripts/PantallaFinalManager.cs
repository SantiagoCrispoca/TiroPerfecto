using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PantallaFinalManager : MonoBehaviour
{
    public GameObject panelGanaste;
    public GameObject panelPerdiste;

    [Header("Textos de Puntos")]
    public TextMeshProUGUI textoPuntosGanador; 
    public TextMeshProUGUI textoPuntosPerdedor;

    private string[] niveles = { "MapaUnoBolita", "Mapados", "Mapatres" };

    public void MostrarGanaste()
    {
        panelGanaste.SetActive(true);
        textoPuntosGanador.text = "Puntos: " + GameManager.Instance.puntos + " / 6"; 
        Invoke("PasarAlSiguienteNivel", 2f);
    }

    public void MostrarPerdiste()
    {
        panelPerdiste.SetActive(true);
        textoPuntosPerdedor.text = "Puntos: " + GameManager.Instance.puntos + " / 6"; 
    }

    private void PasarAlSiguienteNivel()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        int indice = System.Array.IndexOf(niveles, escenaActual);

        if (indice >= 0 && indice < niveles.Length - 1)
        {
            string siguienteNivel = niveles[indice + 1];
            SceneManager.LoadScene(siguienteNivel);
        }
        else
        {
            Debug.Log("🎉 Juego completado 🎉");
        }
    }

    public void ReiniciarNivel()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(escenaActual);
    }

    public void SalirAlMenu()
    {
        SceneManager.LoadScene("Inicio");
    }
}
