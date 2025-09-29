using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnStartButton();
        }
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
