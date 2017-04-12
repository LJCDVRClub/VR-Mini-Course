using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ParentFixedJoint : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    FixedJoint fj;
    public Rigidbody rBJoint;

	// Use this for initialization
	void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
	}

    private void OnTriggerStay(Collider col)
    {
        Debug.Log("You have collided with " + col.name + " and activated OnTriggerStay");
        if (fj == null && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            fj = col.gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = rBJoint;
        } else if(fj != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            GameObject go = fj.gameObject;
            Rigidbody rB = go.gameObject.GetComponent<Rigidbody>();
            Object.Destroy(fj);
            fj = null; //Make sure to set to null
        }
    }
}
