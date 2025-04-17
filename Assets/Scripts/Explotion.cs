// Explosion.cs
using System.Collections;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    public void Activate(float duration)
    {
        StartCoroutine(DeactivateAfterTime(duration));
    }

    private IEnumerator DeactivateAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}