using UnityEngine;
using System.Collections;

public class RotatingProjectileEffect : MonoBehaviour 
{
    //Make a better name for this component

    private const int MAX_PROJECTILES = 10;

    private GameObject[] _nodes;
    private float _radius;
    private float _rotation;
    private float _rotateRate;
    private int _currentProjectiles;

	// Use this for initialization
	void Start () 
    {
        _nodes = new GameObject[10];

        _radius = 100;
        _rotation = 0;
        _rotateRate = 200;
        _currentProjectiles = 4;

        for (int i = 0; i < 10; i++)
        {
            _nodes[i] = (GameObject)Instantiate(Resources.Load("Player/SpecialEffects/RotatingParticle"));

            _nodes[i].transform.parent = transform;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        for (int i = 0; i < _currentProjectiles; i++)
        {
            _nodes[i].transform.localPosition = new Vector3(_radius * Mathf.Cos((float)i * (2.0f * Mathf.PI) / (float)_currentProjectiles),
                                                            _radius * Mathf.Sin((float)i * (2.0f * Mathf.PI) / (float)_currentProjectiles),
                                                            _nodes[i].transform.localPosition.z);
        }

        for (int i = _currentProjectiles; i < MAX_PROJECTILES; i++)
        {
            _nodes[i].transform.position = new Vector3(10000, 100000, _nodes[i].transform.position.z);
        }

        transform.localEulerAngles = new Vector3(0, 0, _rotation);

        _rotation += _rotateRate * Time.deltaTime;

        if (_rotation >= 360)
            _rotation -= 360;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (_currentProjectiles > 0)
                _currentProjectiles--;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (_currentProjectiles < MAX_PROJECTILES)
                _currentProjectiles++;
        }
    }
}
