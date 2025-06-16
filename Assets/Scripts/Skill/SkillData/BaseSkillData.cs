using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillData : ScriptableObject
{
    [Header("Skill Information")]
    public string skillName;
    public string description;
    public Sprite icon;
    public GameObject skillPrefab;
}
