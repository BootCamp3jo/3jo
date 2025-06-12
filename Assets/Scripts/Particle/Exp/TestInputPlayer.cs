using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputPlayer : MonoBehaviour
{
   [SerializeField] private  ExpManager expManager;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            expManager.SpawnXP(this.transform.position,1);
        }
    }
}
