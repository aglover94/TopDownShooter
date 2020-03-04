using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Public Variables
    public float speed, maxDis;
    public int damage = 1;
    public Rigidbody bulletRB;

    //Private Variable
    private GameObject triggeringEnemy, triggeringPlayer;

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = transform.up * speed * Time.deltaTime;
        bulletRB.MovePosition(transform.position + movement);

        maxDis += 1 * Time.deltaTime;

        //Check if the maxDis is more than or equal to 5
        if (maxDis >= 5)
        {
            //If it is then destroy this object
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Check tag for the enemy model
        if(other.tag == "EnemyModel")
        {
            //Set the other.gameObject to the triggeringEnemy variable
            triggeringEnemy = other.gameObject;
            //Get the Enemy component and take away the damage value from the health value
            triggeringEnemy.GetComponentInParent<Enemy>().health -= damage;
            //Destroy this object
            Destroy(this.gameObject);
        }
        else if(other.tag == "Player")//Check if the tag of other is Player
        {
            //Need to have collider on PlayerHolder GameObject to detect collision

            //Set the other.gameObject to the triggeringPlayer variable
            triggeringPlayer = other.gameObject;
            //Get the Player component and take away the damage value from the health value
            triggeringPlayer.GetComponent<Player>().health -= damage;
            
            //Then destroy this object
            Destroy(this.gameObject);
            
        }
        else if (other.tag == "Environment")//Check if the tag of other is Environment
        {
            //If it is then destroy this object
            Destroy(this.gameObject);

        }
    }
}
