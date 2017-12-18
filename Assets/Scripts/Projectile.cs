using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	float speed = 10;
	public Transform Spark;

	void Update () {
		float moveDistance = Time.deltaTime * speed;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.right * moveDistance);
	}

	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
	}

	void CheckCollisions(float moveDistance) {
		Ray ray = new Ray (transform.position, transform.right);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit.collider, hit.point);
		}
	}

	void OnHitObject(Collider c, Vector3 hitPoint) {
		GameObject.Destroy (gameObject);
		Transform hitParticle = Instantiate(Spark, hitPoint, Quaternion.FromToRotation (Vector3.forward, -transform.right)) as Transform;
		Destroy(hitParticle.gameObject, 1f);
	}
}
