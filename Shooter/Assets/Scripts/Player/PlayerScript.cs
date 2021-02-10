using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum Direction
{
    LEFT,
    RIGHT
}
public class PlayerScript : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioManager;
    private Direction dir = Direction.RIGHT;  //by defult we will move to the right side 

    public GameObject missile; // the messile that we are firing from the robot 
    public ParticleSystem rightMuzzle, leftMuzzle, rightFire, leftFire, boost;
    public Transform leftArm, rightArm;
    public Transform misslePoint; //the point will the missle spwaning 
    public float speed = 4f;

  //  private float jumpTimer = 0f;
    private ParticleSystem.EmissionModule right_Muzzle_Emission, left_Muzzle_Emission, right_Fire_Emission, left_Fire_Emission;
    private ParticleSystem.MainModule boost_Main_Emission;
    private Rigidbody rigidbody;
    private ConstantForce constantForce;
    public Light leftLight, rightLight;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        // anim.Play("Walk");
        audioManager = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        constantForce = rigidbody.GetComponent<ConstantForce>();  //constant force attatched on the rigid body to get it you have to use it

        right_Muzzle_Emission = rightMuzzle.emission;
        left_Muzzle_Emission = leftMuzzle.emission;

        left_Muzzle_Emission.rateOverTime = 0f;
        right_Muzzle_Emission.rateOverTime = 0f;

        right_Fire_Emission = rightFire.emission;
        left_Fire_Emission = leftFire.emission;

        // we do this becouse we can no edit the emmission directly

        left_Fire_Emission.rateOverTime = 0f;
        right_Fire_Emission.rateOverTime = 0f;


        boost_Main_Emission = boost.main;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (!LeanTween.isTweening(gameObject))
            {
                if (!isGrounded())
                    anim.Play("Walk");
                else
                    anim.Play("Idle");

                if (dir != Direction.LEFT)
                {
                    LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f, 0.3f).setOnComplete(TurnLeft); //lw el direction is not left hay3ml rotate to his axis not the world becouse of leentween
                    //rotate it on the Y axis 
                    //(gameobject,Y axis, 180 degree , for time 0.3 secs) and when this finished call funtion turnleft 
                }
                else
                {//if the dircetion is lrft move
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);

                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!LeanTween.isTweening(gameObject))
            {
                if (!isGrounded())
                    anim.Play("Walk");
                else
                    anim.Play("Idle");

                if (dir != Direction.RIGHT)
                {
                    LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f, 0.3f).setOnComplete(TurnRight); //lw el direction is not left hay3ml rotate to his axis not the world becouse of leentween
                    //rotate it on the Y axis 
                    //(gameobject,Y axis, 180 degree , for time 0.3 secs) and when this finished call funtion turnleft 
                }
                else
                {//if the dircetion is lrft move
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);

                }
            }
        }
        else
        {
            anim.Play("Idle");
        }
        if(Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.W))
        {
            rightArm.Rotate(Vector3.back * 200f * Time.deltaTime); //rotate (0,0,-1)
         leftArm.Rotate(Vector3.back * 200f * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            rightArm.Rotate(Vector3.forward * 200f * Time.deltaTime); //rotate (0,0,1)
            leftArm.Rotate(Vector3.forward * 200f * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.Z)) //z for fly 
        {
            constantForce.force = Vector3.zero;
            if (rigidbody.velocity.y < 4)
            {
                rigidbody.AddRelativeForce(Vector3.up*20);
                if (!boost_Main_Emission.loop)
                {
                    //if the back effect is not showing the partical effects 
                    boost.Play();
                    boost_Main_Emission.loop = true;
                }
            }
        }
        else //if we did not input the key?{ }
        {
            constantForce.force = new Vector3(0, -10f,0) ;
            boost_Main_Emission.loop = false;

        }
        if (Input.GetKey(KeyCode.X)||Input.GetMouseButtonDown(0))//if you press the left mouse button
        {
            if (!audioManager.isPlaying)
            {
                //if the audio manger is not playing will return false
                audioManager.Play();
                StartCoroutine("LightControl");

            }
            right_Muzzle_Emission.rateOverTime = left_Muzzle_Emission.rateOverTime = 10f;
            right_Fire_Emission.rateOverTime = left_Fire_Emission.rateOverTime = 30f;

        }
        else
        {
            audioManager.Stop();
            right_Muzzle_Emission.rateOverTime = left_Muzzle_Emission.rateOverTime = 0f;
            right_Fire_Emission.rateOverTime = left_Fire_Emission.rateOverTime = 0f;
            leftLight.intensity = rightLight.intensity = 0f;
            StopCoroutine("LightControl");
            //if we not press X 
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            LaunchMissle();
        }
    }


            
    

    bool isGrounded()
    {
        return Physics.Raycast(transform.position + transform.forward*0.4f+transform.up*0.1f,Vector3.down,0.1f);

        //going to create a line from position in direction and the length
    }
    void TurnLeft()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        dir = Direction.LEFT;
    }
    void TurnRight()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        dir = Direction.RIGHT;
    }
    IEnumerator LightControl()
    {
        while (true)
        {
            //while this coroutine is runing 
            leftLight.intensity = rightLight.intensity = 1f;
            yield return new WaitForSeconds(0.3f);
            leftLight.intensity = rightLight.intensity = 0f;
            yield return new WaitForSeconds(0.3f);
            //this will turn lights on and off every .3 secs
        }
    }

    void LaunchMissle()
    {
        if (!LeanTween.isTweening(gameObject))
        {
            Vector3 pos = transform.position; // get the current pos of the robot
            if (dir == Direction.RIGHT)
            {
                pos.x += 1;     //to spawn the rocket in front of the robot 
                pos.y += 1;

            }
            if (dir == Direction.LEFT)
            {
                pos.x -= 1;
                pos.y -= 1;

            }
            for(int i=0; i < 5; i++)
            {
                Vector3 orign = pos + Vector3.up * Random.Range(-1f, 1f) + Vector3.left * Random.Range(-1f, 1f);
                GameObject temp = Instantiate(missile, orign, Quaternion.AngleAxis(dir==Direction.RIGHT? 0f :180, Vector3.up))as GameObject;


                Vector3 tarPos = misslePoint.position + misslePoint.forward * 20f +
                    misslePoint.up * Random.Range(-1, 1); //where is target will go

                temp.SendMessage("LaunchMissle", tarPos); //will go and send message to the game object that means it will go to every single script attached on that game object and call the function lanch missile >



            }
        }
    }
}
