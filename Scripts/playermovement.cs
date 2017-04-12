using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class playermovement : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    public Rigidbody rb;
    public float fly;

    Vector3 axis0, axis1, axis2;
	// Use this for initialization
	void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        axis0 = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0); //Joystick
        axis1 = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis1); //Front Trigger
        axis2 = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis2); //Side Trigger

        if(device.GetPress(Valve.VR.EVRButtonId.k_EButton_A))
        {
            rb.velocity = Vector3.zero;
        }
        if(device.GetPress(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) {
            rb.MovePosition(Vector3.zero);
        }
        if(axis0 + axis1 + axis2 != Vector3.zero)
        {
          /*  if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                transform.Rotate(0, (axis0.x)*fly, 0);*/
              rb.MovePosition(transform.position + new Vector3(axis0.x*fly, (axis1.x-axis2.x)*fly, axis0.y*fly));
        }
    }
}
