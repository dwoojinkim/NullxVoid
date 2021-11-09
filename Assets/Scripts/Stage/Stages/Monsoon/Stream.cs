using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour 
{

    private Monsoon _monsoon;

	// Use this for initialization
	void Start () 
    {
        _monsoon = transform.parent.parent.GetComponent<Monsoon>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Player"))
        {
            other.transform.GetComponent<Player>().Stunned = true;
            other.transform.GetComponent<Player>().Velocity = _monsoon.StreamSpeed;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

    }
}
