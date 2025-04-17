using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void Activar(float duracion)
    {
        StartCoroutine(DesactivarDespuesDeTiempo(duracion));
    }

    private IEnumerator DesactivarDespuesDeTiempo(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        gameObject.SetActive(false);
    }
}