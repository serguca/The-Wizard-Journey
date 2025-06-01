using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Components")]
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Opening door");
            Debug.Log("Posición antes: " + transform.position);

            if (animator != null)
            {
                animator.SetBool("IsOpen", true);
                Debug.Log("Bool 'Open' activado");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Posición actual: " + transform.position);
        }
    }
}
