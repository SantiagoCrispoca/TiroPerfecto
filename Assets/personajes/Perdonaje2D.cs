using UnityEngine;

public class Personaje2D : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Obtener el Animator
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Este objeto necesita un componente Animator.");
        }
    }

    void Update()
    {
        // Verifica si el botón izquierdo del mouse está presionado
        if (Input.GetMouseButton(0)) // 0 = botón izquierdo
        {
            animator.SetBool("isTirando", true);
        }
        else
        {
            animator.SetBool("isTirando", false);
        }
    }
}
