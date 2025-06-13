using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSortingOrder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isUpdateOrder = false;
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
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
}
