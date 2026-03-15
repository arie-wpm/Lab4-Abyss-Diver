using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputHandler : MonoBehaviour
{
    private InputAction restart;
    private InputAction pause;
    private InputAction reset;

    void Start()
    {
        restart = InputSystem.actions.FindAction("Restart");
        pause = InputSystem.actions.FindAction("Pause");
        reset = InputSystem.actions.FindAction("ResetLevel");
    }

    void Update()
    {
        if (restart != null && restart.WasPressedThisFrame())
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;

            GameManager.instance.RestartToTitle();
        }

        if (pause != null && pause.WasPressedThisFrame())
        {
            if (GameStateManager.Instance.CurrentGameState == GameState.Play)
            {
                GameStateManager.Instance.SetGameState(GameState.Pause);
            } else if (GameStateManager.Instance.CurrentGameState == GameState.Pause)
            {
                GameStateManager.Instance.SetGameState(GameState.Play);
            }
        }

        if (reset != null && reset.WasPressedThisFrame())
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;

            GameManager.instance.RestartCurrentLevel();
        }
    }
}
