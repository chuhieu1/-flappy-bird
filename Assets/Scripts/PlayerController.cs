using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float thrust, minTiltSmooth, maxTiltSmooth, hoverDistance, hoverSpeed;
	private bool start;
	private float timer, tiltSmooth, y;
    private Rigidbody2D playerRigid;
    private Quaternion downRotation, upRotation;
	private float velocity, time;

	void Start () {
		tiltSmooth = maxTiltSmooth;
        playerRigid = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler (0, 0, -90);
		upRotation = Quaternion.Euler (0, 0, 35);
	}

	void Update () {
		if (!start) {
			// Hover the player before starting the game
			timer += Time.deltaTime;
			y = hoverDistance * Mathf.Sin (hoverSpeed * timer);
			transform.localPosition = new Vector3 (0, y, 0);
		} else {
			// Rotate downward while falling
			transform.rotation = Quaternion.Lerp (transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
		}
		// Limit the rotation that can occur to the player
		transform.rotation = new Quaternion (transform.rotation.x, transform.rotation.y, Mathf.Clamp (transform.rotation.z, downRotation.z, upRotation.z), transform.rotation.w);
	}

	void LateUpdate () {
		if (GameManager.Instance.GameState ()) {
			if (Input.GetMouseButtonDown (0)) {
				if(!start){
					// This code checks the first tap. After first tap the tutorial image is removed and game starts
					start = true;
					GameManager.Instance.GetReady ();
					GetComponent<Animator>().speed = 2;
				}
				//playerRigid.gravityScale = 1f;
				tiltSmooth = minTiltSmooth;
				transform.rotation = upRotation;
				//playerRigid.velocity = Vector2.zero;
				// Push the player upwards
				//playerRigid.AddForce (Vector2.up * thrust);
				time = 0;
				velocity = 0.02f;
				SoundManager.Instance.PlayTheAudio("Flap");
			}
		}
		//if (playerRigid.velocity.y < -1f) {
		//if (velocity < -1f)
		//{
		//	// Increase gravity so that downward motion is faster than upward motion
		//	tiltSmooth = maxTiltSmooth;
		//	//playerRigid.gravityScale = 2f;
		//	velocity = -2f;
		//}
		if (velocity > 0)
        {
			time += Time.deltaTime;
			if (time > 0.5)
            {
				velocity = -0.04f;
				tiltSmooth = maxTiltSmooth;
			}
        }
		else
        {
			tiltSmooth = maxTiltSmooth;
		}
		playerRigid.velocity = Vector2.zero;
		transform.position += new Vector3(0f, velocity);
    }

	void OnTriggerEnter2D (Collider2D col) {
		if (col.transform.CompareTag ("Score")) {
			Destroy (col.gameObject);
			GameManager.Instance.UpdateScore ();
		} else if (col.transform.CompareTag ("Obstacle")) {
			// Destroy the Obstacles after they reach a certain area on the screen
			foreach (Transform child in col.transform.parent.transform) {
				child.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			}
			KillPlayer ();
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.transform.CompareTag ("Ground")) {
            playerRigid.simulated = false;
            KillPlayer ();
			transform.rotation = downRotation;
		}
	}

	public void KillPlayer () {
		GetComponent<Animator>().Play("PlayerDead");
		GameManager.Instance.EndGame ();
		velocity = 0;
		//playerRigid.velocity = Vector2.zero;
		// Stop the flapping animation
		//GetComponent<Animator> ().enabled = false;
	}

}