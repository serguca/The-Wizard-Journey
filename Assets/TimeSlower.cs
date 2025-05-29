using UnityEngine;

public class TimeSlower : MonoBehaviour
{
    [SerializeField] private float slowDownFactor = 1; // Factor de ralentización

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = slowDownFactor; // Establece el factor de ralentización
    }
}
