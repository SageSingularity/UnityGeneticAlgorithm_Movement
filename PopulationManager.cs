using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// System.Linq adds useful techniques for sorting lists
using System.Linq;

public class PopulationManager : MonoBehaviour {
	// Manages the population overall
	// - Assigns random values per entity spawned initially
	// - Instantiates each entity
	// - Breeds the 50% most fit individuals to generate the next generation

	public GameObject botPrefab;
	public int populationSize = 50;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;
	// How long each one is allowed to be alive
	public float trialTime = 5;
	int generation = 1;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.black;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0, 0, 140, 140), "Stats", guiStyle);
		GUI.Label (new Rect (10, 25, 200, 30), "Generation: " + generation, guiStyle);
		GUI.Label (new Rect (10, 50, 200, 30), "Population: " + population.Count, guiStyle);
		GUI.EndGroup ();
	}

	void Start () {
		// Create the initial population; only necessary for starting out
		for (int i = 0; i < populationSize; i++) {
			Vector3 startingPos = new Vector3 (this.transform.position.x + Random.Range (-2, 2),
				                      this.transform.position.y,
				                      this.transform.position.z + Random.Range (-2, 2));
			GameObject bot = Instantiate (botPrefab, startingPos, this.transform.rotation);
			bot.GetComponent<Brain> ().Init (); // Call the brain's init function
			population.Add (bot);
		}
	}

	GameObject Breed(GameObject parent1, GameObject parent2)
	{
		// Utility function for creating each individual child entity
		Vector3 startingPos = new Vector3 (this.transform.position.x + Random.Range (-2, 2),
			this.transform.position.y,
			this.transform.position.z + Random.Range (-2, 2));
		GameObject offspring = Instantiate (botPrefab, startingPos, this.transform.rotation);
		Brain bot = offspring.GetComponent<Brain> ();
		if (Random.Range (0, 100) == 1) { // Mutation occurs 1 in 100
			bot.Init ();
			bot.cromosome.Mutate ();
		} else {
			bot.Init ();
			bot.cromosome.Combine (parent1.GetComponent<Brain> ().cromosome, parent2.GetComponent<Brain> ().cromosome);
		}
		return offspring;

	}

	void BreedNewPopulation()
	{
		List<GameObject> sortedList = population.OrderBy (o => o.GetComponent<Brain> ().timeAlive).ToList ();
		population.Clear ();
		// In this case, this just creates a copy of the parents
		// The fittest individuals survive to basically clone themselves
		for (int i = (int)(sortedList.Count / 2.0f); i < sortedList.Count - 1; i++) {
			population.Add (Breed (sortedList [i], sortedList [i + 1]));
			population.Add (Breed (sortedList [i + 1], sortedList [i]));
		}

		// Destroy all parents of the previous population
		for (int i = 0; i < sortedList.Count; i++) {
			Destroy (sortedList [i]);
		}
		generation++;
	}

	void Update() {
		elapsed += Time.deltaTime;
		if (elapsed >= trialTime) {
			BreedNewPopulation ();
			elapsed = 0;
		}
	}
}
