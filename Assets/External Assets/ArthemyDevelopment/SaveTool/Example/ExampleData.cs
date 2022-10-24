using System.Collections;
using System.Collections.Generic;
using ArthemyDevelopment.Save;
using UnityEngine;

public class ExampleData : MonoBehaviour
{
    
    //The SaveData instance declaration
    SaveData saveData = new SaveData();

    
    //The different variables that are compatible with the system
    [SerializeField]public Example ExampleCustomClass;
    public string ExampleString;
    public bool ExampleBool;
    public int ExampleInt;
    public float ExampleFloat;
    public Vector2 ExampleVector2;
    public Vector3 ExampleVector3;

    //An enum is recommended for the keys of the saved values
    enum ValuesKeys
    {
        PlayerName,
        SuperJump,
        Hp,
        Damage,
        MinimapPosition,
        Position,
        CustomClass
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Saving the variables value and link them to their key
            saveData.SaveValue(ValuesKeys.PlayerName, ExampleString);
            saveData.SaveValue(ValuesKeys.SuperJump, ExampleBool);
            saveData.SaveValue(ValuesKeys.Hp, ExampleInt);
            saveData.SaveValue(ValuesKeys.Damage, ExampleFloat);
            saveData.SaveValue(ValuesKeys.MinimapPosition, ExampleVector2);
            saveData.SaveValue(ValuesKeys.Position, ExampleVector3);
            saveData.SaveValue(ValuesKeys.CustomClass, ExampleCustomClass);
            
            //Saving the file after every variable has been saved
            saveData.SaveDataFile();

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //Loading the save file
            saveData.LoadDataFile();
            
            //Retrieving the values to their variables using the keys
            saveData.LoadValue(ValuesKeys.PlayerName, out ExampleString);
            saveData.LoadValue(ValuesKeys.SuperJump, out ExampleBool);
            saveData.LoadValue(ValuesKeys.Hp, out ExampleInt);
            saveData.LoadValue(ValuesKeys.Damage, out ExampleFloat);
            saveData.LoadValue(ValuesKeys.MinimapPosition, out ExampleVector2);
            saveData.LoadValue(ValuesKeys.Position, out ExampleVector3);
            saveData.LoadValue(ValuesKeys.CustomClass, out ExampleCustomClass);
            
            
        }
        
        
        
    }

}

[System.Serializable]
public class Example : ISaveClass //A class compatible with the system thanks to the ISaveClass interface
{
    public int PlayerIndex;
    public string PlayerTag;

}
