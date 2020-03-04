using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Public Variables
    public Player_Controls controls;
    public GameObject gameManager, audioManager, bulletSpawnPoint, bullet;
    public float waitTime = 0f;
    public float fireRate = 2f;
    public float moveSpeed;
    public int health, maxhealth = 10;
    public bool pauseGame = false;
    public int ammoClip = 10, ammoHeld, ammoCapacity = 50;
    public int currentAmmo;
    public float reloadTime = 1f;
    public bool interactPressed = false, canInteract = false;

    //Private Variables
    private Vector2 movementinput;
    private Vector2 lookPos;
    private Animator animator;
    private Rigidbody playerrb;
    private float timer;
    private Vector3 inputDir, movement;
    private Plane playerPlane;
    private GameManager gameM;
    private AudioManager audioM;

    private void Awake()
    {
        //Create an instance of Player_Controls and set it in the controls variable
        controls = new Player_Controls();
        
        controls.PlayerControls.Move.performed += ctx => movementinput = ctx.ReadValue<Vector2>(); //If the move input is performed then read the values
        controls.PlayerControls.Move.canceled += ctx => movementinput = Vector2.zero; //If the move input is canceled then set Vector2 variables to 0

        controls.PlayerControls.Rotation.performed += ctx => lookPos = ctx.ReadValue<Vector2>(); //If the rotation input is performed then read the values

        //Get Animator in children
        animator = GetComponentInChildren<Animator>();

        //Get the RigidBody component from this object and set it to the playerrb variable
        playerrb = this.GetComponent<Rigidbody>();

        //Set playerPlane to a new Plane
        playerPlane = new Plane(Vector3.up, transform.position);

        //Set the currentAmmo to ammoClip and ammoHeld to the ammoCapacity
        currentAmmo = ammoClip;
        ammoHeld = ammoCapacity;

        //Get the GameManager and AudioManager components and set them to respective variables
        gameM = gameManager.GetComponent<GameManager>();
        audioM = audioManager.GetComponent<AudioManager>();
    }

    private void Update()
    {
        //Plus the timer variable with the Time.deltaTime value
        timer += Time.deltaTime;

        //Check if the Firing input action has been triggered and that time is more than waitTime and that currentAmmo is more than 0
        if(controls.PlayerControls.Firing.triggered == true && Time.time >= waitTime && currentAmmo > 0)
        {
            //If it is then call the Shooting method and set the waitTime to Time.time + 1 / fireRate
            waitTime = Time.time + 1f / fireRate;
            Shooting();
        }

        if (controls.PlayerControls.Firing.triggered == true && currentAmmo == 0 && ammoHeld > 0)
        {
            StartCoroutine(Reload());
        }

        //Check if the Reload input action has been triggered, and that currentAmmo is less than ammoClip and ammoHeld is more than 0
        if (controls.PlayerControls.Reload.triggered == true && currentAmmo < ammoClip && ammoHeld > 0)
        {
            //If that is true than start the coroutine of Reload
            StartCoroutine(Reload());
        }

        //Check if health is less than or equal to 0
        if (health <= 0)
        {
            //If it is then call the Die method
            Die();
        }
    }

    private void FixedUpdate()
    {
        //Create float variables h and v, set their values to the movementInput.x and y respectively
        float h = movementinput.x;
        float v = movementinput.y;
        var targetInput = new Vector3(h, 0, v);

        inputDir = Vector3.Lerp(inputDir, targetInput, Time.deltaTime * 10f);

        var cameraForward = Camera.main.transform.forward;
        var cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 desiredDirection = cameraForward * inputDir.z + cameraRight * inputDir.x;

        //When needed call the MoveThePlayer method using the desiredDirection variable and also call the TurnThePlayer method 
        MoveThePlayer(desiredDirection);
        TurnThePlayer();
    }

    //Private method called when moving the player
    private void MoveThePlayer(Vector3 desiredDir)
    {
        movement.Set(desiredDir.x, 0f, desiredDir.z);
        animator.SetFloat("Speed", movement.magnitude);

        movement = movement * moveSpeed * Time.deltaTime;
        playerrb.MovePosition(transform.position + movement);
        
    }

    //Private method called when turning the player
    private void TurnThePlayer()
    {
        //Checks if the current mouse was updated this frame. Used for mouse input
        if (Mouse.current.wasUpdatedThisFrame == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0.0f;

            if(playerPlane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Quaternion targetRot = Quaternion.LookRotation(targetPoint - transform.position);
                targetRot.x = 0;
                targetRot.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 7f * Time.deltaTime);
            }
        }
        else
        {
            //If a mouse wasn't updated this frame then use the input system for a gamepad

            Vector2 input = lookPos;

            Vector3 lookDir = new Vector3(input.x, 0, input.y);
            var lookRot = Camera.main.transform.TransformDirection(lookDir);
            lookRot = Vector3.ProjectOnPlane(lookRot, Vector3.up);

            if (lookRot != Vector3.zero)
            {
                Quaternion newRot = Quaternion.LookRotation(lookRot);
                playerrb.MoveRotation(newRot);
            }
        }
    }

    //Private method to shoot a weapon
    private void Shooting()
    {
        //Minus 1 to the currentAmmo, instantiate the bullet model and play the Fire audio
        currentAmmo--;
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        audioM.PlaySound("Fire");
    }

    //A private IEnumerator coroutine to reload a weapon
    private IEnumerator Reload()
    {
        //Wait to do the rest of the coroutine for the amount specified in the reloadTime variable
        yield return new WaitForSeconds(reloadTime);
        
        //Check if the current ammo is less than or equal to 0
        if(currentAmmo <= 0)
        {
            //If it is then check if ammoheld is more than or equal to 10
            if (ammoHeld >= 10)
            {
                //If it is then set the currentAmmo to the value of ammoClip. And minus the value of ammoClip from the ammoHeld variable
                currentAmmo = ammoClip;
                ammoHeld -= ammoClip;
            }
            else if(ammoHeld > 0 && ammoHeld < 10) //If it isn't >= 10 then check if ammoHeld is more than 0 and ammo held is less than 10
            {
                //If it is then while currentAmmo doesn't equal ammoClip and ammoHeld doesn't equal 0
                while (currentAmmo != ammoClip && ammoHeld != 0)
                {
                    //Plus 1 to currentAmmo and minus 1 to ammoHeld
                    currentAmmo++;
                    ammoHeld--;
                }
            }
            else if(ammoHeld == 0)//If it isn't >= 10 then check if ammoHeld is equal to 0
            {
                //Do something
            }
        }
        else if(currentAmmo > 0 && currentAmmo < 10)//If it isn't <= 0 then check if currentAmmo is more than 0 and less than 10
        {
            //If it is then check if the ammoHeld is more than or equal to 10
            if (ammoHeld >= 10)
            {
                //If it is then create a loop adding 1 to currentAmmo and taking away 1 from the ammoheld until currentAmmo equals ammoClip
                for (int i = currentAmmo; i < ammoClip; i++)
                {
                    currentAmmo++;
                    ammoHeld--;
                }
            }
            else if(ammoHeld > 0 && ammoHeld < 10)//If it isn't >= 10 then check if ammoHeld is more than 0 and ammo held is less than 10
            {
                //If it is then while currentAmmo doesn't equal ammoClip and ammoHeld doesn't equal 0
                while (currentAmmo != ammoClip && ammoHeld != 0)
                {
                    //Plus 1 to currentAmmo and minus 1 to ammoHeld
                    currentAmmo++;
                    ammoHeld--;
                }
            }
            else if(ammoHeld == 0)
            {
                //Do something
            }
        }

        audioM.PlaySound("Reload");
    }

    //Public method that will be called when the player dies
    public void Die()
    {
        audioM.PlaySound("Player Death");
        gameM.Dead();
    }

    //Public method to set the health variable to the value of setHealth
    public void SetHealth(int setHealth)
    {
        health = setHealth;
    }

    //Public method to set the ammoHeld variable to the value of setAmmo 
    public void SetAmmo(int setAmmo)
    {
        ammoHeld = setAmmo;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
