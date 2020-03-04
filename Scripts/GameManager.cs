using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Public Variables
    public static bool gameIsPaused = false;
    public TextMeshProUGUI healthText, ammoText;
    public GameObject pausePanel, gameOverPanel, interactPanel, completePanel, objectivePanel, infoPanel, audioManager;
    public Image greenHealth;
    public float currentHealth, maxHealth, minHealth;
    public string timer;

    //Private Variables
    private int playerHealth, playerMaxHealth, playerCurrentAmmo, playerMaxAmmo;
    private GameObject playerObject;
    private Player playerScript;
    private float startTime;


    private void Awake()
    {
        maxHealth = 1.0f;
        minHealth = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<Player>();
        audioManager.GetComponent<AudioManager>().PlaySound("Environment Audio");
        gameIsPaused = false;
        Time.timeScale = 1f;

        //playerScript.controls.PlayerControls.Pause.performed += ctx => PausePressed();
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");
        timer = minutes + ":" + seconds;

        playerHealth = playerScript.health;
        playerMaxHealth = playerScript.maxhealth;
        playerCurrentAmmo = playerScript.currentAmmo;
        playerMaxAmmo = playerScript.ammoHeld;


        currentHealth = Map(playerHealth, 0, playerMaxHealth, minHealth, maxHealth);
        greenHealth.fillAmount = currentHealth;

        healthText.text = playerHealth + " / " + playerMaxHealth;
        ammoText.text = playerCurrentAmmo + " / " + playerMaxAmmo;

        //Check if the keyDown is P
        if(Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("PauseXbox"))
        {
            //If it is then check if gameIsPaused is true
            if(gameIsPaused)
            {
                //If it is then call Resume
                Resume();
            }
            else
            {
                //Else call pause
                Pause();
            }
        }
    }

    private void FixedUpdate()
    {
        /*if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (gameIsPaused)
            {
                //If it is then call Resume
                Resume();
            }
            else
            {
                //Else call pause
                Pause();
            }
        }*/
    }

    //Public method to resume the game
    public void Resume()
    {
        //Set the pausePanel to inactive, timeScale set to 1 and gameIsPaused set to false
        pausePanel.SetActive(false);
        
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    //Public method to pause the game
    public void Pause()
    {
        //Set the pausePanel to active, timeScale set to 0 and gameIsPaused set to true
        pausePanel.SetActive(true);
        //Time.timeScale = 0.05f; //This is what is causing pause not to work from the new input system(fix)
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    void PausePressed()
    {
        if (gameIsPaused)
        {
            //If it is then call Resume
            Resume();
        }
        else
        {
            //Else call pause
            Pause();
        }
    }

    //Public method called when going to the main menu
    public void MainMenu()
    {
        //Set timeScale to 1 and load the scene
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene("Main_Menu");
    }

    //Public method called when dead
    public void Dead()
    {
        //Set gameOverPanel to active and set timeScale to 0
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    //Public method called when restarting the level
    public void RestartLevel()
    {
        //Set timeScale to 1, and load the scene
        /*Time.timeScale = 1f;
        gameIsPaused = false;*/
        SceneManager.LoadScene("Chapter_1");
    }

    //Public method called when completing the level
    public void CompleteLevel()
    {
        //Set the completePanel to active, set timeScale to 0 and set gameIsPaused to true
        completePanel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    //Public method to open the interaction panel
    public void OpenInteraction()
    {
        //Set the interact panel to active
        interactPanel.SetActive(true);
    }

    //Public method to close the interaction panel
    public void CloseInteraction()
    {
        //Set the interact panel to inactive
        interactPanel.SetActive(false);
    }

    //Public method to show HealthInfo
    public void ShowHealthInfo()
    {
        //Set the infoPanel to active
        infoPanel.SetActive(true);
        //Get the TextMeshProUGUI component and set the text to Ammo Full
        infoPanel.GetComponentInChildren<TextMeshProUGUI>().SetText("Health full");
        //Get the FadeInfo component and call the StartFade method
        infoPanel.GetComponent<FadeInfo>().StartFade();
    }

    //Public method to show AmmoInfo
    public void ShowAmmoInfo()
    {
        //Set the infoPanel to active
        infoPanel.SetActive(true);
        //Get the TextMeshProUGUI component and set the text to Ammo Full
        infoPanel.GetComponentInChildren<TextMeshProUGUI>().SetText("Ammo full");
        //Get the FadeInfo component and call the StartFade method
        infoPanel.GetComponent<FadeInfo>().StartFade();
    }

    private float Map(int value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
