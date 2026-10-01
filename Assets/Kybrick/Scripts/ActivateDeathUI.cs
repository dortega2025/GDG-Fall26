using UnityEngine;
using UnityEngine.SceneManagement;
public class ActivateDeathUI : MonoBehaviour
{
    public static ActivateDeathUI instance; 
    public GameObject Container;

    public void Awake()
    {
        instance = this;
    }

    public void ActivateUI()
    {
        Container.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
