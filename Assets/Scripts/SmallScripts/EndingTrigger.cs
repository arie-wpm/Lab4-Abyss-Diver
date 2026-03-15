using UnityEngine;
using UnityEngine.SceneManagement;


public class EndingTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            Animator anim = other.GetComponent<Animator>();
            anim.Rebind();
            SceneManager.LoadScene("Ending"); 
        }
    }
}
