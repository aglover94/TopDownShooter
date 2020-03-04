using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

//Placed on the MainMenuManager in the main menu to control what happens when the user clicks on a button
public class MainMenu : MonoBehaviour
{
    public AudioManager audioManager;

    //Public method to load into the level specified by the string level
    public void LoadLevel(string level)
    {
        audioManager.PlaySound("Button Click");
        SceneManager.LoadScene(level);
    }

    //Public method to quit the demo from the main menu
    public void QuitDemo()
    {
        audioManager.PlaySound("Button Click");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
