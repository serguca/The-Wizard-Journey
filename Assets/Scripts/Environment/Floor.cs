using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<Character>()?.TakeDamage(99999999f); //Insta kill
    }
}
