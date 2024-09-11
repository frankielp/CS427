using UnityEngine;
using System.Collections;

/// <summary>
/// Damage class handler.
/// </summary>
public class Damage : MonoBehaviour {
	
	public float damageAmount = 10.0f;
	
	public bool damageOnTrigger = true;
	public bool damageOnCollision = false;
	public bool continuousDamage = false;
	public float continuousTimeBetweenHits = 0;

	// Variables dealing with exploding on impact (area of effect).
	public bool destroySelfOnImpact = false;	
	public float delayBeforeDestroy = 0.0f;
	public GameObject explosionPrefab;

	private float savedTime = 0;

	/// <summary>
	/// Called when the Collider "other" enters the trigger.
	/// Used for things like bullets, which are triggers.
	/// See https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html
	/// </summary>
	void OnTriggerEnter(Collider collision)	{
		if (damageOnTrigger) {
			// If the player got hit with it's own bullets, ignore it.
			if (this.tag == "PlayerBullet" && collision.gameObject.tag == "Player")
				return;
			if (this.tag == "Water")
			{
				Debug.Log("Trigger with water detected!");

				// Liệt kê tất cả các thông tin của đối tượng va chạm
				Debug.Log("Collided with: " + collision.gameObject.name);
				Debug.Log("Tag of collided object: " + collision.gameObject.tag);
				Debug.Log("Layer of collided object: " + collision.gameObject.layer);
			}
	
				// If the hit object has the Health script on it, deal damage.
				if (collision.gameObject.GetComponent<Health> () != null) {
				if (this.tag == "Water")
					Debug.Log("YES");
				collision.gameObject.GetComponent<Health> ().ApplyDamage(damageAmount);
				// Destroy the object whenever it hits something.
				if (destroySelfOnImpact) {
					Destroy (gameObject, delayBeforeDestroy);
				}
			
				// If explosion prefab is not null instantiates it.
				if (explosionPrefab != null) {
					Instantiate (explosionPrefab, transform.position, transform.rotation);
				}
			}
            else
            {
				if (this.tag == "Water")
					Debug.Log("NULL");
            }
        }
	}

	/// <summary>
	/// Called when this collider/rigidbody has begun touching another rigidbody/collider.
	/// This is used for things that explode on impact and are NOT triggers.
	/// See https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter.html
	/// </summary>
	void OnCollisionEnter(Collision collision) {	
		if (damageOnCollision) {
			// If the player got hit with it's own bullets, ignore it.
			if (this.tag == "PlayerBullet" && collision.gameObject.tag == "Player")	
				return;

			// If the hit object has the Health script on it, deal damage.
			if (this.tag == "Water")
			{
				Debug.Log("Collide with water detected!");

				// Liệt kê tất cả các thông tin của đối tượng va chạm
				Debug.Log("Collided with: " + collision.gameObject.name);
				Debug.Log("Tag of collided object: " + collision.gameObject.tag);
				Debug.Log("Layer of collided object: " + collision.gameObject.layer);
			}
		
			if (collision.gameObject.GetComponent<Health> () != null) {
				collision.gameObject.GetComponent<Health> ().ApplyDamage (damageAmount);
			
				// Destroy the object whenever it hits something.
				if (destroySelfOnImpact) {
					Destroy (gameObject, delayBeforeDestroy);
				}
			
				if (explosionPrefab != null) {
					Instantiate (explosionPrefab, transform.position, transform.rotation);
				}
			}
		}
	}

	/// <summary>
	/// Called once per frame for every collider/rigidbody that is touching rigidbody/collider.
	/// This is used for damage over time things.
	/// See https://docs.unity3d.com/ScriptReference/Collider.OnCollisionStay.html
	/// </summary>
	void OnCollisionStay(Collision collision) {
		if (continuousDamage) {
			// It is only triggered if whatever it hits is the player.
			if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Health> () != null) {
				if (Time.time - savedTime >= continuousTimeBetweenHits) {
					savedTime = Time.time;
					collision.gameObject.GetComponent<Health> ().ApplyDamage (damageAmount);
				}
			}
		}
	}
}