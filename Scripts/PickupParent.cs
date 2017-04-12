using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))] //Fixes Null Point Reference
public class PickupParent : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    public Rigidbody sphereRigidBody;
    public float lift;

    Vector3 axis0, axis1, axis2;

    //When script is active
    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        //sphereRigidBody = GetComponent<Rigidbody>();
    }

    //FixedUpdate is called every physics event
    void FixedUpdate() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

      //  Debug.Log(device.GetState().ulButtonPressed);
      //Logging Purposes
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'Touch' on the Trigger");
            //  device.TriggerHapticPulse(700);
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You have activated TouchDown on the Trigger");
        }

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You have activated TouchUp on the Trigger");
        }


        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'Press' on the Trigger");
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You have activated PressDown on the Trigger");
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You have activated PressUp on the Trigger");
        }
        axis0 = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0); //Joystick
        axis1 = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis1); //Front Trigger
        axis2 = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis2); //Side Trigger
        /*
         * Alt method for presses:
         * device.GetState().ulButtonPressed == 128 (x)
         * device.GetState().ulButtonPressed == 2 (y)
         */
        if (device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_A)) //X Button PressDown Event
        {
            Debug.Log("The X button was pressed.");
            //Pause Motion of sphere
            sphereRigidBody.useGravity = false;
            sphereRigidBody.isKinematic = true;
        }else if(device.GetPressUp(Valve.VR.EVRButtonId.k_EButton_A)) //X Button PressUp Event
        {
            Debug.Log("The X button was released");
            //Resume Motion of sphere
            sphereRigidBody.useGravity = true; 
            sphereRigidBody.isKinematic = false;

        }
        if(device.GetPress(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) //Y Button Press Event
        {
            Debug.Log("The Y button was pressed.");
            sphereRigidBody.velocity = Vector3.zero; //Stops movement
            sphereRigidBody.MovePosition(new Vector3(0f, 0.25f, 1f)); //Resets position of sphere
        }
        if (axis0 != Vector3.zero || axis2 != Vector3.zero) { //Saves memory
            //Debug.Log("axis0 = " + axis0);
            //sphereRigidBody.MovePosition(transform.position + axis0);
            sphereRigidBody.AddForce(new Vector3(axis0.x, axis2.x*lift, axis0.y)); //Moves ball based on joystick
        }

    }
    private void OnTriggerStay(Collider col)
    {
        
        Debug.Log("You have collided with " + col.name + " and activated OnTriggerStay.");
        if(device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You have collided with " + col.name + " while holding down touch.");
            col.gameObject.transform.SetParent(gameObject.transform);
            col.attachedRigidbody.isKinematic = true;
        } if ((device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) || device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))) 
        {
            col.gameObject.transform.SetParent(null);
            col.attachedRigidbody.isKinematic = false;

            tossObject(col.attachedRigidbody);
        }
    }

   void tossObject(Rigidbody rb)
    {
        Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
        if (origin != null)
        {
            rb.velocity = origin.TransformVector(device.velocity);
            rb.angularVelocity = origin.TransformVector(device.angularVelocity);
        }
        else
        {
            rb.velocity = device.velocity;
            rb.angularVelocity = device.angularVelocity;
        }
        }
}
