using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanel : MonoBehaviour
{
    public Image image;
    public void Init(Sprite image)
    {
        this.image.sprite = image;
    }
}
