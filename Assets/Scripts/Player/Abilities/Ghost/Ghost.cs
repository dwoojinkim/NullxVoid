using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour 
{
    private GameManager GM;

    private const float GHOST_TIME = 0.75f;

    private Player _player;
    private SpriteRenderer _playerSprite;
    private float _currentGhostDuration;
    private bool _ghost;

	// Use this for initialization
	void Start () 
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        _player = transform.GetComponent<Player>();
        _playerSprite = transform.Find("PlayerSprite").GetComponent<SpriteRenderer>();

        _currentGhostDuration = 0;
        _ghost = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        PlayerInput();
	}

    void FixedUpdate()
    {
        if (_ghost)
        {
            _playerSprite.color = new Color(_playerSprite.color.r, _playerSprite.color.g, _playerSprite.color.b, 0.5f);
            transform.GetComponent<BoxCollider2D>().enabled = false;

            _currentGhostDuration += Time.deltaTime;

            _player.DisableInput = true;
            _player.CollidedLeft = false;
            _player.CollidedRight = false;
            _player.Velocity = 0;

            if (_currentGhostDuration >= GHOST_TIME)
            {
                _currentGhostDuration = 0;
                _ghost = false;
                transform.GetComponent<BoxCollider2D>().enabled = true;
                _playerSprite.color = new Color(_playerSprite.color.r, _playerSprite.color.g, _playerSprite.color.b, 1);
                _player.DisableInput = false;
            }
        }
    }

    private void PlayerInput()
    {
        if (GM.GetAbilityDown(name))
        {
            _ghost = true;
        }
    }
}
