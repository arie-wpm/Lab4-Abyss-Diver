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
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
                AudioListener.pause = false;
            }
            else
            {
                Time.timeScale = 0f;
                AudioListener.pause = true;
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
