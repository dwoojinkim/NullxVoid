using UnityEngine;
using System.Collections;

public class StageChoice : MonoBehaviour 
{
    public bool Hovered { get; set; }
    
    public StageChoice Up { get; set; }
    public StageChoice Down { get; set; }
    public StageChoice Left { get; set; }
    public StageChoice Right { get; set; }
    
    public Sprite bgSprite;
    
    // Use this for initialization
    void Start () 
    {
        
    }
    
    // Update is called once per frame
    void Update () 
    {
        
    }
}
