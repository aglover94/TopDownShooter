using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour
{
    public float delay = 3f;
    public float speed;
    public Rigidbody grenadeRB;

    private float countDown;
    private bool hasExploded = false;

    private void Start()
    {
        countDown = delay;
    }
    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;

        //Vector3 movement = transform.up * speed * Time.deltaTime;
        //grenadeRB.MovePosition(transform.position + movement);
        grenadeRB.AddForce(transform.up * speed, ForceMode.VelocityChange);
        if (countDown <= 0f && !hasExploded)
        {
            Destroy(this.gameObject);
            hasExploded = true;
        }
    }
}
