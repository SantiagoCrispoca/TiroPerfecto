using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Virtual Cameras")]
    public CinemachineVirtualCamera vcamBarriles;      // Cámara para ver los barriles
    public CinemachineVirtualCamera vcamSeguimiento;   // Cámara para seguir la bola

    [Header("Tiempos")]
    [Tooltip("Segundos que tarda en paneo de barriles → bola")]
    public float delayAntesDePaneo = 2f;

    public GameObject bola; // asigna tu objeto bola en el Inspector

    void Start()
    {

        // Asegura que al inicio veamos los barriles
        vcamBarriles.Priority = 20;
        vcamSeguimiento.Priority = 10;

        bola.GetComponent<TiroParabolico>().entradaHabilitada = false;

        StartCoroutine(PaneoInicial());
    }

    IEnumerator PaneoInicial()
    {
        // 1) Espera un momento viendo los barriles
        yield return new WaitForSeconds(delayAntesDePaneo);

        // 2) Dispara el blending: baja priority de VCam_Barriles, sube de VCam_Seguimiento
        vcamBarriles.Priority = 9;
        vcamSeguimiento.Priority = 21;

        // 3) (Opcional) Si quieres desactivar la cámara de barriles tras el blend
        yield return new WaitForSeconds(Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
        vcamBarriles.gameObject.SetActive(false);
        bola.GetComponent<TiroParabolico>().entradaHabilitada = true;
    }
}
