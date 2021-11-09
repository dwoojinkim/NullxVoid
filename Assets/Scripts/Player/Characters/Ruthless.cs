using UnityEngine;
using System.Collections;

public class Ruthless : MonoBehaviour 
{
    public string Name { get; private set; }
    
	// Use this for initialization
	void Start () 
    {
        Name = "Ruthless";
        gameObject.AddComponent<Boost>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
