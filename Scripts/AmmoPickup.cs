using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    //Public Variable
    public int smallAmmo, mediumAmmo, largeAmmo;

    //Private Variable
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
            int playerAmmo, maxPlayerAmmo;

            //Set the other.gameObject to the triggeringPlayer
            triggeringPlayer = other.gameObject;

            //Get the Player component from the triggeringPlayer object and set the ammoHeld value to playerAmmo
            playerAmmo = triggeringPlayer.GetComponent<Player>().ammoHeld;

            //Get the Player component from the triggeringPlayer object and set the ammoCapacity value to maxPlayerAmmo
            maxPlayerAmmo = triggeringPlayer.GetComponent<Player>().ammoCapacity;

            //Check if playerAmmo is less than maxPlayerAmmo
            if (playerAmmo < maxPlayerAmmo)
            {
                //Check if the gameObject name contains SmallAmmo
                if (this.gameObject.name.Contains("SmallAmmo"))
                {
                    //Set smallAmmo value to 5
                    smallAmmo = 5;

                    //Plus the value of smallAmmo on to playerAmmo
                    playerAmmo += smallAmmo;

                    //Check if the playerAmmo value is more than maxPlayerAmmo
                    if (playerAmmo > maxPlayerAmmo)
                    {
                        //If it is then set playerAmmo to maxPlayerAmmo
                        playerAmmo = maxPlayerAmmo;
                    }
                    //Get the Player component from the triggeringPlayer object and call the SetAmmo method
                    triggeringPlayer.GetComponent<Player>().SetAmmo(playerAmmo);
                }
                else if (this.gameObject.name.Contains("MediumAmmo"))//Check if the gameObject name contains MediumAmmo
                {
                    //Set mediumAmmo value to 10
                    mediumAmmo = 10;

                    //Plus the value of mediumAmmo on to playerAmmo
                    playerAmmo += mediumAmmo;

                    //Check if the playerAmmo value is more than maxPlayerAmmo
                    if (playerAmmo > maxPlayerAmmo)
                    {
                        //If it is then set playerAmmo to maxPlayerAmmo
                        playerAmmo = maxPlayerAmmo;
                    }
                    //Get the Player component from the triggeringPlayer object and call the SetAmmo method
                    triggeringPlayer.GetComponent<Player>().SetAmmo(playerAmmo);
                }
                else if (this.gameObject.name.Contains("LargeAmmo"))//Check if the gameObject name contains LargeAmmo
                {
                    //Set playerAmmo to maxPlayerAmmo
                    playerAmmo = maxPlayerAmmo;
                    //Get the Player component from the triggeringPlayer object and call the SetAmmo method
                    triggeringPlayer.GetComponent<Player>().SetAmmo(playerAmmo);
                }
                //Need to have collider on PlayerHolder GameObject to detect collision

                audioManager.PlaySound("Reload");
                
                //Destroy this gameObject
                Destroy(this.gameObject);
            }
            else
            {
                //Show player that they are at max ammo capacity.
                gameManager.ShowAmmoInfo();
            }
        }
    }
}
