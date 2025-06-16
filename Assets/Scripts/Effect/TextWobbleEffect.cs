using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextWobbleEffect : MonoBehaviour
{
    private TMP_Text tmpText;
    private Mesh mesh;
    private Vector3[] vertices;

    [SerializeField] private float wobbleSpeed = 4f;
    [SerializeField] private float wobbleHeight = 5f;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        tmpText.ForceMeshUpdate(); // 텍스트 초기화
    }

    void Update()
    {
        tmpText.ForceMeshUpdate(); // 매프레임 텍스트 메쉬 새로고침

        mesh = tmpText.mesh;
        vertices = mesh.vertices;

        int characterCount = tmpText.textInfo.characterCount;

        for (int i = 0; i < characterCount; i++)
        {
            if (!tmpText.textInfo.characterInfo[i].isVisible)
                continue;

            int vertexIndex = tmpText.textInfo.characterInfo[i].vertexIndex;

            Vector3 offset = WobbleOffset(i, Time.time);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        mesh.vertices = vertices;
        tmpText.canvasRenderer.SetMesh(mesh);
    }

    Vector2 WobbleOffset(int index, float time)
    {
        // 홀수/짝수로 오프셋 방향 다르게
        float direction = (index % 2 == 0) ? 1f : -1f;
        return new Vector2(0, Mathf.Sin(time * wobbleSpeed + index * 0.15f) * wobbleHeight * direction);
    }
}
