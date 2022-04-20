using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float playerSpeed;  //Declaring a varible playerspeed for player
    public float playerJumpForce; //Declaring a variable player Jumpforce
    public float playerRotationSpeed; // Declaring a variable player rotation speed

    Rigidbody rb;                 //Declaring a rigidbody
    CapsuleCollider colliders;    //Collider
    Quaternion camRotation;
    public Camera cam;              //Declaring camera
    Quaternion playerRotation;
    public Transform bulletLaunch;
    public GameObject playerPrefabs;


    float inputX;
    float inputz;
    public Animator animator;
    SpawnManager spawnManager;

    int ammo = 50;
    int medical = 100;
    int maxAmmo = 100;
    int maxMedical = 100;
    int reloadAmmo = 0;
    int maxReloadAmmo = 10;
    //public Transform bulletLaunch;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponent<CapsuleCollider>();
        //spawnManager = GameObject.Find("SpawnPosition").GetComponent<SpawnManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("isReload", !animator.GetBool("isReload"));
            int amountAmmoNeeded = maxReloadAmmo - reloadAmmo;
            int ammoAvailable = amountAmmoNeeded < ammo ? amountAmmoNeeded : ammo;

            reloadAmmo += ammoAvailable;
            ammo -= ammoAvailable;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (ammo > 0)
            {
                animator.SetBool("isShoot", !animator.GetBool("isShoot"));
                HitEnemy();
                ammo = Mathf.Clamp(ammo - 1, 0, maxAmmo);
            }

        }

    }
    void FixedUpdate()
    {
        inputX = Input.GetAxis("Horizontal") * playerSpeed;   //Movement of the player in horizantal direction 
        inputz = Input.GetAxis("Vertical") * playerSpeed;     //Movement of the player in vertical direction 

        transform.position += new Vector3(inputX, 0f, inputz);


        float mouseX = Input.GetAxis("Mouse X") * playerRotationSpeed;    //Using mouse to move the player in horizantal direction
        float mouseY = Input.GetAxis("Mouse Y") * playerRotationSpeed;    //Using mouse to move the player in vertical direction

        playerRotation = Quaternion.Euler(0f, mouseX, 0f);
        camRotation = Quaternion.Euler(-mouseY, 0f, 0f);


        transform.localRotation = playerRotation * transform.localRotation;
        cam.transform.localRotation = camRotation * cam.transform.localRotation; // Rotation of the camera

    }

    public void HitEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(bulletLaunch.position, bulletLaunch.forward, out hit, 100f))
        {
            GameObject hitEnemy = hit.collider.gameObject;
            if (hitEnemy.gameObject.tag == "Enemy")
            {
                Destroy(hitEnemy);
                print("Enemy hit");
                hitEnemy.GetComponent<EnemyController>().EnemyDead();
            }
        }

       

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ammo" && ammo < maxAmmo)
        {
           
            ammo = Mathf.Clamp(ammo + 10, 0, maxAmmo);
           
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Medical" && medical < maxMedical)
        {
            
            medical = Mathf.Clamp(medical + 10, 0, maxMedical);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Lava")
        {
            
            medical = Mathf.Clamp(medical - 10, 0, maxMedical);
            //Debug.Log("Medical: "+medical);
        }
    }

    public void TakeHit(float value)
    {

        medical = (int)(Mathf.Clamp(medical - value, 0, maxMedical));// medical = health
        print("Health " + medical);
        if (medical <= 0)
        {
            Vector3 position = new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(this.transform.position), transform.position.z);
            GameObject tempSteve = Instantiate(playerPrefabs, position, this.transform.rotation);
            tempSteve.GetComponent<Animator>().SetTrigger("Death");
            GameStart.isGameOver = true;
            Destroy(this.gameObject);
        }

    }
}


   
   

   
    


