using UnityEngine;
using System.Collections;

public class FishBehavior : MonoBehaviour
{
    private Coroutine moveRoutine;
    private Coroutine bopRoutine;

    [HideInInspector] public Vector3 originalLocalPos;

    [Header("Bop Settings")]
    public float bopAmplitude = 0.2f;
    public float bopSpeed = 2f;

    private void Start()
    {
        float randomOffset = Random.Range(0f, Mathf.PI * 2f);
        bopRoutine = StartCoroutine(Bop(randomOffset));
    }

    public void MoveToLocalPosition(Vector3 target, float duration)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveCoroutine(target, duration));
    }

    private IEnumerator MoveCoroutine(Vector3 target, float duration)
    {
        Vector3 start = transform.localPosition;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.localPosition = target;
    }

    private IEnumerator Bop(float startPhase)
    {
        Vector3 basePos = transform.localPosition;
        float phase = startPhase;

        while (true)
        {
            phase += Time.deltaTime * bopSpeed;
            float yOffset = Mathf.Sin(phase) * bopAmplitude;
            transform.localPosition = new Vector3(transform.localPosition.x, basePos.y + yOffset, transform.localPosition.z);
            yield return null;
        }
    }
}