using UnityEngine;
using System.Collections;

public class Em : MonoBehaviour 
{
    public string Name { get; private set; }
    
	// Use this for initialization
	void Start () 
    {
        Name = "Em";
        gameObject.AddComponent<Boost>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
