using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillGetEffect : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image skillBorder;
    [SerializeField] private GameObject skillParticlePrefab;
    [SerializeField] private Transform particleSpawnPoint;

    [Header("잔상 효과 세팅")]
    [SerializeField] private Image smallSquarePrefab;
    [SerializeField] private int smallSquareCount = 40;  // 잔상 조각 개수 (많을수록 부드러움)
    [SerializeField] private float oneLoopDuration = 3f;

    [Header("커스텀 경계선 (없으면 skillBorder 사용)")]
    [SerializeField] private RectTransform customBorderRect;

    private List<Image> smallSquares = new List<Image>();
    private List<Vector3> pathPositions = new List<Vector3>();

    private Coroutine edgeGlowCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlaySkillGetEffect();
        }
    }

    public void PlaySkillGetEffect()
    {
        // 아이콘은 항상 켜져 있고 색 초기화만 하도록 수정
        skillIcon.color = new Color(1, 1, 1, 0);
        skillIcon.gameObject.SetActive(true);
        skillBorder.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();

        seq.Append(skillIcon.DOFade(1f, 0.3f));
        seq.Join(skillIcon.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));
        seq.Append(skillIcon.DOFade(1f, 0.5f));
        seq.Join(skillIcon.transform.DOScale(1f, 0.5f).SetEase(Ease.InBack));
        /*
        seq.AppendCallback(() =>
        {
            if (skillParticlePrefab != null && particleSpawnPoint != null)
            {
                var p = Instantiate(skillParticlePrefab, particleSpawnPoint.position, Quaternion.identity, particleSpawnPoint.parent);
                Destroy(p, 2f);
            }
        });
        */
        seq.AppendCallback(() =>
        {
            SetupEdgeGlow();
            if (edgeGlowCoroutine != null) StopCoroutine(edgeGlowCoroutine);
            edgeGlowCoroutine = StartCoroutine(EdgeGlowLoop());
        });

        seq.Play();
    }

    private void SetupEdgeGlow()
    {
        // 기존 잔상 오브젝트 제거
        foreach (var sq in smallSquares)
        {
            if (sq != null) Destroy(sq.gameObject);
        }
        smallSquares.Clear();
        pathPositions.Clear();

        // 경계선 RectTransform 선택(customBorderRect 우선)
        RectTransform rt = customBorderRect != null ? customBorderRect : skillBorder.rectTransform;

        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        // 네 변 길이 계산
        float[] edgeLengths = new float[4];
        for (int i = 0; i < 4; i++)
        {
            edgeLengths[i] = Vector3.Distance(corners[i], corners[(i + 1) % 4]);
        }
        float perimeter = 0;
        foreach (var len in edgeLengths) perimeter += len;

        // 작은 사각형 가로 크기(px) 기준 - 화면 좌표계로 계산
        float smallSquareWidth = smallSquarePrefab.rectTransform.rect.width * skillBorder.canvas.scaleFactor;
        smallSquareWidth = Mathf.Clamp(smallSquareWidth, 5f, 30f);

        // perimeter 를 smallSquareWidth 단위로 쪼갠다 (작은 사각형이 딱 붙도록)
        int count = Mathf.FloorToInt(perimeter / smallSquareWidth);
        smallSquareCount = count;

        // 변별로 몇 개씩 배치할지 계산
        int[] edgeCounts = new int[4];
        for (int i = 0; i < 4; i++)
        {
            edgeCounts[i] = Mathf.RoundToInt(edgeLengths[i] / perimeter * count);
        }

        // edgeCounts 합 조정
        int sumEdgeCounts = 0;
        foreach (var c in edgeCounts) sumEdgeCounts += c;

        while (sumEdgeCounts != count)
        {
            for (int i = 0; i < 4 && sumEdgeCounts != count; i++)
            {
                edgeCounts[i] += (sumEdgeCounts < count) ? 1 : -1;
                sumEdgeCounts = 0;
                foreach (var c in edgeCounts) sumEdgeCounts += c;
            }
        }

        // 각 변별로 작은 사각형 위치 생성 (꼭짓점에 붙도록)
        for (int i = 0; i < 4; i++)
        {
            Vector3 start = corners[i];
            Vector3 end = corners[(i + 1) % 4];
            int segmentCount = edgeCounts[i];

            for (int j = 0; j < segmentCount; j++)
            {
                float t = j / (float)segmentCount;
                Vector3 pos = Vector3.Lerp(start, end, t);
                pathPositions.Add(pos);
            }
        }

        // 잔상 조각 생성 및 초기 위치 설정
        for (int i = 0; i < smallSquareCount; i++)
        {
            Image sq = Instantiate(smallSquarePrefab, skillBorder.transform.parent);
            sq.rectTransform.pivot = new Vector2(0.5f, 0f);
            sq.transform.position = pathPositions[i];
            sq.color = new Color(1, 1, 1, 0);
            smallSquares.Add(sq);
        }
    }

    private IEnumerator EdgeGlowLoop()
    {
        int pathLength = pathPositions.Count;

        float elapsed = 0f;
        while (elapsed < oneLoopDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / oneLoopDuration; // 0~1

            for (int i = 0; i < smallSquares.Count; i++)
            {
                float progress = (t * pathLength + i) % pathLength;

                int idx1 = Mathf.FloorToInt(progress) % pathLength;
                int idx2 = (idx1 + 1) % pathLength;
                float lerpT = progress - idx1;

                Vector3 pos = Vector3.Lerp(pathPositions[idx1], pathPositions[idx2], lerpT);
                smallSquares[i].transform.position = pos;

                // 알파값 점진적으로 감소 (잔상 효과)
                float alpha = 1f - ((progress - i) / smallSquares.Count);
                if (alpha < 0) alpha = 0;

                Color originalColor = smallSquares[i].color;
                smallSquares[i].color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            }

            yield return null;
        }

        // 끝나면 잔상 제거 및 테두리 비활성화
        foreach (var sq in smallSquares)
        {
            if (sq != null) Destroy(sq.gameObject);
        }
        smallSquares.Clear();
        skillBorder.gameObject.SetActive(false);
    }
}
