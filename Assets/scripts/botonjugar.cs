using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class botonjugar : MonoBehaviour
{
    public int numeroEscena;
    // Start is called before the first frame update
    public void cambiarEscena()
    {
        SceneManager.LoadScene(numeroEscena);
    }

    // Update is called once per frame
    public void Exit()
    {
        Debug.Log("Aplicacion cerrada");
       
    }
}
