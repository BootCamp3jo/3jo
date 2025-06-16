using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSortingOrder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isUpdateOrder = false;
    [SerializeField] private float pivotYOffset = 0f;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUpdateOrder)
            return;

        float adjustedY = transform.position.y + pivotYOffset;
        spriteRenderer.sortingOrder = -(int)(adjustedY * 100);
    }
}
