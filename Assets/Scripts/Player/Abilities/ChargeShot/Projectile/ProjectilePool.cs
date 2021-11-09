using UnityEngine;
using System.Collections;

public class ProjectilePool : MonoBehaviour 
{

    public Transform projectilePrefab;

    protected Pool<Projectile> pool;

	// Use this for initialization
	void Start () 
    {
        pool = new Pool<Projectile>(20, "Projectile", projectilePrefab, transform.parent);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public Projectile RequestObject()
    {
        return pool.RequestObject();
    }

    public void ResetProjectiles()
    {
        pool.ResetPool();
    }
}
