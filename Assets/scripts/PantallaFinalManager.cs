using UnityEngine;

public class PantallaFinalManager : MonoBehaviour
{
    public GameObject pantallaGanaste;
    public GameObject pantallaPerdiste;

    public void MostrarGanaste()
    {
        pantallaGanaste.SetActive(true);
    }

    public void MostrarPerdiste()
    {
        pantallaPerdiste.SetActive(true);
    }
}
