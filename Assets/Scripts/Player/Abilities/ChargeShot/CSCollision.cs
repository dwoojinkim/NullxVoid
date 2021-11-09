using UnityEngine;
using System.Collections;

public class CSCollision : MonoBehaviour 
{
    private GameManager GM;
    private Player _player;
    
    private const float MAX_RECOIL = 350f;  //Max amount of additional recoil with max amount of projectile charge.
    private const float DECEL = 2000;       //Normal deceleration rate
    
    public float PushSpeed { get; set; }
    public ParticleSystem SlideEffect
    {
        get { return _slideParticles; }
        set { _slideParticles = value; }
    }
    
    private ParticleSystem _hitParticles;
    private ParticleSystem _slideParticles;
    private ParticleSystem _deflectParticles;
    private ParticleSystem _deflectSprite;
    private ChargeShot _chargeShot;
    private Projectile _collidedProjectile;
    private AudioSource _hitAudio;
    private AudioSource _deflectAudio;
    
    private float _pushDecel;
    private float _hitTime;
    private float _rightPressTime;
    private float _leftPressTime;
    private float _lastRightPress;
    private float _lastLeftPress;
    private bool _rightHit;
    private bool _leftHit;
    
	// Use this for initialization
	void Start () 
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            
        _player = transform.GetComponent<Player>();

        _hitParticles = ((GameObject)Instantiate(Resources.Load("Player/SpecialEffects/ChargeShotEffects/HitEffect"))).GetComponent<ParticleSystem>();
        _hitParticles.transform.parent = transform.Find("Effects");
        
        _hitAudio = _hitParticles.transform.GetComponent<AudioSource>();

        _slideParticles = ((GameObject)Instantiate(Resources.Load("Player/SpecialEffects/ChargeShotEffects/SlideEffect"))).GetComponent<ParticleSystem>();
        _slideParticles.transform.parent = transform.Find("Effects");
        _slideParticles.transform.localPosition = new Vector3 (0, _slideParticles.transform.localPosition.y, -0.15f);
        _slideParticles.Play();
        _slideParticles.enableEmission = false;
        _slideParticles.Clear();
        
        _deflectParticles = ((GameObject)Instantiate(Resources.Load("Player/SpecialEffects/ChargeShotEffects/DeflectEffect"))).transform.Find("DeflectParticles").GetComponent<ParticleSystem>();
        _deflectParticles.transform.parent.parent = transform.Find("Effects");
        _deflectParticles.transform.parent.localPosition = new Vector3 (0, _deflectParticles.transform.parent.localPosition.y, -0.15f);
        
        _deflectSprite = _deflectParticles.transform.parent.Find("DeflectSprite").GetComponent<ParticleSystem>();
        
        _deflectAudio = _deflectParticles.transform.GetComponent<AudioSource>();

