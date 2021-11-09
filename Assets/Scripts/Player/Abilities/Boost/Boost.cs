using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour 
{
    private GameManager GM;
    private Player _player;

    public bool BoostArmor { get; set; }
    public float BoostTime { get; set; }

    private float _currentBoost;    //Current amount of time left in the boost
    private bool _rightBoost;
    private bool _leftBoost;
    
    private ParticleSystem _boostTrail;
    private ParticleSystem _superArmorEffect;
    private AudioSource _audio;

	// Use this for initialization
	void Awake () 
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
        _player = transform.GetComponent<Player>();
    
        BoostTime = 0.5f;
    
	    _boostTrail = ((GameObject)Instantiate(Resources.Load("Player/SpecialEffects/BoostEffects/BoostTrail"))).GetComponent<ParticleSystem>();
        _boostTrail.transform.parent = _player.transform.Find("Effects");
        _boostTrail.enableEmission = false;
        _boostTrail.Clear();
        _boostTrail.transform.position = new Vector3 (_player.transform.position.x, _player.transform.position.y, _boostTrail.transform.position.z);
        if (name.Contains("1"))
            _boostTrail.startColor = GM.P1CharChoice;
        else if (name.Contains("2"))
            _boostTrail.startColor = GM.P2CharChoice;
        
        _superArmorEffect = ((GameObject)Instantiate(Resources.Load("Player/SpecialEffects/BoostEffects/SuperArmor"))).GetComponent<ParticleSystem>();
        _superArmorEffect.transform.parent = _player.transform.Find("Effects");
        _superArmorEffect.transform.position = new Vector3 (_player.transform.position.x, _player.transform.position.y, _superArmorEffect.transform.position.z);
        _superArmorEffect.enableEmission = false;
        _superArmorEffect.Clear();
            if (name.Contains("1"))
            _superArmorEffect.startColor = GM.P1CharChoice;
        else if (name.Contains("2"))
            _superArmorEffect.startColor = GM.P2CharChoice;
        
        _audio = _boostTrail.transform.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!GM.EndRound && !GM.EndOfMatch)
	        PlayerInput();
	}
    
    void FixedUpdate()
    {
        CheckBoost();
        
        if (_player.Dead)
        {
            _currentBoost = 0;
            _player.Velocity = 0;
        }
    }
    
    private void PlayerInput()
    {
        //Boost input
        if (GM.GetAbilityDown(name))
        {
            //Boost mode
            if (_currentBoost <= 0 && _player.IsPressing)
            {
                _currentBoost = BoostTime;
                
                if (GM.GetLeft(name))
                    _leftBoost = true;
                else if (GM.GetRight(name))
                    _rightBoost = true;
            }
        }
    }
    
    private void CheckBoost()
    {
        if (_currentBoost > 0 && !_player.Stunned)
        {
            _player.DisableInput = true;
        
            if (_leftBoost)
                _player.Velocity = -1000.0f;
            else if (_rightBoost)
                _player.Velocity = 1000.0f;
            
            if (!_boostTrail.enableEmission)
            {
                _audio.Play();
                    
                _player.IsPressing = true;
                _boostTrail.enableEmission = true;
            }
            
            if (_currentBoost / BoostTime <= 0.5f )
            {
                BoostArmor = true;
            }
            
            _currentBoost -= Time.deltaTime;
        } else
        {
            if (_boostTrail.enableEmission)
            {
                _boostTrail.enableEmission = false;
                _rightBoost = false;
                _leftBoost = false;
                _player.IsPressing = false;
                BoostArmor = false;
                
                _player.DisableInput = false;
            }
        }
        
        if (BoostArmor)
            _superArmorEffect.enableEmission = true;
        else
            _superArmorEffect.enableEmission = false;
    }
    
}
