using UnityEngine;
using System.Collections;

public class Quillby : MonoBehaviour 
{
    public string Name { get; private set; }
    
	// Use this for initialization
	void Start () 
    {
        Name = "Quillby";
        gameObject.AddComponent<Boost>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
