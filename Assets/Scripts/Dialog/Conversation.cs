using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Line 
{
    public Character character;
   
    [TextArea(2, 5)]
   // Variável que representa o texto do qual o personagem irá falar
    public string text;
}

[CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation")]
public class Conversation : ScriptableObject
{
    // Propriedades que ditam quem irá falar em cada lado da tela
    public Character speakerLeft;
    public Character speakerRight;
    
    // Representa as linhas de cada fala
    public Line[] lines;

}