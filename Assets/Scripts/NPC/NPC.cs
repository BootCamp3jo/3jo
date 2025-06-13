using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : ANPC
{
    public CutSceneData cutSceneData;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        //Interaction();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Interaction()
    {
        CutSceneManager.Instance.StartCutScene(this.cutSceneData);
    }

}
