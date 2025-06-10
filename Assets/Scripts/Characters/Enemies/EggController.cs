using UnityEngine;

public class EggController : MonoBehaviour
{
    [SerializeField] GameObject _full;
    [SerializeField] GameObject _damaged;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject spiderEnemy;
    [SerializeField] private AudioClip eggDestroySound;
    public void Start()
    {
        if (spiderEnemy != null)
            spiderEnemy.SetActive(false);
    }

    public void Clear()
    {
        _full.gameObject.SetActive(true);
        _damaged.gameObject.SetActive(false);
    }
    public void Destroy()
    {
        if (eggDestroySound != null)
        {
            SoundManager.Instance.PlaySound(eggDestroySound, transform.position);
        }
        _full.gameObject.SetActive(false);
        _damaged.gameObject.SetActive(true);
        _particleSystem.Play();
        spiderEnemy.SetActive(true);
    }

}