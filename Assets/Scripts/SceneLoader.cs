using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string name)
    {
        switch(name)
        {
            case "exit":
                Debug.Log("Exit Game.");
                Application.Quit();
                break;
            default:
                SceneManager.LoadScene(name);
                break;
        }
    }
}
