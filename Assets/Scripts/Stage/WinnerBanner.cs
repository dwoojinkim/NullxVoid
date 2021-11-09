using UnityEngine;
using System.Collections;

public class WinnerBanner : MonoBehaviour 
{
	private GameManager GM;
    
	public bool Moving
	{
		get { return _rightWin || _leftWin; }
	}

	public bool RtWin
	{
		get { return _rightWin; }
	}

	public bool LtWin
	{
		get { return _leftWin; }
	}

	public Color BannerColor
	{	
		get { return transform.GetComponent<SpriteRenderer>().color; }
		set { transform.GetComponent<SpriteRenderer>().color = value; }
	}

	private SpriteRenderer _sprite;
	private Level _level;

	private bool _leftWin;
	private bool _rightWin;
	private float _velocity;
	private float _speed;

	// Use this for initialization
	void Start () 
	{
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    
		_sprite = transform.GetComponent<SpriteRenderer>();

		_leftWin = false;
		_rightWin = false;

		_speed = 1500.0f;
		_level = transform.parent.parent.GetComponent<Level>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (!GM.EndOfMatch)
		{
			if (_leftWin || _rightWin)
				transform.position = new Vector3(transform.position.x + (_velocity * Time.fixedDeltaTime), transform.position.y, transform.position.z);

			if (_leftWin)
			{
				if (transform.position.x >= _sprite.bounds.extents.x * 2)
				{
					_leftWin = false;
					_velocity = 0.0f;
					GM.EndRound = false;
				}
			}
			else if (_rightWin)
			{
				if (transform.position.x <= -_sprite.bounds.extents.x * 2)
				{
					_rightWin = false;
					_velocity = 0.0f;
					GM.EndRound = false;
				}
			}
		}
		else
		{
			_leftWin = false;
			_rightWin = false;
		}
	}

	public void LeftWin()
	{
		transform.position = new Vector3(-_sprite.bounds.extents.x * 2, transform.position.y, transform.position.z);
		_leftWin = true;
		_velocity = _speed;
		GM.EndRound = true;
	}

	public void RightWin()
	{
		transform.position = new Vector3(_sprite.bounds.extents.x * 2, transform.position.y, transform.position.z);
		_rightWin = true;
		_velocity = -_speed;
		GM.EndRound = true;
	}
}
