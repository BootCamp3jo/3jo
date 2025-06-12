using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    [field: SerializeField] public MonsterData monsterData;

    MonsterAnimatorParameters monsterAnimatorParameters = new();
}
