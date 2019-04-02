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

		// if (Input.GetButtonDown("Crouch"))
		// {
		// 	crouch = true;
		// } else if (Input.GetButtonUp("Crouch"))
		// {
		// 	crouch = false;
		// }

		if (transform.position.y < -30f) {
			dead = true;
		}

	}

	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
	}

	// public void OnCrouching (bool isCrouching)
	// {
	// 	animator.SetBool("IsCrouching", isCrouching);
	// }

	void OnCollisionEnter2D(Collision2D collision) { //monster kill
		// jump = false;
		if (collision.gameObject.tag == "Monster") {
			dead = true;
		} 
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (collider.gameObject.tag == "Exit") {
			levelCompleted = true;
		}
	}

	void FixedUpdate ()
	{
		// Move our character
		if (!dead) {
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		}

		jump = false;

		noteUI.GetComponent<Text>().text = "Notes gather: "+notesGathered.ToString()+" / "+notesTotal.ToString();
	}
}
