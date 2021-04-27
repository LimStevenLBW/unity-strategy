﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public bool isControlEnabled;
    
    public float panSpeed = 120.0f;
    public float jumpSpeed = 370.0f; //Unused?
    public float panBorderThiccness = 5.0f;
    public float scrollDistance = 25.0f;

    public float cameraYMinPos = 30;
    public float cameraYMaxPos = 200;

    public float cameraXMinPos = -200;
    public float cameraXMaxPos = 200;

    public float cameraZMinPos = -200;
    public float cameraZMaxPos = 200;

    private float scrollSpeed;
    private float scroll;
    private float cameraZoomPosition;


    // Start is called before the first frame update
    void Start()
    {
        cameraZoomPosition = transform.position.y;
        isControlEnabled = true;
    }

    // Update is called once per frame
    // Handles Keyboard and Mouse Panning of Camera
    void Update()
    {
        if (isControlEnabled)
        {
            Vector3 pos = transform.position;


            if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThiccness)
            {
                if(pos.x > cameraXMinPos) pos.x -= panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThiccness)
            {
                if (pos.x < cameraXMaxPos) pos.x += panSpeed * Time.deltaTime;
            }
            
            if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThiccness)
            {
                if (pos.z < cameraZMaxPos) pos.z += panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThiccness)
            {
                if (pos.z > cameraZMinPos) pos.z -= panSpeed * Time.deltaTime;
            }


            transform.position = pos;

            //Check which direction the user is scrolling and store it
            scroll = Input.GetAxis("Mouse ScrollWheel");
            //Zoom In
            if (scroll > 0)
            {
                // cameraZoomPosition = transform.position.y;
                cameraZoomPosition -= scrollDistance;
            }
            else if (scroll < 0) //Zoom Out
            {
                // cameraZoomPosition = transform.position.y;
                cameraZoomPosition += scrollDistance;
            }

            //Restrict the camera y position
            cameraZoomPosition = Mathf.Clamp(cameraZoomPosition, cameraYMinPos, cameraYMaxPos);

            //Check if the current position matches the destination height
            if (Mathf.Abs(cameraZoomPosition - transform.position.y) > 1)
            {
                //Speed will deaccelerate as camera approaches target position
                scrollSpeed = (cameraZoomPosition - transform.position.y) * 5f;
                transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime, Space.World);
            }

        }

    }

    /* Quickly pans the camera in the direction of the given vector and sets it behind the target object
     * Disables user camera control while in effect
     */
    public IEnumerator CameraJump(Vector3 targetPosition)
    {
        targetPosition.y += 50;
        targetPosition.z -= 50;
        isControlEnabled = false;
        float smoothTime = 0.3F;
        Vector3 velocity = Vector3.zero;

        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));

        //Vector3.SqrMagnitude(targetPosition - transform.position) > 100.0f

        //While close enough, continue to move the camera
        while (Vector3.Distance(transform.position, targetPosition) > 3f)
        {
            //Debug.Log((Vector3.SqrMagnitude(targetPosition - transform.position)));
            //Debug.Log(Vector3.Distance(transform.position, targetPosition));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, jumpSpeed * Time.deltaTime);
            //  transform.Translate(Space.World);
            //Wait a frame and repeat
            yield return null;
        }

        //Reset camera target position to prevent auto scrolling after the jump
        cameraZoomPosition = transform.position.y;
        isControlEnabled = true;
    }
}


    


