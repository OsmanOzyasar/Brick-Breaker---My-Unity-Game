
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    } 

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }

   
}
