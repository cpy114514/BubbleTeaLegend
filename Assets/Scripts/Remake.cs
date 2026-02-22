using UnityEngine;
using UnityEngine.SceneManagement;

public class RedoScene : MonoBehaviour
{
    public void Redo()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
