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
	float attackSpeed = 1.5f;
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
			//Debug.Log ("====================");
			if ((agent.pathEndPosition.magnitude - pathLocation.magnitude) < attackDistance && agent.CalculatePath (pathLocation, path)) {
				//Debug.Log ("Following path to player");
				if (Mathf.Abs ((agent.transform.position - pathLocation).magnitude) > attackDistance) {
					bool attacking = false;
					foreach (Obstacle obstacle in GameObject.FindObjectsOfType<Obstacle>()) {
						if (Mathf.Abs ((transform.position - obstacle.transform.position).magnitude) < attackDistance) {
							attacking = true;
							agent.Stop ();
							//Debug.Log ("Attacking Obstacle");
							GetComponent<Animator> ().Play ("attack");
							if (Time.realtimeSinceStartup - time > attackSpeed) {
								time = Time.realtimeSinceStartup;
								//Debug.Log ("Doing Damage to Obstacle");
								obstacle.GetComponent<Obstacle> ().Damage (damage);
							}
						}
					}
					if (!attacking) {
						//Debug.Log ("Walking");
						GetComponent<Animator> ().Play ("walk");
						agent.SetDestination (pathLocation);
						agent.Resume ();
					}
				} else {
					agent.Stop ();
					//Debug.Log ("Attacking Player");
					GetComponent<Animator> ().Play ("attack");
					if (Time.realtimeSinceStartup - time > attackSpeed) {
						time = Time.realtimeSinceStartup;
						//Debug.Log ("Doing Damage to Player");
						target.gameObject.GetComponentInParent<PlayerController> ().DoDamage (damage);
					}
				}
			} else {
				//Debug.Log ("No Path to Player was found");
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