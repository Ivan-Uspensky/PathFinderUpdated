using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

	public float moveSpeed = 2.5f;
	public float minWeaponRange = 1f;
	public float maxWeaponRange = 2f;

	public bool displayGridGizmos;

	PlayerController controller;
	Animator animator;
	GunController gunController;

	Vector3 offsetPosition;
	float nextFloor;
	Vector2 rotation;
	Vector2 direction;

	void Start () {
		controller = GetComponent<PlayerController> ();
		animator = GetComponentInChildren<Animator>();
		gunController = GetComponent<GunController>();
	}
	
	void Update () {
		// get left or right directions
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"), 0);
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		// rotate character left or right
		direction = (Vector2)moveInput.normalized;
		rotation = new Vector2( transform.eulerAngles.x, transform.eulerAngles.y );
		if (direction != Vector2.zero && (direction != Vector2.right && rotation.y == 0)) {
			transform.eulerAngles = Vector3.up * 180;
		}
		if (direction != Vector2.zero && (direction == Vector2.right && rotation.y != 0)) {
			transform.eulerAngles = Vector3.up * 0;
		}
		// move player
		controller.Move (moveVelocity);
		// play animation
		float animationSpeedPercent = ((direction != Vector2.zero) ? 0.5f : 0) * moveInput.normalized.magnitude;
		animator.SetFloat("Blend", animationSpeedPercent);
		// shoot
		if (Input.GetButton("Shoot")) {
			gunController.Shoot(gunController.weaponHold.position, transform.eulerAngles.y);
			animator.SetFloat("Blend", 1);
		}
	}

	void OnDrawGizmos() {
        if (displayGridGizmos) {
			Gizmos.color = new Color (1, 1, 0, .3f);
        	Gizmos.DrawCube(transform.position, new Vector3(minWeaponRange, .5f, .08f));
			Gizmos.color = new Color (0, 1, 1, .3f);
			Gizmos.DrawCube(transform.position, new Vector3(maxWeaponRange, .5f, .05f));
		}
    }
}
