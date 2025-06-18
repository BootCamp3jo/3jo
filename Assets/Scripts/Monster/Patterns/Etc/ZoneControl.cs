using UnityEngine;

public class ZoneControl : MonoBehaviour
{
    public float scaleUpSpeed;
    public Transform AttackEffects;

    private void Start()
    {
        transform.position = GameManager.Instance.boss.transform.position-Vector3.up;
    }
    void Update()
    {
        transform.localScale += Vector3.one * scaleUpSpeed * Time.deltaTime;
    }

    public void ZoneOver()
    {
        AttackEffects.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
