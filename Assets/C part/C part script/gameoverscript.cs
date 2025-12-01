using UnityEngine;
using UnityEngine.SceneManagement;

public class gameoverscript : MonoBehaviour
{

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
