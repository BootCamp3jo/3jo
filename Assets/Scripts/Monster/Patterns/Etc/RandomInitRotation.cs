using UnityEngine;

public class RandomInitRotation : MonoBehaviour
{
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }
}
