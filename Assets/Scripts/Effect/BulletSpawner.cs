using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] spawnPoints;

    public float fireInterval = 1f; // 발사 간격 (초)

    private void Start()
    {
        StartCoroutine(AutoFire());
    }

    private IEnumerator AutoFire()
    {
        while (true)
        {
            SpawnRandomBullet();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    public void SpawnRandomBullet()
    {
        if (spawnPoints.Length == 0) return;

        int index = Random.Range(0, spawnPoints.Length);
        GameObject bullet = Instantiate(bulletPrefab, spawnPoints[index].position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        rb.velocity = randomDir * bullet.GetComponent<BulletController>().normalSpeed;
    }
}
