using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour {

	public MonsterController2D controller;
	public Animator animator;
	public GameObject player;
	public GameObject darkness;

	private DarknessController darknessScript;

	public float runSpeed = 20f;

	private bool targeting;

	private float playerDistance;
	private float lightRadius;

	private float horizontalMove = 0f;
	private bool jump = false;


	void Start() {
		darknessScript = darkness.GetComponent<DarknessController>();
		Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
		Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
		Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
		Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
	}

	// Update is called once per frame
	void Update () {
		Vector3 target = player.transform.position;

		if (PlayerInRange(target)) {
			horizontalMove = (target.x-transform.position.x)/Mathf.Abs(target.x-transform.position.x);
			horizontalMove*=runSpeed;
		} else {
			horizontalMove = 0;
		}

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
	}

	bool PlayerInRange(Vector3 target) {
		playerDistance = Vector3.Distance(transform.position, target);
		lightRadius = darknessScript.scale*1.6f;

		if (playerDistance < lightRadius)
		{
			targeting = true;
		} else {
			targeting = false;
		}
		return targeting;
	}

	void OnCollisionStay2D(Collision2D collision) {
		// jump = false;
		if (collision.otherCollider == GetComponent<BoxCollider2D>() && collision.collider.GetType() == typeof(CompositeCollider2D)) {
			jump = true;
		}
	}

	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
		jump = false;
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
	}
}
