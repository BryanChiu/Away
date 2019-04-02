using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    public GameObject player;       //Public variable to store a reference to the player game object
    public Camera cam;
    private GameObject[] monsters;

    public float xOffset;
    public float yOffset;
    private Vector3 offset;       //Private variable to store the offset distance between the player and camera

    public float CameraSpeed;

    private Vector3 destination;

    private float scale;
    private float scaleVelocity;
    private float MAX_SIZE;
    private float MIN_SIZE;

    private Vector3 CharacterLastPosition;
    public float CharacterSpeed;

    // Use this for initialization
    void Start () 
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        xOffset = 0f;
        yOffset = 1f;
        offset = transform.position - player.transform.position;
        offset.x += xOffset;
        offset.y += yOffset;
        CameraSpeed = 0.04f;

        MAX_SIZE = 7f;
        MIN_SIZE = 4f;
        scaleVelocity = 0f;
        scale = 7f;

        CharacterLastPosition = player.transform.position;

        monsters = GameObject.FindGameObjectsWithTag("Monster");
    }
    

    void FixedUpdate() {
		CharacterSpeed = Mathf.Abs(player.transform.position.x - CharacterLastPosition.x)*50;
    	CharacterLastPosition = player.transform.position;
        bool playerTargeted = false;

        foreach (GameObject monst in monsters) {
            if (monst.GetComponent<MonsterMovement>().targeting) {
                playerTargeted = true;
            }
        }

        if (CharacterSpeed>0 && !playerTargeted) {
            if (scale<MAX_SIZE) {
                scaleVelocity = Mathf.MoveTowards(Mathf.Max(0f, scaleVelocity), 0.03f, 0.0005f);
                scale += scaleVelocity;
            } else {
                scaleVelocity = 0f;
            }
        } else {
            if (scale>MIN_SIZE) {
                scaleVelocity = Mathf.MoveTowards(Mathf.Min(0f, scaleVelocity), -0.06f, 0.001f);
                scale += scaleVelocity;
            } else {
                scaleVelocity = 0f;
            }
        }
        cam.orthographicSize = scale;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate () 
    {
        offset.x = xOffset + CharacterSpeed;
        offset.y = yOffset;
        Vector3 temp = offset;
        temp.x*=player.transform.localScale.x/Mathf.Abs(player.transform.localScale.x);
        destination = player.transform.position + temp;
        transform.position = Vector3.MoveTowards(transform.position, destination, CameraSpeed*Vector3.Distance(transform.position, destination));
    }
}
