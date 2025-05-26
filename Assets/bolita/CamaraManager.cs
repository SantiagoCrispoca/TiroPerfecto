using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Virtual Cameras")]
    public CinemachineVirtualCamera vcamBarriles;     
    public CinemachineVirtualCamera vcamSeguimiento;   

    [Header("Tiempos")]
    [Tooltip("Segundos que tarda en paneo de barriles → bola")]
    public float delayAntesDePaneo = 2f;

    public GameObject bola; 
    void Start()
    {

      
        vcamBarriles.Priority = 20;
        vcamSeguimiento.Priority = 10;

        bola.GetComponent<TiroParabolico>().entradaHabilitada = false;

        StartCoroutine(PaneoInicial());
    }

    IEnumerator PaneoInicial()
    {
       
        yield return new WaitForSeconds(delayAntesDePaneo);

       
        vcamBarriles.Priority = 9;
        vcamSeguimiento.Priority = 21;

        
        yield return new WaitForSeconds(Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
        vcamBarriles.gameObject.SetActive(false);
        bola.GetComponent<TiroParabolico>().entradaHabilitada = true;
    }
}
