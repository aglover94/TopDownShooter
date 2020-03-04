using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    //Public Variables
    public int smallHealth, mediumHealth, largeHealth;

    //Private Variables
    private GameObject triggeringPlayer;
    private AudioManager audioManager;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //Find the objects with each tag, then get the AudioManager and GameManager component then set them into respective variables
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        //Check if tag of other is Player
        if (other.tag == "Player")
        {
            int playerHealth, maxPlayerHealth;

            //Set the other.gameObject to the triggeringPlayer
            triggeringPlayer = other.gameObject;

            //Get the Player component from the triggeringPlayer object and set the playerHealth value to health
            playerHealth = triggeringPlayer.GetComponent<Player>().health;
            //Get the Player component from the triggeringPlayer object and set the maxPlayerHealth value to maxHealth
            maxPlayerHealth = triggeringPlayer.GetComponent<Player>().maxhealth;

            //Check if playerHealth is less than maxPlayerHealth
            if (playerHealth < maxPlayerHealth)
            {
                //Check if the gameObject name contains SmallHealth
                if (this.gameObject.name.Contains("SmallHealth"))
                {
                    //Set smallHealth value to 1
                    smallHealth = 1;

                    //Plus the value of smallHealth on to playerHealth
                    playerHealth += smallHealth;

                    //Check if the playerHealth value is more than maxPlayerHealth
                    if (playerHealth > maxPlayerHealth)
                    {
                        //If it is then set playerHealth to maxPlayerHealth
                        playerHealth = maxPlayerHealth;
                    }
                    //Get the Player component from the triggeringPlayer object and call the SetHealth method
                    triggeringPlayer.GetComponent<Player>().SetHealth(playerHealth);
                }
                else if (this.gameObject.name.Contains("MediumHealth"))//Check if the gameObject name contains MediumHealth
                {
                    //Set mediumHealth value to 5
                    mediumHealth = 5;

                    //Plus the value of mediumHealth on to playerHealth
                    playerHealth += mediumHealth;

                    //Check if the playerHealth value is more than maxPlayerHealth
                    if (playerHealth > maxPlayerHealth)
                    {
                        //If it is then set playerHealth to maxPlayerHealth
                        playerHealth = maxPlayerHealth;
                    }
                    //Get the Player component from the triggeringPlayer object and call the SetHealth method
                    triggeringPlayer.GetComponent<Player>().SetHealth(playerHealth);
                }
                else if (this.gameObject.name.Contains("LargeHealth"))//Check if the gameObject name contains LargeHealth
                {
                    //Set playerHealth to maxPlayerHealth
                    playerHealth = maxPlayerHealth;
                    //Get the Player component from the triggeringPlayer object and call the SetHealth method
                    triggeringPlayer.GetComponent<Player>().SetHealth(playerHealth);
                }

                //Need to have collider on PlayerHolder GameObject to detect collision
                audioManager.PlaySound("Health Pickup");
                Destroy(this.gameObject);
            }
            else
            {
                //Show player they are at max health already
                gameManager.ShowHealthInfo();

            }
        }
    }
}
