using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private SpellManager spellManager;
    [SerializeField] private Transform shootPoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spellManager.LaunchProjectile(shootPoint.position, shootPoint.forward);
        }
    }
}