using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager>
{
    [Header("SkillManager")]
    [SerializeField] private Transform VFXSpawnPoint;
    [SerializeField] private GameObject skillVFXPrefab;

    public List<BaseSkillData> basicSkillList;
    public List<BaseSkillData> upgradedSkillList;
    public List<BaseSkillData> ultimateSkillList;
}
