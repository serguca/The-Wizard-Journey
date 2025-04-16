using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public HechizoManager hechizoManager;
    public Transform shootPoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hechizoManager.LanzarProyectil(shootPoint.position, shootPoint.forward);
        }
    }
}