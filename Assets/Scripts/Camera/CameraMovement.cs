using UnityEngine;
using System.Collections;

// CameraMovement takes care of the movement of the camera
// from the camera shake, to zooming and panning.
public class CameraMovement : MonoBehaviour 
{
    //Camera Shake Variables
    private Vector3 _originPosition;
    private float _shakeDecay;
    private float _shakeIntensity;
    
    public enum CameraType { Perspective, Orthographic};
    
    public CameraType CameraView = CameraType.Orthographic;
    
    private Level _level;
    private Player _p1;
    private Player _p2;
    
    //Orthographic View Variables
    private float size;
    private float sensitivityDistanceO = -0.005f;
    private float dampingO = 2f;
    private float minO = 250;
    private float maxO = 540;
    private float sizeTrans;  //Transitioning size
    private Vector3 _posTrans;
    private float _playerDistance;
    private float _camPosX;
    
    //Perspective View Variables
    private float distance;
    private float sensitivityDistanceP = -7.5f;
    private float dampingP = 2.5f;
    private float minP = -1000;
    private float maxP = -550;
    private Vector3 zdistance;
    private float _zoomOffset = 1;

	// Use this for initialization
	void Start () 
    {
	    _originPosition = transform.position;
        
        size = 540f;
        
        if (GameObject.FindGameObjectWithTag("Level") != null)
        {
            _level = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
            _p1 = _level.player1;
            _p2 = _level.player2;
        }
    }
    
	// Update is called once per frame
	void Update () 
    {
        _playerDistance = Mathf.Abs(_p1.transform.position.x - _p2.transform.position.x);
        
        Zoom();
        Pan();
        
        if (_shakeIntensity > 0)
        {
            transform.position = new Vector3 (_posTrans .x + Random.insideUnitCircle.x * _shakeIntensity,
                                              _originPosition.y + Random.insideUnitCircle.y * _shakeIntensity,
                                              transform.position.z);
            _shakeIntensity -= _shakeDecay;
        }
        else
        {
            transform.position = new Vector3 (transform.position.x,
                                              _originPosition.y,
                                              transform.position.z);
        }
        
        
	}

    public void Shake(float shakeValue, float decayValue)
    {
        _originPosition = transform.position;
        _shakeIntensity = shakeValue;
        _shakeDecay = decayValue;
    }
    
    private void Zoom()
    {

        if (CameraView == CameraType.Orthographic)
        {
            size = _playerDistance / 2.0f;
            size = Mathf.Clamp(size, minO, maxO);
            sizeTrans = Mathf.Lerp(this.GetComponent<Camera>().orthographicSize, size, Time.deltaTime * dampingO);
            this.GetComponent<Camera>().orthographicSize = sizeTrans;
            
        }
        else if (CameraView == CameraType.Perspective)
        {
            distance = -_playerDistance * _zoomOffset;
            distance = Mathf.Clamp(distance, minP, maxP);
            zdistance = new Vector3(transform.position.x,
                                    transform.position.y,
                                    Mathf.Lerp(transform.position.z, distance, Time.deltaTime * dampingP));
            
            transform.position = zdistance;
        }
    }
    
    private void Pan()
    {
        
        if (_p1.transform.position.x > _p2.transform.position.x)  
            _camPosX = _p1.transform.position.x - (_playerDistance / 2.0f);
        else
            _camPosX = _p2.transform.position.x - (_playerDistance / 2.0f);
        
        _posTrans = new Vector3(Mathf.Lerp(this.GetComponent<Camera>().transform.position.x, _camPosX, Time.deltaTime * dampingO),
                                this.GetComponent<Camera>().transform.position.y,
                                this.GetComponent<Camera>().transform.position.z);
        
        transform.position = _posTrans;
    }
}
