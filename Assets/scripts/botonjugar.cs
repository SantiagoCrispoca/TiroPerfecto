using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class botonjugar : MonoBehaviour
{
    public int numeroEscena;
  
    public void cambiarEscena()
    {
        SceneManager.LoadScene(numeroEscena);
    }


    public void Exit()
    {
        Debug.Log("Aplicacion cerrada");
        Application.Quit();
    }
}
