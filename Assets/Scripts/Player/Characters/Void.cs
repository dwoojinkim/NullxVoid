using UnityEngine;
using System.Collections;

public class Void : MonoBehaviour 
{
    private GameManager GM;

    private GameObject _ice;
    private GameObject _ice2;

	// Use this for initialization
	void Start () 
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            
        gameObject.AddComponent<Boost>();

        /*
        _ice = (GameObject)Instantiate(Resources.Load("Player/SpecialEffects/CharacterEffects/Ice"));
        _ice.transform.parent = transform.Find("Effects");
        _ice.transform.localPosition = new Vector3 (0, 0, 0.1f);
        
        _ice2 = (GameObject)Instantiate(Resources.Load("Player/SpecialEffects/CharacterEffects/Ice2"));
        _ice2.transform.parent = transform.Find("Effects");
        _ice2.transform.localPosition = new Vector3 (0, 0, -0.11f);*/
    }
    
	// Update is called once per frame
	void FixedUpdate () 
    {
        /*
	    if (GM.EndRound)
        {
            _ice.GetComponent<ParticleSystem>().enableEmission = false;
            _ice2.GetComponent<ParticleSystem>().enableEmission = false;
        }
        else
        {
            _ice.GetComponent<ParticleSystem>().enableEmission = true;
            _ice2.GetComponent<ParticleSystem>().enableEmission = true;
        }*/
	}
}
