using UnityEngine;
using System.Collections;

public class FishGroupController : MonoBehaviour
{
    [Header("School Settings")]
    public FishBehavior[] fishes;
    public float scatterDistance = 2f;
    public float scatterDuration = 0.5f;
    public float regroupDelay = 3f;
    public float regroupDuration = 1f;

    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;
    public float patrolPause = 0.5f;
    public bool patrolLoop = true;

    private void Awake() {
        if (fishes.Length == 0)
            fishes = GetComponentsInChildren<FishBehavior>();

        foreach (var fish in fishes) {
            fish.originalLocalPos = fish.transform.localPosition;
            SpriteRenderer sr = fish.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = Random.Range(-2, 3);
        }
    }

    private void Start() {
        if (pointA != null && pointB != null) {
            transform.position = pointA.position;
            StartCoroutine(PatrolRoutine());
        } 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;

        ScatterSchool();
        StopCoroutine("RegroupAfterDelay"); 
        StartCoroutine(RegroupAfterDelay());
    }

    private void ScatterSchool() {
        foreach (var fish in fishes) {
            float angle = Random.Range(0f, 360f);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            float distance = scatterDistance * Random.Range(0.8f, 1.2f);
            Vector3 target = fish.originalLocalPos + (Vector3)dir * distance;
            SpriteRenderer sr = fish.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = Random.Range(-2, 3);

            fish.MoveToLocalPosition(target, scatterDuration);
        }
    }

    private IEnumerator RegroupAfterDelay() {
        yield return new WaitForSeconds(regroupDelay);

        foreach (var fish in fishes) {
            fish.MoveToLocalPosition(fish.originalLocalPos, regroupDuration);
        }
    }

    private IEnumerator PatrolRoutine() {
        Transform target = pointB;

        while (true) {
            while (Vector3.Distance(transform.position, target.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(patrolPause);
            target = (target == pointA) ? pointB : pointA;

            if (fishes.Length == 0)
                fishes = GetComponentsInChildren<FishBehavior>();

            foreach (var fish in fishes) {
                SpriteRenderer sr = fish.GetComponent<SpriteRenderer>();
                if (sr != null && target == pointB) sr.flipX = true;
                else sr.flipX = false;
            }

            if (!patrolLoop) break;
        }
    }
}