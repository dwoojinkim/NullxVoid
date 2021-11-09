using UnityEngine;
using System.Collections;

public class ChargeShot : MonoBehaviour 
{
    private GameManager GM;
    private Player _player;

    private const float MAX_RECOIL = 350f;  //Max amount of additional recoil with max amount of projectile charge.

    public bool FiringProjectile { get; set; }
    public bool LeftShoot { get; set; }
    public bool RightShoot { get; set; }
    public float Charge
    {
        get { return _charge; }
        set { _charge = value; }
    }
    public bool Charging
    {
        get { return _charging; }
        set { _charging = value; }
    }
    public ParticleSystem ChargeEffect
    {
        get { return _chargeParticles; }
        set { _chargeParticles = value; }
    }
    public AudioSource ChargeAudio
    {
        get { return _chargeAudio; }
        set { _chargeAudio = value; }
    }

    private CSCollision _csCollision;
    private ProjectilePool _projPool;
    private ParticleSystem _chargeParticles;
    private ParticleSystem _shootParticles;
    private AudioSource _chargeAudio;
    private AudioSource _shootAudio;
    private AudioClip _chargeUp1;
    private AudioClip _chargeUp2;
    
    private bool _charging;
    private float _charge;

	// Use this for initialization
	void Start () 
    {        
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            
        _player = transform.GetComponent<Player>();
        _csCollision =  gameObject.AddComponent<CSCollision>();
        if (name.Contains("1") && transform.parent.Find("Player2").GetComponent<ChargeShot>() == null)
            transform.parent.Find("Player2").gameObject.AddComponent<CSCollision>();
        else if (name.Contains("2") && transform.parent.Find("Player1").GetComponent<ChargeShot>() == null)
            transform.parent.Find("Player1").gameObject.AddComponent<CSCollision>();
        
        _projPool = _player.gameObject.AddComponent<ProjectilePool>();
        _projPool.projectilePrefab = ((GameObject)Resources.Load("Player/Projectile")).transform;

        _chargeParticles = ((GameObject)Instantiate(Resources.Load("Player/SpecialEffects/ChargeShotEffects/ChargeEffect"))).GetComponent<ParticleSystem>();
        _chargeParticles.transform.parent = transform.Find("Effects");
        _chargeParticles.transform.localPosition = new Vector3 (0, 0, -0.15f);
        
        _shootParticles = ((GameObject)Instantiate(Resources.Load("Player/SpecialEffects/ChargeShotEffects/ShootEffect"))).GetComponent<ParticleSystem>();
        _shootParticles.transform.parent = transform.Find("Effects");
        _shootParticles.transform.localPosition = new Vector3 (0, 0, -0.15f);
        
        _chargeAudio = _chargeParticles.transform.GetComponent<AudioSource>();
        _chargeAudio.clip = _chargeUp1;
        _shootAudio = _shootParticles.transform.GetComponent<AudioSource>();
        
        _chargeUp1 = (AudioClip)Resources.Load("Sounds/SoundEffects/Player/Chargeup1");
        _chargeUp2 = (AudioClip)Resources.Load("Sounds/SoundEffects/Player/Chargeup2");
            
        _charge = 0;
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!GM.EndRound && !GM.EndOfMatch)
        {
	        PlayerInput();
            
            PlayerProjectiles();
        }
        else
        {
            _charging = false;
            _charge = 0;
            FiringProjectile = false;
            _chargeParticles.enableEmission = false;
            _chargeAudio.Stop();
            _chargeAudio.clip = null;
            _chargeAudio.loop = false;
        }
	}
    
    private void PlayerInput()
    {
        if (GM.GetAbility(name))
            ChargeProjectile();
        else
        {
            if (_charge > 0)
                ShootProjectile();
        }
    }
    
    private void ChargeProjectile()
    {
        _charge += Time.deltaTime;
        _charging = true;
        
        if (!_chargeParticles.enableEmission && _charge > 0.05f)
            _chargeParticles.enableEmission = true;
        
        if (_charge > 1)
        {
            _chargeParticles.startColor = Color.green;
            _chargeParticles.emissionRate = 50;
        } else if (_charge > 0.5f)
        {
            _chargeParticles.startColor = Color.magenta;
            _chargeParticles.emissionRate = 25;
        } else if (_charge > 0.25f)
        {
            _chargeParticles.startColor = Color.blue;
            _chargeParticles.emissionRate = 10;
        } else
        {
            _chargeParticles.startColor = Color.cyan;
            _chargeParticles.emissionRate = 5;
        }
        
        if (_chargeAudio.clip == null)
        {
            _chargeAudio.clip = _chargeUp1;
            _chargeAudio.Play();
        } else if (_chargeAudio.clip == _chargeUp1 && !_chargeAudio.isPlaying)
        {
            _chargeAudio.clip = _chargeUp2;
            _chargeAudio.Play();
            _chargeAudio.loop = true;
        }
    }
    
    private void ShootProjectile()
    {
        if (_charge > 0)
        {
            FiringProjectile = true;
            
            _shootAudio.Play();
            
            if (_player.LookingLeft)
            {
                LeftShoot = true;
                _csCollision.PushSpeed = 1000;
                
                if (_charge <= 1)
                    _csCollision.PushSpeed += _charge * MAX_RECOIL;
                else
                    _csCollision.PushSpeed += MAX_RECOIL;
                
                _csCollision.SlideEffect.enableEmission = true;
            }
            else
            {
                RightShoot = true;
                _csCollision.PushSpeed = -1000;
                
                if (_charge <= 1)
                    _csCollision.PushSpeed -= _charge * MAX_RECOIL;
                else
                    _csCollision.PushSpeed -= MAX_RECOIL;
                
                _csCollision.SlideEffect.enableEmission = true;
            }
        }
        
        _charging = false;
        
        if (_chargeParticles.enableEmission)
            _chargeParticles.enableEmission = false;
        _chargeParticles.Clear();
        
        _chargeAudio.Stop();
        _chargeAudio.clip = null;
        _chargeAudio.loop = false;
    }
    
    private void PlayerProjectiles()
    {
        if (FiringProjectile)
        {
            Projectile projectile = GetComponent<ProjectilePool>().RequestObject();
            
            if (_player.LookingLeft)
                projectile.MoveLeft = true;
            else
                projectile.MoveLeft = false;
            
            projectile.enabled = true;
            if (name.Contains("1"))
                projectile.Owner = 1;
            else if (name.Contains("2"))
                projectile.Owner = 2;
            projectile.ClearParticles();
            
            if (_charge > 1)
                projectile.PowerLevel = 1;
            else
                projectile.PowerLevel = _charge;
            
            if (projectile.MoveLeft)
                _shootParticles.transform.eulerAngles = new Vector3(0, -90, 0);
            else
                _shootParticles.transform.eulerAngles = new Vector3(0, 90, 0);
                
            projectile.transform.position = transform.position;
            projectile.trail.GetComponent<ParticleSystem>().Clear();
            
            _shootParticles.Emit(40);
            
           _charge = 0;
           FiringProjectile = false;
           _shootAudio.Play();
        }
    }
    
    public void Reset()
    {
        _projPool.ResetProjectiles();
        _charging = false;
        _charge = 0;
        FiringProjectile = false;
        _chargeParticles.enableEmission = false;
        _chargeAudio.Stop();
        _chargeAudio.clip = null;
        _chargeAudio.loop = false;
    }
}
