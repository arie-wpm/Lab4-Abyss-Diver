using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevelLoad : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
