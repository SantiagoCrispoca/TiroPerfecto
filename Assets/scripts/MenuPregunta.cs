using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPregunta : MonoBehaviour
{
    [SerializeField] private GameObject botonPregunta;
    [SerializeField] private GameObject menuPregunta;
    public void Pausa()
    {
        TiroParabolico tiro = FindObjectOfType<TiroParabolico>();
        if (tiro != null)
        {
            tiro.entradaHabilitada = false; // Deshabilita el disparo al pausar
        }

        Time.timeScale = 0f;
        botonPregunta.SetActive(false);
        menuPregunta.SetActive(true);
    }
    public void Reanudar()
    {
        Time.timeScale = 1f;
        botonPregunta.SetActive(true);
        menuPregunta.SetActive(false);
        StartCoroutine(ReanudarDespuesDeRetardo());

    }

    private IEnumerator ReanudarDespuesDeRetardo()
    {
        Time.timeScale = 1f;
        botonPregunta.SetActive(true);
        menuPregunta.SetActive(false);

        TiroParabolico tiro = FindObjectOfType<TiroParabolico>();
        if (tiro != null)
        {
            tiro.entradaHabilitada = false;
            yield return new WaitForSeconds(0.3f); // espera antes de reactivar disparo
            tiro.entradaHabilitada = true;
        }
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Cerrar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("cuartamapa");
    }


}
