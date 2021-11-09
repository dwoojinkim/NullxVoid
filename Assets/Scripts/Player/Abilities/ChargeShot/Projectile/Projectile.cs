using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{

    private GameManager GM;

    public GameObject trail;

    protected SpriteRenderer sprite;
    public float speed;
    protected float accel;
    private Vector3 resetPos;
    private ParticleSystem _explosion;
	private Level _level;

    public bool MoveLeft
    {
        get;
        set;
    }

    public int Owner //Player # That owns the bullet
    {
        get;
        set;
    }

	public float PowerLevel
	{
		get;
		set; 
	}
	
	public float JFTiming	//"Just Frame" timing
	{
		get;
		set;
	}
	
	public ParticleSystem Explosion
	{
		get { return _explosion; }
	}

    private const float MAX_SCALE = 0.4f;    //Max additional scale to projectile

	// Use this for initialization
	void Start () 
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        speed = 600f;

        sprite = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
        
        resetPos = new Vector3(-1000, 0, 0);
        
		trail.GetComponent<ParticleSystem>().enableEmission = false;
		
		_explosion = transform.Find("Explosion").GetComponent<ParticleSystem>();

		_level = transform.parent.GetComponent<Level>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        CheckEnding();

        if (this.enabled && !GM.EndRound)
        {
            Move();
            trail.GetComponent<ParticleSystem>().enableEmission = true;

            speed = 700;

            if (PowerLevel <= 1)
	            transform.localScale = new Vector3(0.2f + PowerLevel * MAX_SCALE, 0.2f + PowerLevel * MAX_SCALE, 1);
            else
                transform.localScale = new Vector3(0.2f + PowerLevel, 0.2f + PowerLevel, 1);

            if (Owner == 1)
            {
                transform.GetComponentInChildren<SpriteRenderer>().color = GM.P1CharChoice;
                transform.Find("Trail").GetComponent<ParticleSystem>().startColor = GM.P1CharChoice;
            }
            else if (Owner == 2)
            {
                transform.GetComponentInChildren<SpriteRenderer>().color = GM.P2CharChoice;
                transform.Find("Trail").GetComponent<ParticleSystem>().startColor = GM.P2CharChoice;
            }
        }
	}

    protected void CheckEnding()
    {
        if (this.enabled && (transform.position.x > 1000 || transform.position.x < -1000))
        {
            Reset();
            return;
        }
    }

	void OnTriggerEnter2D( Collider2D other )
	{
		if (this.enabled && other.transform.GetComponent<Projectile>() != null && other.transform.GetComponent<Projectile>().enabled)
		{
			if (other.transform.GetComponent<Projectile>().PowerLevel == PowerLevel)
			{
                if (_explosion != null)
				    _explosion.Emit(50);
				other.transform.GetComponent<Projectile>().Reset();
				Reset();
			}
			else if (other.transform.GetComponent<Projectile>().PowerLevel > PowerLevel)
			{
                if (_explosion != null)
                    _explosion.Emit(50);
                Reset();
			}
			else
			{
                if (_explosion != null)
                    _explosion.Emit(50);
                other.transform.GetComponent<Projectile>().Reset();
			}
		}
	}

    protected void Move()
    {
        float directionalVelocity = 0;
        if (MoveLeft)
            directionalVelocity = -speed;
        else
            directionalVelocity = speed;

        transform.position = new Vector3(transform.position.x + (directionalVelocity * Time.fixedDeltaTime), 0,0);
    }
    
    public void Reset()
    {
    	transform.position = resetPos;
    	Owner = 0;
		PowerLevel = 0;
    	
		trail.GetComponent<ParticleSystem>().enableEmission = false;
        enabled = false;
    }

	public void ClearParticles()
	{
		trail.GetComponent<ParticleSystem>().Clear();
	}
	
	public void Deflect()
	{
		MoveLeft = !MoveLeft;
		
		if (Owner == 1)
			Owner = 2;
		else if (Owner == 2)
			Owner = 1;
	}
}
