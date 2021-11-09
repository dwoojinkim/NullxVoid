using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
    public Color SpriteColor { get; set; }
    public bool Hovered { get; set; }
    public string Name { get; private set; }
    
    public Character Up { get; set; }
    public Character Down { get; set; }
    public Character Left { get; set; }
    public Character Right { get; set; }
    
    public string Name2;    

	// Use this for initialization
	void Start () 
    {
	    SpriteColor = transform.GetComponent<SpriteRenderer>().color;
        Name = name;
	}
	
	// Update is called once per frame
	void Update () 
    {
    
	}
}
