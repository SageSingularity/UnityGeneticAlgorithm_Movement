using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
	// The brain is the controller of the character, sits between the character and the DNA
	// Reads the DNA and then tells the character what to do

	// DNA Length is the possible 'Options' for DNA to use/pass on
	public int DNALength = 1;
	// Timer to ensure death after a period of time
	public float timeAlive;
	public Cromosome cromosome;
	// Accessed to enable jumping
	public Rigidbody rb;
	private Vector3 m_Move;
	private bool m_Jump;
	bool alive = true;
	Renderer rend;

	void OnCollisionEnter(Collision obj)
	{
		// Kills the entity if it hits anything that kills entities
		if (obj.gameObject.tag == "dead") {
			alive = false;
			rend.material.SetColor ("_Color", Color.red);
		}
	}

	public void Init()
	{
		// Is called when initializing the game object from the prefab, initializes DNA
		// 0 Forward
		// 1 Back
		// 2 Left
		// 3 Right
		// 4 Jump
		cromosome = new Cromosome(DNALength, 4);
		timeAlive = 0;
		alive = true;
		rb = GetComponent<Rigidbody> ();
		rend = GetComponent<Renderer> ();
	}

	public void MovementFunction( Vector3 movementDirection, bool jump, Rigidbody rb, float thrust){
		transform.Translate (movementDirection.x * Time.deltaTime *2, movementDirection.y * Time.deltaTime*2, movementDirection.z * Time.deltaTime*2);
		//rb.AddForce(transform.up * thrust, ForceMode.Impulse );
	}

	// Fixed update is called in sync with physics
	private void FixedUpdate()
	{
		// The brain needs to be able to give commands to the entity
		// This boils down to DNA linking to actual commands

		float h = 0;
		float v = 0;

		if (cromosome.GetGene (0) == 0)
			v = 1;
		else if (cromosome.GetGene (0) == 1)
			v = -1;
		else if (cromosome.GetGene (0) == 2)
			h = -1;
		else if (cromosome.GetGene (0) == 3)
			h = 1;
		m_Jump = false; // This isn't used for now
		// Uses the forward and right vector positive/negative values to control movement
		m_Move = v * Vector3.forward + h * Vector3.right;
		MovementFunction (m_Move, m_Jump, rb, 1.0f);
		// Force entity to only jump once per command
		if (alive)
			timeAlive += Time.deltaTime;
	}

}
