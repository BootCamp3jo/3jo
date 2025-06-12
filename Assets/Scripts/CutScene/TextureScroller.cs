using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureScroller : MonoBehaviour
{
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.0f;

    private Image uiImage;
    private Material uiMaterial;
    private Vector2 currentOffset;

    void Start()
    {
        uiImage = GetComponent<Image>();
        if (uiImage != null && uiImage.material != null)
        {
            uiMaterial = uiImage.material;
            currentOffset = uiMaterial.mainTextureOffset;
        }
        else
        {
            enabled = false;
        }
    }

    void Update()
    {
        if (uiMaterial != null)
        {
            currentOffset.x += scrollSpeedX * Time.deltaTime;
            currentOffset.y += scrollSpeedY * Time.deltaTime;
            uiMaterial.mainTextureOffset = currentOffset;
        }
    }

    void OnDisable()
    {
        if (uiMaterial != null)
        {
            uiMaterial.mainTextureOffset = Vector2.zero;
            uiMaterial = null;
        }
    }
}
