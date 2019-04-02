using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour {

	public MonsterController2D controller;
	public Animator animator;
	public GameObject player;
	public GameObject darkness;
	private AudioSource staticSound;

	private DarknessController darknessScript;

	public float runSpeed = 17f;

	public bool targeting;

	private float playerDistance;
	private float lightRadius;

	private float horizontalMove = 0f;
	private bool jump = false;


	void Start() {
		darknessScript = darkness.GetComponent<DarknessController>();
		staticSound = GetComponent<AudioSource>();
		// Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
		// Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
		// Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
		// Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
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

		staticSound.volume = Mathf.MoveTowards(staticSound.volume, Mathf.Lerp(0.0f, 1.0f, Mathf.InverseLerp(12f, 5f, playerDistance)), 0.02f);
		staticSound.pitch = Mathf.MoveTowards(staticSound.pitch, Mathf.Lerp(0.7f, 2.0f, Mathf.InverseLerp(12f, 5f, playerDistance)), 0.026f);
		staticSound.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", 1f / Mathf.Max(1.0f, staticSound.pitch));
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
		if (collision.otherCollider == GetComponent<BoxCollider2D>() && collision.collider.GetType() == typeof(PolygonCollider2D)) {
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
