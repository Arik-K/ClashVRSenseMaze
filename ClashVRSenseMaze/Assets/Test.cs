using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject cube;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleObjects();
        }
    }

    private void ToggleObjects()
    {
        startPoint.SetActive(!startPoint.activeSelf);
        cube.SetActive(!cube.activeSelf);
    }
}