        if (transform.GetComponent<ChargeShot>() != null)
            _chargeShot = transform.GetComponent<ChargeShot>();
	}
	
    void Update()
    {
        PlayerInput();
    }
    
	// Update is called once per frame
	void FixedUpdate () 
    {
	    CheckStun();
	}
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GM.EndRound && other.transform.name.Contains("Projectile"))
        {
            _collidedProjectile = other.transform.GetComponent<Projectile>();
            
            if ((name.Equals("Player1") && _collidedProjectile.Owner == 2) || (name.Equals("Player2") && _collidedProjectile.Owner == 1))
                ProjectileCollision(_collidedProjectile);
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (!GM.EndRound && other.transform.name.Contains("Projectile"))
        {
            _collidedProjectile = other.transform.GetComponent<Projectile>();
            
            if ((name.Equals("Player1") && _collidedProjectile.Owner == 2) || (name.Equals("Player2") && _collidedProjectile.Owner == 1))
                ProjectileCollision(_collidedProjectile);
        }
    }
    
    private void PlayerInput()
    {
        //Left Swap
        if (GM.GetLeftDown(name))
        {
            _lastLeftPress = Time.time;
            _leftPressTime = 0;
        }
        //Right Swap
        if (GM.GetRightDown(name))
        {
            _lastRightPress = Time.time;
            _rightPressTime = 0;
        }
        
        if (GM.GetLeft(name))
        {
            _leftPressTime += Time.deltaTime;
        }
        else if (GM.GetRight(name))
        {
            _rightPressTime += Time.deltaTime;
        }
    }
    
    private void ProjectileCollision(Projectile projectile)
    {
        _hitTime = Time.time;
        
        if (projectile.transform.position.x > transform.position.x)
            _rightHit = true;
        else
            _leftHit = true;
        
        if (_rightHit)
        {
            if (_rightPressTime < 0.05f && _hitTime - _lastRightPress < 0.15f && !GM.GetRight(name))
            {
                projectile.Deflect();
                _deflectAudio.Play();
                _deflectParticles.transform.parent.localEulerAngles = new Vector3 (0, 0, 0);
                _deflectParticles.Play();
                _deflectSprite.startRotation = 0;
                _deflectSprite.Play();
                _rightHit = false;
            } else if (_hitTime - _lastLeftPress < 0.25f)
            {
                //Projectile swap code
                SwapProjectiles();
                _rightHit = false;
            } else
            {
                if (transform.GetComponent<Boost>() == null || !transform.GetComponent<Boost>().BoostArmor)
                {
                    PushSpeed = -750;
                    
                    if (projectile.PowerLevel <= 1)
                        PushSpeed -= projectile.PowerLevel * MAX_RECOIL;
                    else
                        PushSpeed -= MAX_RECOIL;
                }
                
                _hitParticles.transform.position = new Vector3(transform.position.x + transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                               transform.position.y, transform.position.z);
                                                               
                _hitParticles.Emit(100);
                
                if (projectile.Explosion != null)
                    projectile.Explosion.Emit(50);
                
                if (_chargeShot != null)
                {
                    _chargeShot.Charge = 0.0f;
                    _chargeShot.Charging = false;
                    _chargeShot.ChargeEffect.enableEmission = false;
                    _chargeShot.ChargeEffect.Clear();
                    _chargeShot.ChargeAudio.Stop();
                    _chargeShot.ChargeAudio.clip = null;
                }
                _hitAudio.Play();
                
                _player.RightShoot = false;
                _player.LeftShoot = false;
                
                projectile.Reset();
            }
        }
        if (_leftHit)
        {
            if (_leftPressTime < 0.05f && _hitTime - _lastLeftPress < 0.15f && !GM.GetLeft(name))
            {
                projectile.Deflect();
                _deflectAudio.Play();
                _deflectParticles.transform.parent.localEulerAngles = new Vector3 (0, 180, 0);
                _deflectParticles.Play();
                _deflectSprite.startRotation = 180;
                _deflectSprite.Play();
                _leftHit = false;
            } 
            else if (_hitTime - _lastRightPress < 0.25f)
            {
                //Projectile swap code
                SwapProjectiles();
                _leftHit = false;
            } else
            {
                if (transform.GetComponent<Boost>() == null || !transform.GetComponent<Boost>().BoostArmor)
                {
                    PushSpeed = 750;
                    
                    if (projectile.PowerLevel <= 1)
                        PushSpeed += projectile.PowerLevel * MAX_RECOIL;
                    else
                        PushSpeed += MAX_RECOIL;
                }
                
                _hitParticles.transform.position = new Vector3(transform.position.x - transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                               transform.position.y, transform.position.z);
                                                               
                _hitParticles.Emit(100);
                
                if (projectile.Explosion != null)
                    projectile.Explosion.Emit(50);
                
                if (_chargeShot != null)
                {
                    _chargeShot.Charge = 0.0f;
                    _chargeShot.Charging = false;
                    _chargeShot.ChargeEffect.enableEmission = false;
                    _chargeShot.ChargeEffect.Clear();
                    _chargeShot.ChargeAudio.Stop();
                    _chargeShot.ChargeAudio.clip = null;
                }
                _hitAudio.Play();
                
                _player.RightShoot = false;
                _player.LeftShoot = false;
                
                projectile.Reset();
            }
        }
    }
    
    private void SwapProjectiles()
    {
        Vector3 _tempPos = transform.position;
        
        transform.position = new Vector3(_collidedProjectile.transform.position.x, _collidedProjectile.transform.position.y,
                                         _collidedProjectile.transform.position.z);
        _collidedProjectile.transform.position = new Vector3(_tempPos.x, _tempPos.y, _tempPos.z);
        
        if (transform.position.x < _collidedProjectile.transform.position.x)
            _player.SwapEffect.transform.position = new Vector3(transform.position.x - transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                        _player.SwapEffect.transform.position.y, _player.SwapEffect.transform.position.z);
        else
            _player.SwapEffect.transform.position = new Vector3(transform.position.x + transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                                                        _player.SwapEffect.transform.position.y, _player.SwapEffect.transform.position.z);
        
        _player.SwapEffect.Emit(100);
        _player.SwapSound();
    }
    
    private void CheckStun()
    {
        //Projectile hit
        if (_leftHit && PushSpeed > 0)
        {            
            if (GM.GetLeft(name))
                _pushDecel = DECEL * 1.25f;
            else
                _pushDecel = DECEL;
            
            PushSpeed -= _pushDecel * Time.deltaTime;
            
            if (PushSpeed <= 0)
            {
                _leftHit = false;
                _slideParticles.enableEmission = false; 
                _player.Velocity = 0;
                PushSpeed = 0;
            }
            _player.Velocity = PushSpeed;
        }
        else if (_rightHit && PushSpeed < 0)
        {
            _player.Velocity = PushSpeed;
            
            if (GM.GetRight(name))
                _pushDecel = DECEL * 1.25f;
            else
                _pushDecel = DECEL;
            
            PushSpeed += _pushDecel * Time.deltaTime;
            
            if (PushSpeed >= 0)
            {
                _rightHit = false;
                _slideParticles.enableEmission = false; 
                _player.Velocity = 0;
                PushSpeed = 0;
            }
            _player.Velocity = PushSpeed;
        }
        //Projectile Recoil
        else if (_chargeShot != null && _chargeShot.LeftShoot && PushSpeed > 0)
        {
            _player.Velocity = PushSpeed;
            
            if (GM.GetLeft(name))
                _pushDecel = DECEL * 2;
            else
                _pushDecel = DECEL;

            PushSpeed -= _pushDecel * Time.deltaTime;
            
            if (PushSpeed <= 0)
            {
                _chargeShot.LeftShoot = false;
                _slideParticles.enableEmission = false; 
                _player.Velocity = 0;
                PushSpeed = 0;
            }
            _player.Velocity = PushSpeed;
        }
        else if (_chargeShot != null && _chargeShot.RightShoot && PushSpeed < 0)
        {
            _player.Velocity = PushSpeed;
            
            if (GM.GetRight(name))
                _pushDecel = DECEL * 2;
            else
                _pushDecel = DECEL;
            
            PushSpeed += _pushDecel * Time.deltaTime;
            
            if (PushSpeed >= 0)
            {
                _chargeShot.RightShoot = false;
                _slideParticles.enableEmission = false; 
                _player.Velocity = 0;
                PushSpeed = 0;
            }
            _player.Velocity = PushSpeed;
        }
    }
    
}
