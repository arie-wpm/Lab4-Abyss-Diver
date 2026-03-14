using System;
using UnityEngine;

public class GameOverMenuManager : MonoBehaviour
{
    private void Awake()
    {
        UIManager.instance.SetGameOverScreen(gameObject);
    }
}
