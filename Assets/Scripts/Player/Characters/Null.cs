using UnityEngine;
using System.Collections;

public class Null : MonoBehaviour 
{
    private GameManager GM;
    
    private GameObject _fire;
    private GameObject _fire2;

	// Use this for initialization
	void Start () 
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
        gameObject.AddComponent<Boost>();

        //GameObject rotatingSystem = (GameObject)Instantiate(Resources.Load("Player/SpecialEffects/RotatingSystem"));
        //rotatingSystem.transform.parent = transform;
        //rotatingSystem.transform.localPosition = new Vector3(0, 0, -0.1f);

        /*
        _fire = (GameObject)Instantiate(Resources.Load("Player/SpecialEffects/CharacterEffects/Fire"));
        _fire.transform.parent = transform.Find("Effects");
        _fire.transform.localPosition = new Vector3 (0, 0, 0.1f);
        _fire.GetComponent<ParticleSystem>().enableEmission = true;
        
        _fire2 = (GameObject)Instantiate(Resources.Load("Player/SpecialEffects/CharacterEffects/Fire2"));
        _fire2.transform.parent = transform.Find("Effects");
        _fire2.transform.localPosition = new Vector3 (0, -12, -0.11f);
        _fire2.GetComponent<ParticleSystem>().enableEmission = true;*/
    }
    
	// Update is called once per frame
	void FixedUpdate () 
    {
        /*
        if (GM.EndRound)
        {
            _fire.GetComponent<ParticleSystem>().enableEmission = false;
            _fire2.GetComponent<ParticleSystem>().enableEmission = false;
        }
        else
        {
            _fire.GetComponent<ParticleSystem>().enableEmission = true;
            _fire2.GetComponent<ParticleSystem>().enableEmission = true;
        }*/
    }
}
