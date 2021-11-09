using UnityEngine;
using System.Collections;

public class Monsoon : MonoBehaviour 
{

    public float StreamSpeed
    {
        get { return _streamSpeed; }
    }

    private Transform _flood;
    private Vector3 _floodStartPos;
    private float _floodSpeed;
    private Transform _stream; 
    private float _streamSpeed;
    private Vector3 _streamRightStart;
    private Vector3 _streamLeftStart;
    private ParticleSystem _streamParticles;

	// Use this for initialization
	void Start () 
    {
        _flood = transform.Find("Monsoon/Flood");
        _stream = transform.Find("Monsoon/Stream");

        _floodStartPos = _flood.position;
        _streamLeftStart = _stream.position;
        _streamRightStart = new Vector3 (-_stream.position.x, _stream.position.y, _stream.position.z);
        _floodSpeed = 50.0f;
        _streamSpeed = 0.0f;

        _streamParticles = transform.Find("Monsoon/Stream/StreamParticles").GetComponent<ParticleSystem>();
        _streamParticles.enableEmission = false;
        _streamParticles.Clear();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        
	    _flood.position = new Vector3(_flood.position.x, _flood.position.y + (_floodSpeed * Time.deltaTime),
                                      _flood.position.z);

        _stream.position = new Vector3(_stream.position.x + (_streamSpeed * Time.deltaTime),
                                       _stream.position.y,
                                       _stream.position.z);

        if (_flood.position.y > 400)
        {
            if (Random.Range(0, 2) == 0)
                LeftFlush();
            else
                RightFlush();

            _floodSpeed = -500;

            _streamParticles.enableEmission = true;
        }
        else if (_flood.position.y <= _floodStartPos.y)
        {
            _floodSpeed = 50;
        }

        if (_stream.position.x > 1920)
        {
            _streamSpeed = 0.0f;
            _streamParticles.enableEmission = false;
        }
	}

    
    public void LeftFlush()
    {
        _streamSpeed = 1500;
        _stream.position = _streamLeftStart;
    }
    
    public void RightFlush()
    {
        _streamSpeed = -1500;
        _stream.position = _streamRightStart;
    }
}
