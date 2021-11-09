using UnityEngine;
using System.Collections;

public class LohEpic : MonoBehaviour 
{
    public string Name { get; private set; }
    
	// Use this for initialization
	void Start () 
    {
        Name = "Loh Epic";
        gameObject.AddComponent<Boost>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
