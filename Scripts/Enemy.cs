using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //Public Variables
    public float waitTime;
    public int health, enemyMaxHealth;
    public GameObject bullet, bulletSpawnPoint;
    public Canvas canvasObj;
    public Image greenHealth;

    //Private Variables
    private Camera GameCamera;
    private Player player;
    private float currentWait;
    private bool shot;
    private PlayerDetection playerDetection;
    private Player playerTarget;
    private float currentHealth, minHealth, maxHealth;
    private float attackTimer;
    private AudioManager audioManager;
    [SerializeField]
    private float attackRefreshRate = 1.5f;
    private Animator animator;
    private EnemyMovement em;
    private bool stopAttacking;
    private bool notDead = true;

    private void Awake()
    {
        GameCamera = Camera.main;
        playerDetection = GetComponent<PlayerDetection>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerDetection.OnDetection += PlayerDetection_OnDetection;
        enemyMaxHealth = 5;
        health = 5;
        minHealth = 0;
        maxHealth = 1;
        animator = GetComponentInChildren<Animator>();
        em = GetComponent<EnemyMovement>();
        stopAttacking = false;
    }

    private void PlayerDetection_OnDetection(Transform target)
    {
        //Get the Player component from the target and set it to the player variable
        Player player = target.GetComponent<Player>();

        //Check if player doesn't equal null
        if(player != null)
        {
            //Set playerTarget to player
            playerTarget = player;
        }
    }

    public void Update()
    {
        //Check if health is less than or equal to 0
        if(health <= 0)
        {
            
            //If it is then call the Die method
            Die();
        }

        //Set current health to the value returned from the Map method
        currentHealth = Map(health, 0, enemyMaxHealth, minHealth, maxHealth);

        canvasObj.transform.LookAt(GameCamera.transform, GameCamera.transform.up);   //Making the canvas always follow the game camera (make the text readable)

        //Set the fillAmount of greenHealth to currentHealth value
        greenHealth.fillAmount = currentHealth;

        //Check if playerTarget doesn't equal null
        if (playerTarget != null)
        {
            if (notDead)
            {
                this.transform.LookAt(playerTarget.transform);
            }

            //Plus the value of Time.deltaTime to the value of attackTimer
            attackTimer += Time.deltaTime;

            //Check if CanAttack returns true
            /*if(CanAttack())
            {
                //Call the Attack method
                Attack();
            }*/

            if (em.stopped)
            {
                if (CanAttack() && !stopAttacking)
                {
                    Debug.Log("Attack");
                    Attack();
                }
            }
        }
    }

    public void Die()
    {
        //Play Enemy Death audio and destroy this gameObject
        audioManager.PlaySound("Enemy Death");
        animator.SetBool("Death", true);
        stopAttacking = true;
        notDead = false;
        em.ResetDestination();

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }

    private bool CanAttack()
    {
        //Return true if attackTimer is more than attackRefreshRate else return false
        return attackTimer >= attackRefreshRate;
    }

    private void Attack()
    {
        //Set attackTimer to 0
        animator.SetBool("FinishedWalking", true);
        animator.SetBool("StartWalking", false);
        attackTimer = 0;

        //Instantiate the bullet model and play the Fire audio
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        audioManager.PlaySound("Fire");
    }

    private float Map(int value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
