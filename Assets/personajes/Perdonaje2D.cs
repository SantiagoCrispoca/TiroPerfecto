using UnityEngine;

public class Personaje2D : MonoBehaviour
{
    private Animator animator;
    private TiroParabolico tiroParabolico;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Este objeto necesita un componente Animator.");
        }

        // Busca el script TiroParabolico en la escena
        tiroParabolico = FindObjectOfType<TiroParabolico>();

        if (tiroParabolico == null)
        {
            Debug.LogError("No se encontró un objeto con el script TiroParabolico.");
        }
    }

    void Update()
    {
        if (tiroParabolico != null)
        {
            animator.SetBool("isTirando", tiroParabolico.estaApuntandoValido);
        }
    }
}
