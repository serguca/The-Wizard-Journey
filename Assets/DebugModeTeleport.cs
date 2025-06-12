using UnityEngine;

public class DebugModeTeleport : MonoBehaviour
{
    private Vector3 room1 = new Vector3(0, 1, 0);
    [SerializeField] private Transform room2;
    [SerializeField] private Transform room3;
    [SerializeField] private Transform room4;
    [SerializeField] private Transform room5;
    [SerializeField] private Transform room6;
    [SerializeField] private Transform room7;
    [SerializeField] private Transform room8;

    private void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = room1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.position = room2.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.position = room3.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.position = room4.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            transform.position = room5.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            transform.position = room6.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            transform.position = room7.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            transform.position = room8.position;
        }
    }
}
