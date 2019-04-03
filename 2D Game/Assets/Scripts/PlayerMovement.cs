using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	// public AudioSource noteSound;

	public PhysicsMaterial2D friction;
	public PhysicsMaterial2D nofriction;

	public float runSpeed = 40f;

	private int notesTotal;
	public int notesGathered;
	private GameObject noteUI;
	private GameObject uiBack;

	float horizontalMove = 0f;
	bool jump = false;
	public bool dead;
	public bool levelCompleted;


	private CircleCollider2D feet;

	void Start() {
		feet = GetComponent<CircleCollider2D>();
		feet.sharedMaterial = nofriction;
		notesGathered = 0;
		dead = false;

		notesTotal = GameObject.FindGameObjectsWithTag("Note").Length;
		noteUI = GameObject.Find("NoteUI");
		levelCompleted = false;
	}

	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (horizontalMove == 0) {
			feet.sharedMaterial = friction;
		} else {
			feet.sharedMaterial = nofriction;
		}

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}

		if (transform.position.y < -30f) {
			dead = true;
		}

		if (dead) {
			transform.Rotate(0f, 0f, 1f, Space.Self);
			GameObject.Find("Main Camera").GetComponent<com.cortastudios.DynamicColorCorrection.ColorCurvesManager>().Factor+=0.05f;
		}

	}

	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
	}

	void OnCollisionEnter2D(Collision2D collision) { //monster kill
		// jump = false;
		if (collision.gameObject.tag == "Monster") {
			dead = true;
		} 
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (collider.gameObject.tag == "Exit") {
			if (notesGathered==notesTotal) {
				levelCompleted = true;
			} else {
				Color col = noteUI.GetComponent<Text>().color;
				if (Time.frameCount%30<=14) {
					noteUI.GetComponent<Text>().color = new Color(Mathf.Clamp(col.r+0.03f, 0f, 1f), Mathf.Clamp(col.r+0.03f, 0f, 1f), Mathf.Clamp(col.r+0.03f, 0f, 1f), 1f);
				} else {
					noteUI.GetComponent<Text>().color = new Color(Mathf.Clamp(col.r-0.03f, 0f, 1f), Mathf.Clamp(col.r-0.03f, 0f, 1f), Mathf.Clamp(col.r-0.03f, 0f, 1f), 1f);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "Exit") {
			noteUI.GetComponent<Text>().color = new Color(0.54f, 0.54f, 0.54f, 1f);
		}
	}

	void FixedUpdate ()
	{
		// Move our character
		if (!dead) {
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		}

		jump = false;

		noteUI.GetComponent<Text>().text = "Notes gathered: "+notesGathered.ToString()+" / "+notesTotal.ToString();
	}
}
