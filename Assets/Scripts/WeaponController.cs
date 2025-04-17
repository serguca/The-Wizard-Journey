using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private HechizoManager hechizoManager;
    [SerializeField] private Transform shootPoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hechizoManager.LanzarProyectil(shootPoint.position, shootPoint.forward);
        }
    }
}