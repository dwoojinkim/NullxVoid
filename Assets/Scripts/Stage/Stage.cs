using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour 
{
    private GameManager GM;

	private const float MAX_SPEED = 32.0f;
	private const float TRAIL_OFFSET = 2.5f;
	private const float INITIAL_SPEED = 2.0f;

	public GameObject stage;

	private bool _shrinking;
    private float _speed;   //Speed stage degrades over time
	private Transform _leftTrail;
	private Transform _rightTrail;
	private WinnerBanner _winnerBanner;
	private Level _level;

	// Use this for initialization
	void Start () 
	{
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
		_speed = INITIAL_SPEED;
		_shrinking = true;

		_leftTrail = transform.FindChild("LeftTrail");
		_rightTrail = transform.FindChild("RightTrail");
        
        _leftTrail.position = new Vector3(-stage.GetComponentInChildren<SpriteRenderer>().bounds.extents.x + TRAIL_OFFSET,
                                          _leftTrail.position.y, _leftTrail.position.z);
        _rightTrail.position = new Vector3(stage.GetComponentInChildren<SpriteRenderer>().bounds.extents.x - TRAIL_OFFSET,
                                           _leftTrail.position.y, _leftTrail.position.z);
                                           
        _leftTrail.GetComponent<ParticleSystem>().Clear();
        _rightTrail.GetComponent<ParticleSystem>().Clear();
        
        _winnerBanner = transform.GetComponentInChildren<WinnerBanner>();

		_level = transform.parent.GetComponentInChildren<Level>();                                         
    }
    
    // Update is called once per frame
	void FixedUpdate () 
	{
		if (!_winnerBanner.Moving && !GM.EndOfMatch)
		{
			if(stage.transform.localScale.x <= 0.0f)
			{
				_shrinking = false;

				if (_speed < MAX_SPEED)
					_speed *= 2.0f;
			}
			else if(!_shrinking && stage.transform.localScale.x >= 30.0f)
			{
				_shrinking = true;

				if(_speed < MAX_SPEED)
					_speed *= 2.0f;
			}

			if(_shrinking)
				stage.transform.localScale = new Vector3(stage.transform.localScale.x - (_speed * Time.deltaTime), 1, 1);
			else
				stage.transform.localScale = new Vector3(stage.transform.localScale.x + (_speed * Time.deltaTime), 1, 1);

			_leftTrail.position = new Vector3(-stage.GetComponentInChildren<SpriteRenderer>().bounds.extents.x + TRAIL_OFFSET,
			                                  _leftTrail.position.y, _leftTrail.position.z);
			_rightTrail.position = new Vector3(stage.GetComponentInChildren<SpriteRenderer>().bounds.extents.x - TRAIL_OFFSET,
			                                  _leftTrail.position.y, _leftTrail.position.z);

			if (!_leftTrail.GetComponent<ParticleSystem>().enableEmission)
			{
				_leftTrail.GetComponent<ParticleSystem>().enableEmission = true;
				_rightTrail.GetComponent<ParticleSystem>().enableEmission = true;
			}
		}
		else
		{
			if (_leftTrail.GetComponent<ParticleSystem>().enableEmission)
			{
				_leftTrail.GetComponent<ParticleSystem>().enableEmission = false;
				_rightTrail.GetComponent<ParticleSystem>().enableEmission = false;
			}
		}
	}
	
    /*public void CheckStageSelection()
    {
        print("Blah");
    
        //if (GM.StageSelection.Equals("Bridge"))
        //{
        //}
        //else
        //{
            GameObject LevelPrefab = (GameObject)Instantiate(Resources.Load("Stages/" + GM.StageSelection + "/" + GM.StageSelection));
            gameObject.AddComponent(GM.StageSelection);

            LevelPrefab.transform.parent = this.transform;
            LevelPrefab.name = GM.StageSelection;
        //}
    }*/

	public void Reset()
	{
		_speed = INITIAL_SPEED;
		_shrinking = true;
        
        _leftTrail.position = new Vector3(-stage.GetComponentInChildren<SpriteRenderer>().bounds.extents.x + TRAIL_OFFSET,
                                          _leftTrail.position.y, _leftTrail.position.z);
        _rightTrail.position = new Vector3(stage.GetComponentInChildren<SpriteRenderer>().bounds.extents.x - TRAIL_OFFSET,
                                           _leftTrail.position.y, _leftTrail.position.z);
                                           
        _leftTrail.GetComponent<ParticleSystem>().Clear();
		_rightTrail.GetComponent<ParticleSystem>().Clear();
	}
}
