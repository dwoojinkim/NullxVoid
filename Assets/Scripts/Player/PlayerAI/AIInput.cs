using UnityEngine;
using System.Collections;

public static class AIInput
{
    //AI Input on Player 1 side
    private static bool Left1 = false;
    private static bool Right1 = false;
    private static bool Ability1 = false;
    private static bool LeftDown1 = false;
    private static bool RightDown1 = false;
    private static bool AbilityDown1 = false;
    private static bool LeftUp1 = false;
    private static bool RightUp1 = false;
    private static bool AbilityUp1 = false;
    
    //AI Input on Player 2 side
    private static bool Left2 = false;
    private static bool Right2 = false;
    private static bool Ability2 = false;
    private static bool LeftDown2 = false;
    private static bool RightDown2 = false;
    private static bool AbilityDown2 = false;
    private static bool LeftUp2 = false;
    private static bool RightUp2 = false;
    private static bool AbilityUp2 = false;

    public static bool GetKey(string key)
    {
        if (key.Equals("Left1"))
            return Left1;
        if (key.Equals("Right1"))
            return Right1;
        if (key.Equals("Ability1"))
            return Ability1;
        
        if (key.Equals("Left2"))
            return Left2;
        if (key.Equals("Right2"))
            return Right2;
        if (key.Equals("Ability2"))
            return Ability2;       
            
        return false;
    }
    public static bool GetKeyDown(string key)
    {
        if (key.Equals("Left1"))
            return Left1 && LeftDown1;
        if (key.Equals("Right1"))
            return Right1 && RightDown1;
        if (key.Equals("Ability1"))
            return Ability1 && AbilityDown1;
        
        if (key.Equals("Left2"))
            return Left2 && LeftDown2;
        if (key.Equals("Right2"))
            return Right2 && RightDown2;
        if (key.Equals("Ability2"))
            return Ability2 && AbilityDown2; 
            
        return false;
    }
    public static bool GetKeyUp(string key)
    {
        if (key.Equals("Left1"))
            return !Left1 && LeftUp1;
        if (key.Equals("Right1"))
            return !Right1 && RightUp1;
        if (key.Equals("Ability1"))
            return !Ability1 && AbilityUp1;
        
        if (key.Equals("Left2"))
            return !Left2 && LeftUp2;
        if (key.Equals("Right2"))
            return !Right2 && RightUp2;
        if (key.Equals("Ability2"))
            return !Ability2 && AbilityUp2; 
        
        return false;
    }
    
    public static void PressKey(string key)
    {
        //Player 1 AI
        if (key.Equals("Player1Left"))
        {
            if (!Left1)
                LeftDown1 = true;
            else 
                LeftDown1 = false;
            
            Left1 = true;
        }
        if (key.Equals("Player1Right"))
        {
            if (!Right1)
                RightDown1 = true;
            else 
                RightDown1 = false;
        
            Right1 = true;
        }
        if (key.Equals("Player1Ability"))
        {
            if (!Ability1)
                AbilityDown1 = true;
            else
                AbilityDown1 = false;
                
            Ability1 = true;
        }
        
        //Player 2 AI    
        if (key.Equals("Player2Left"))
        {
            if (!Left2)
                LeftDown2 = true;
            else 
                LeftDown2 = false;
            
            Left2 = true;
        }
        if (key.Equals("Player2Right"))
        {
            if (!Right2)
                RightDown2 = true;
            else 
                RightDown2 = false;
            
            Right2 = true;
        }
        if (key.Equals("Player2Ability"))
        {
            if (!Ability2)
                AbilityDown2 = true;
            else
                AbilityDown2 = false;
            
            Ability2 = true;
        }    
    }
    
    public static void ReleaseKey(string key)
    {
        if (key.Equals("Player1Left"))
        {
            if (Left1)
                LeftUp1 = true;
            else
                LeftUp1 = false;
                
            Left1 = false;
        }
        if (key.Equals("Player1Right"))
        {
            if (Right1)
                RightUp1 = true;
            else
                RightUp1 = false;
            
            Right1 = false;
        }
        if (key.Equals("Player1Ability"))
        {
            if (Ability1)
                Ability1 = true;
            else
                Ability1 = false;
            
            Ability1 = false;
        }
        
        if (key.Equals("Player2Left"))
        {
            if (Left2)
                LeftUp2 = true;
            else
                LeftUp2 = false;
            
            Left2 = false;
        }
        if (key.Equals("Player2Right"))
        {
            if (Right2)
                RightUp2 = true;
            else
                RightUp2 = false;
            
            Right2 = false;
        }
        if (key.Equals("Player2Ability"))
        {
            if (Ability2)
                Ability2 = true;
            else
                Ability2 = false;
            
            Ability2 = false;
        }   
    }
}
