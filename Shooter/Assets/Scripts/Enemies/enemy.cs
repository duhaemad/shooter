using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioManager;
   // private Direction dir = Direction.RIGHT;  //by defult we will move to the right side 

   // public GameObject missile; // the messile that we are firing from the robot 
    public ParticleSystem rightMuzzle, leftMuzzle, rightFire, leftFire;
   // public Transform leftArm, rightArm;
   // public Transform misslePoint; //the point will the missle spwaning 
    public float speed = 1f;
    public GameObject explosion;
    private bool canMove = true;
    private bool canShoot = true;

    //  private float jumpTimer = 0f;
    private ParticleSystem.EmissionModule right_Muzzle_Emission, left_Muzzle_Emission, right_Fire_Emission, left_Fire_Emission;
 
    private Rigidbody rigidbody;
  //  private ConstantForce constantForce;
    public Light leftLight, rightLight;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        audioManager = GetComponent<AudioSource>();
        anim.Play("Walk");
        
        right_Muzzle_Emission = rightMuzzle.emission;
        left_Muzzle_Emission = leftMuzzle.emission;

        left_Muzzle_Emission.rateOverTime = 0f;
        right_Muzzle_Emission.rateOverTime = 0f;

        right_Fire_Emission = rightFire.emission;
        left_Fire_Emission = leftFire.emission;

        // we do this becouse we can no edit the emmission directly

        left_Fire_Emission.rateOverTime = 0f;
        right_Fire_Emission.rateOverTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    void move()
    {
        if (canMove)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (!isGrounded() && checkFront())
            {
                //not grounded or 
                anim.Play("Idle");
                canMove = false;
                LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f, 0.5f).setOnComplete(completedMove);
            }

        }
        //if (isGrounded() || !checkFront())
        //{
        //    //not grounded or 
        //    anim.Play("Idle");
        //    canMove = false;
        //    LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f, 0.5f).setOnComplete(completedMove);
        //}
    }
    bool isGrounded()
    {
        bool y= Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f, Vector3.down, 0.1f);
        return y;
        //going to create a line from position in direction and the length
    }
    bool checkFront()
    {
        //if he does not have space to move he will turn back
        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.5f,transform.forward, 0.1f);
    }
    void completedMove()
    {
        anim.Play("Walk");
        canMove = true;

    }
}
