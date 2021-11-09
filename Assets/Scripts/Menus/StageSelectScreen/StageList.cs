using UnityEngine;
using System.Collections;

public class StageList : MonoBehaviour 
{
    public GameObject[] Stages;
    
    private float _boundaryPos;     //Boundary position of the left/right most Stages
    private int _columns;           //Max number of columns in the StageChoice list
    private float _rowOffset;       //Position offset
    private float _startYPos;
    
    // Use this for initialization
    void Start () 
    {
        _boundaryPos = 275.0f;
        _columns = 3;
        _rowOffset = 150;
        _startYPos = 200;
        
        CalculatePositions();
    }
    
    // Update is called once per frame
    void Update () 
    {
        
    }
    
    private void CalculatePositions()
    {
        int currentStageNum = 0;
        
        for (int i = 0; i < (float)Stages.Length / _columns; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (currentStageNum < Stages.Length)
                {
                    Stages[currentStageNum].transform.localPosition = new Vector3(-_boundaryPos + (j * (_boundaryPos * 2 / (_columns - 1))), 
                                                                                _startYPos - (i * _rowOffset),
                                                                                Stages[j].transform.localPosition.z);
                    
                    //Up
                    if (i - 1 >= 0)
                        Stages[currentStageNum].GetComponent<StageChoice>().Up = Stages[(i - 1) * _columns + j].GetComponent<StageChoice>();
                    else
                    {
                        if ((Mathf.CeilToInt((float)Stages.Length / _columns) - 1) * _columns + j < Stages.Length)
                            Stages[currentStageNum].GetComponent<StageChoice>().Up = Stages[(Mathf.CeilToInt((float)Stages.Length / _columns) - 1) * _columns + j].GetComponent<StageChoice>();
                        else
                            Stages[currentStageNum].GetComponent<StageChoice>().Up = Stages[(Mathf.CeilToInt((float)Stages.Length / _columns) - 2) * _columns + j].GetComponent<StageChoice>();
                    }
                    
                    //Down
                    if ((i + 1) <= (float)Stages.Length / _columns && (i + 1) * _columns + j < Stages.Length)
                        Stages[currentStageNum].GetComponent<StageChoice>().Down = Stages[(i + 1) * _columns + j].GetComponent<StageChoice>();
                    else
                        Stages[currentStageNum].GetComponent<StageChoice>().Down = Stages[j].GetComponent<StageChoice>();
                    
                    //Right
                    if (currentStageNum + 1 < Stages.Length && currentStageNum + 1 < ((i * _columns)) + _columns)
                        Stages[currentStageNum].GetComponent<StageChoice>().Right = Stages[currentStageNum + 1].GetComponent<StageChoice>();
                    else
                        Stages[currentStageNum].GetComponent<StageChoice>().Right = Stages[i * _columns].GetComponent<StageChoice>();
                    
                    //Left
                    if (currentStageNum - 1 >= 0 && currentStageNum - 1 >= i * _columns)
                        Stages[currentStageNum].GetComponent<StageChoice>().Left = Stages[currentStageNum - 1].GetComponent<StageChoice>();
                    else if ((i * _columns) + _columns - 1 < Stages.Length)
                        Stages[currentStageNum].GetComponent<StageChoice>().Left = Stages[(i * _columns) + _columns - 1].GetComponent<StageChoice>();
                    
                }
                currentStageNum++;                                                   
            }      
        }
    }
}
