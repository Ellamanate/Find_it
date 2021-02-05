using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        PlayerPrefs.SetInt("Load", false ? 1 : 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Map");
    }

    public void LoadLevel()
    {
        PlayerPrefs.SetInt("Load", true ? 1 : 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Map");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
