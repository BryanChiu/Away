using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteContoller : MonoBehaviour
{
	public AudioSource noteSound;
	private float timer;
	private bool dying;

    // Start is called before the first frame update
    void Start()
    {
    	timer = 0f;
    	dying = false;
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
    	if (collider.gameObject.tag == "Player" && collider.GetType() == typeof(CircleCollider2D) && !dying) {
    		collider.gameObject.GetComponent<PlayerMovement>().notesGathered++;
    		noteSound.Play();
    		gameObject.GetComponent<Renderer>().enabled = false;
    		dying = true;
    	}
    }

    // Update is called once per frame
    void Update()
    {
    	if (dying) {
    		timer+=Time.deltaTime;
    		if (timer>1f) {
    			Destroy(gameObject);
    		}
    	}
        
    }
}
