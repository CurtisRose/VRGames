using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {
	private NavMeshAgent agent;
	public Transform target;
	bool alive = true;
	float health = 100;
	private GameController gameController;
	AudioSource[] zombieSounds;
	int damage = 10;
	float attackDistance = 2.0f;
	float time;
	float attackSpeed = 2.0f;
	NavMeshPath path = null;
	public GameObject targetGameObject = null;
	public GameObject player = null;

	void Start () {
		//Debug.Log ("Zombie Spawned");
		this.gameController = GameController.GetInstance();
		agent = GetComponent<NavMeshAgent> ();
		zombieSounds = GetComponents<AudioSource> ();
		time = Time.realtimeSinceStartup;
		path = new NavMeshPath ();
	}

	/*void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag("Bullet")) {
			health -= 34;
			if (!zombieSounds [3].isPlaying) {
				zombieSounds [3].Play ();
			}
		}
	}*/

	public void SetHealth(int setHealth) {
		health = setHealth;
	}

	public void DoDamage(int damage) {
		if (!zombieSounds [3].isPlaying) {
			zombieSounds [3].Play ();
		}
		health -= damage;
	}

	void Update () {
		if (health > 0) {
			//Debug.Log ("Remaining Distance: " + Mathf.Abs((agent.transform.position - target.transform.position).magnitude));
			Vector3 pathLocation = player.transform.position;
			agent.SetDestination (pathLocation);
			Debug.Log (agent.pathEndPosition == pathLocation);
			Debug.Log (agent.pathEndPosition);
			Debug.Log (pathLocation);
			if (agent.pathEndPosition != pathLocation) {
				//Debug.Log ("Player is not viable target");
				if (targetGameObject) {
					//Debug.Log ("Continue to obstacle");
					pathLocation = targetGameObject.transform.position;
					agent.SetDestination (pathLocation);
				} else {
					//Debug.Log ("Find new target obstacle");
				}
			} else {
				//Debug.Log ("Player is viable target");
			}
			if (agent.pathEndPosition == pathLocation) {
				Debug.Log ("Continuing on path");
				if (Mathf.Abs ((agent.transform.position - pathLocation).magnitude) > attackDistance) {
					Debug.Log ("Walking");
					GetComponent<Animator> ().Play ("walk");
					agent.SetDestination (pathLocation);
					agent.Resume ();
				} else {
					agent.Stop ();
					Debug.Log ("Attacking");
					GetComponent<Animator> ().Play ("attack");
					if (Time.realtimeSinceStartup - time > attackSpeed) {
						time = Time.realtimeSinceStartup;
						//Debug.Log ("Doing Damage");
						if (targetGameObject) {
							targetGameObject.GetComponentInParent<Obstacle> ().Dagmage (damage);
						} else {
							target.gameObject.GetComponentInParent<PlayerController> ().DoDamage (damage);
						}
					}
				}
			} else {
				Debug.Log ("Calculating new obstacle target");
				float pathLength = -1;
				Obstacle finalObstacle = null;
				foreach (Obstacle obstacle in Object.FindObjectsOfType<Obstacle> ()) {
					agent.SetDestination (obstacle.transform.position);
					float temp = 0;
					foreach (Vector3 corner in agent.path.corners) {
						temp += corner.magnitude;
					}
					if (pathLength == -1 || pathLength > temp) {
						pathLength = temp;
						finalObstacle = obstacle;
					}
				}
				if (finalObstacle) {
					targetGameObject = finalObstacle.gameObject;
					agent.SetDestination (targetGameObject.transform.position);
				} else {
					agent.Stop ();
				}
			}
		} else {
			Destroy (agent);
			GetComponent<Animator> ().Play ("back_fall");
			if (alive) {
				//Debug.Log ("Zombie Killed");
				zombieSounds[4].Play();
				gameController.KillZombie ();
				if (GetComponent<Collider> ()) {
					GetComponent<Collider> ().enabled = false;
				}
				if (GetComponentInChildren<Collider> ()) {
					GetComponentInChildren<Collider> ().enabled = false;
				}
				Destroy (gameObject, 2);
				alive = false;
			}
		}
	}
}
