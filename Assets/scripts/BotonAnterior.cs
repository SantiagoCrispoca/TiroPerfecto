using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonAnterior : MonoBehaviour
{
    public void Cerrar()
    {
        SceneManager.LoadScene("tercerapersonaje");
    }
    public void Cerrar1()
    {
        SceneManager.LoadScene("segundapagina");
    }
    public void Cerrar2()
    {
        SceneManager.LoadScene("Inicio");
    }
}
