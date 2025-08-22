using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("Virtual Cameras")]
    public CinemachineVirtualCamera vcamBarriles;
    public CinemachineVirtualCamera vcamSeguimiento;
    public CinemachineVirtualCamera vcamMapaGeneral;

    [Header("UI")]
    public Button botonMiniMapa; // Botón que se ve abajo a la izquierda

    [Header("Tiempos")]
    public float delayAntesDePaneo = 2f;

    public GameObject bola;

    private bool mapaExpandido = false;

    void Start()
    {
        vcamBarriles.Priority = 20;
        vcamSeguimiento.Priority = 10;
        vcamMapaGeneral.Priority = 0;

        bola.GetComponent<TiroParabolico>().entradaHabilitada = false;

        if (botonMiniMapa != null)
        {
            botonMiniMapa.onClick.AddListener(ToggleMapa);
        }

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

    void ToggleMapa()
    {
        mapaExpandido = !mapaExpandido;

        if (mapaExpandido)
        {
            vcamMapaGeneral.Priority = 100;
        }
        else
        {
            vcamMapaGeneral.Priority = 0;
        }
    }
}
