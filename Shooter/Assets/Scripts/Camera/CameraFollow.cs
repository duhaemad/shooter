using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 20f;
    private Transform playerTarget;
    // Start is called before the first frame update
    private void Awake()
    {
        playerTarget = GameObject.Find("Robot").transform; // THE NAME IN THE HIRE panel 

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTarget)
        {
            Vector3 start = transform.position;
            Vector3 pos = new Vector3(playerTarget.position.x, playerTarget.position.y + 3f, playerTarget.position.z);
            //set the camera 3 points above the player 
            Vector3 end = Vector3.MoveTowards(start, pos, followSpeed * Time.deltaTime);
            end.z = start.z;
            transform.position = end;
        }
    }
}
