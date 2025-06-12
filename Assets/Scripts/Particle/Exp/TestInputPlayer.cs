using UnityEngine;

public class TestInputPlayer : MonoBehaviour
{
    [SerializeField] private ExpManager expManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            expManager.SpawnExp(this.transform.position, 1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.transform.position);
            Vector2 center = new Vector2(viewportPos.x, viewportPos.y);

            ShockWaveFeature.Instance.SetParameters(
                speed: 0.5f,
                width: 0.1f,
                duration: 5f
            );
            ShockWaveFeature.Instance.SetCenter(center);
            ShockWaveFeature.Instance.TriggerShockWave();
        }
    }
}
