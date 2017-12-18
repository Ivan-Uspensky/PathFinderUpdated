using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(GunController))]
public class Unit : MonoBehaviour {
	
	public Transform target;
	public float speed = 20;

	CustomGrid grid;
	GameObject go;
	GunController gunController;

	Vector2[] path;
	int targetIndex;

	string state = "";

	Vector2 direction;
	Vector2 rotation;

	void Awake() {
		go = GameObject.Find ("Pathfinder");
		grid = go.GetComponent<CustomGrid>();
		gunController = GetComponent<GunController>();
	}

	void Start() {
		StartCoroutine (RefreshPath ());
	}

	IEnumerator RefreshPath() {
		Vector2 targetPositionOld = (Vector2)target.position + Vector2.up; // ensure != to target.position initially
			
		while (true) {
			if (targetPositionOld != (Vector2)target.position) {
				targetPositionOld = (Vector2)target.position;

				path = Pathfinding.RequestPath (transform.position, target.position);
				StopCoroutine ("FollowPath");
				StartCoroutine ("FollowPath");
			}

			yield return new WaitForSeconds (.25f);
		}
	}
		
	IEnumerator FollowPath() {
		if (path.Length > 0) {
			targetIndex = 0;
			Vector2 currentWaypoint = path [0];

			while (true) {
				if ((Vector2)transform.position == currentWaypoint) {
					targetIndex++;
					if (targetIndex >= path.Length) {
						yield break;
					}
					currentWaypoint = path [targetIndex];
				}

				//rotation
				direction = (currentWaypoint - (Vector2)transform.position).normalized;
				rotation = new Vector2( transform.eulerAngles.x, transform.eulerAngles.y );

				if (direction != Vector2.right && rotation.y == 0) {
					transform.eulerAngles = Vector3.up * 180;
				}
				if (direction == Vector2.right && rotation.y != 0) {
					transform.eulerAngles = Vector3.up * 0;
				} 

				// transform.position = Vector2.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
				transform.position = Vector3.MoveTowards (transform.position, new Vector3(currentWaypoint.x, currentWaypoint.y, -0.5f), speed * Time.deltaTime);
				yield return null;

			}
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				//Gizmos.DrawCube((Vector3)path[i], Vector3.one *.5f);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
