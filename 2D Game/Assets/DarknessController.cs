using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessController : MonoBehaviour
{
    public GameObject player;       //Public variable to store a reference to the player game object

    public float scale;
    private float scaleVelocity;
    private float MAX_RAD;
    private float MIN_RAD;

    private Vector3 CharacterLastPosition;
    public float CharacterSpeed;

    // Use this for initialization
    void Start () 
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        MAX_RAD = 6f;
        MIN_RAD = 1f;
        scaleVelocity = 0f;
        scale = 1f;

        CharacterLastPosition = player.transform.position;
    }
    

    void FixedUpdate() {
		CharacterSpeed = Mathf.Abs(player.transform.position.x - CharacterLastPosition.x)*50;
    	CharacterLastPosition = player.transform.position;

    	if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetButtonDown("Jump")) {
    		if (scale<MAX_RAD) {
	    		scaleVelocity = Mathf.MoveTowards(Mathf.Max(0f, scaleVelocity), 0.03f, 0.0005f);
	    		scale += scaleVelocity;
	    	} else {
	    		scaleVelocity = 0f;
	    	}
    	} else {
    		if (scale>MIN_RAD) {
	    		scaleVelocity = Mathf.MoveTowards(Mathf.Min(0f, scaleVelocity), -0.06f, 0.001f);
	    		scale += scaleVelocity;
	    	} else {
	    		scaleVelocity = 0f;
	    	}
    	}
    	transform.localScale = new Vector3(scale, scale, 1);
    }
}
