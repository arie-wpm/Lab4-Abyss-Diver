using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameInputHandler : MonoBehaviour
{
    InputAction restart;
    InputAction pause;
    InputAction reset;

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
            Time.timeScale = 1;
            AudioListener.pause = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (pause != null && pause.WasPressedThisFrame())
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
            }
            else
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
            }
        }

        if (reset != null && reset.WasPressedThisFrame())
        {
            Time.timeScale = 1;
            AudioListener.pause = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
