using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interference : MonoBehaviour {

	public float speed = 30;

	Vector3 targetpos;

	// Use this for initialization
	void Start () {
		targetpos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		targetpos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10-.48f);
		targetpos = Camera.main.ScreenToWorldPoint(targetpos);

		transform.position = Vector3.MoveTowards (transform.position, targetpos, speed * Time.deltaTime);

		//	this.transform.position = Camera.current.ScreenPointToRay (Input.mousePosition);
		StartCoroutine(explode());

		if(Input.GetKey(KeyCode.Q)){
			UnityEngine.SceneManagement.SceneManager.LoadScene (0);
		}
	}

	public float explosionForce = 4;


	private IEnumerator explode()
	{
		// wait one frame because some explosions instantiate debris which should then
		// be pushed by physics force
		yield return null;

		float multiplier = .1f;//GetComponent<ParticleSystemMultiplier>().multiplier;

		float r = 10 *multiplier;
		var cols = Physics.OverlapSphere(transform.position, r);
		var rigidbodies = new List<Rigidbody>();
		foreach (var col in cols)
		{
			if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
			{
				rigidbodies.Add(col.attachedRigidbody);
			}
		}
		foreach (var rb in rigidbodies)
		{
			rb.AddExplosionForce(explosionForce*multiplier, transform.position, r, 1*multiplier, ForceMode.Impulse);
		}
	}
}