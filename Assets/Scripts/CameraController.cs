using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //mario transform 
    public Transform player;
    //gameobject that indicates end of map
    public Transform endLimit;
    //initial x-offset between camera and mario
    private float offset;
    //smallest x-coordinate of the camera
    private float startX;
    //largest x-coordinate of the camera
    private float endX;
    private float viewportHalfWidth;

    // Start is called before the first frame update
    void Start()
    {
        //get coordinate of the bottomleft of the viewport
        //z doesnt matter since camera is orthographic
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);
        offset = this.transform.position.x - player.position.x;
        startX = this.transform.position.x;
        endX = endLimit.transform.position.x - viewportHalfWidth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame, camera constantly follows player unless it reached end of game map
    void Update()
    {
        float desiredX = player.position.x + offset;
        //check if desiredX is within startX and endX
        if (desiredX > startX && desiredX < endX)
        {
            this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
        }
    }
}
