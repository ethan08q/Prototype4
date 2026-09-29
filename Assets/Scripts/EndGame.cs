using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            SceneManager.LoadScene("EndScene");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
   

