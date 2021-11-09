using UnityEngine;
using System.Collections;

public class Bryce : MonoBehaviour 
{
    public string Name { get; private set; }

	// Use this for initialization
	void Start () 
    {
        Name = "Bryce";
        gameObject.AddComponent<Boost>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
