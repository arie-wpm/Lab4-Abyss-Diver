using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class TileHider : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private Coroutine exitCoroutine;

    // adding a delay because the player Collider changes size
    [SerializeField] private float exitDelay = 0.5f;

    void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        if (tilemapRenderer == null)
            Debug.LogWarning("No TilemapRenderer found on this object!");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (exitCoroutine != null)
            {
                StopCoroutine(exitCoroutine);
                exitCoroutine = null;
            }

            tilemapRenderer.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (exitCoroutine != null) StopCoroutine(exitCoroutine);
            exitCoroutine = StartCoroutine(EnableRendererAfterDelay());
        }
    }

    private IEnumerator EnableRendererAfterDelay()
    {
        yield return new WaitForSeconds(exitDelay);
        tilemapRenderer.enabled = true;
        exitCoroutine = null;
    }
}