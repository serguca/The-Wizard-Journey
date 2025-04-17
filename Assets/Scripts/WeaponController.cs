using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private SpellManager hechizoManager;
    [SerializeField] private Transform shootPoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hechizoManager.LaunchProjectile(shootPoint.position, shootPoint.forward);
        }
    }
}