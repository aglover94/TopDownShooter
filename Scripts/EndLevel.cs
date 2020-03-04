using UnityEngine;

public class EndLevel : MonoBehaviour
{
    //Public Variable
    public GameManager gameManager;

    //Private Variables
    private Player player;
    private bool hasPlayer, enterTrigger = false;

    private void Update()
    {
        //Check if enterTrigger is true
        if (enterTrigger)
        {
            //If it is then check if the Interact input action has been triggered and hasPlayer is true
            if (player.controls.PlayerControls.Interact.triggered && hasPlayer)
            {
                //Call the CompleteLevel method from gameManager
                gameManager.CompleteLevel();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Set enterTrigger to true
        enterTrigger = true;

        //Check if the tag of other is Player
        if (other.tag == "Player")
        {
            //Get the Player component and set it to player. Then call the OpenInteraction method from gameManager and set the hasPlayer to true
            player = other.GetComponent<Player>();
            gameManager.OpenInteraction();
            
            hasPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Set enterTrigger to false
        enterTrigger = false;

        //Check if the tag of other is Player
        if (other.tag == "Player")
        {
            //Call the CloseInteraction method from gameManager and set the hasPlayer to false
            gameManager.CloseInteraction();
            hasPlayer = false;
        }
    }
}
