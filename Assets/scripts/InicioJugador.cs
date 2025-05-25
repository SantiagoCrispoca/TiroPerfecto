using UnityEngine;

public class InicioJugador : MonoBehaviour
{
    private void Start()
    {
        int indexJugador = PlayerPrefs.GetInt("JugadorIndex");
        Debug.Log("Index del jugador: " + indexJugador);

        var personajeData = GameManager.Instance.personajes[indexJugador];

        if (personajeData.personajeJugable != null)
        {
            Debug.Log("Instanciando personaje: " + personajeData.nombre);
            Instantiate(personajeData.personajeJugable, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("El prefab del personaje no está asignado.");
        }

    }
}
