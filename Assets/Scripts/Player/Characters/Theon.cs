using UnityEngine;
using System.Collections;

public class Theon : MonoBehaviour 
{
    public string Name { get; private set; }
    
	// Use this for initialization
	void Start () 
    {
        Name = "Theon";
        gameObject.AddComponent<Boost>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
