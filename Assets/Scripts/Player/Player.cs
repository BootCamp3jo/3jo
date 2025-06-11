using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : APlayer
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        /*float horizontal = Input.GetAxis("Horizontal"); // A/D or ←/→
        float vertical = Input.GetAxis("Vertical");     // W/S or ↑/↓

        Vector3 move = new Vector3(horizontal, vertical);

        // 프레임별로 이동량 계산
        transform.position += move * 5f * Time.deltaTime;*/
    }
}
