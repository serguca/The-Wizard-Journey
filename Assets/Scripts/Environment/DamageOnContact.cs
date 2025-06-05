using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField] private float damage = 0; // Damage to apply on contact
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<Character>()?.TakeDamage(damage); //Insta kill
    }
}



