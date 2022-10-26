using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    // Propriedades que o Scriptable Object irá ter. Nome e foto
    public string fullName;
}