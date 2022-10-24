using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mz.ExpressionTools;

namespace Plugins.Mezzanine.DataModels.Editor.Tests.Collections
{
    public class Character
    {
        public string Name { get; private set; }
        public Weapon MainWeapon => Weapons[0];
        public List<Weapon> Weapons { get; private set; }
        public Dictionary<string, Pet> Pets { get; private set; }

        public Character()
        {
            Weapons = new List<Weapon>();
            Pets = new Dictionary<string, Pet>();
        }
        public Character(string name) : this()
        {
            Name = name;
        }
    }

    public enum WeaponType
    {
        Pistol,
        AssaultRifle,
        Shotgun,
        SniperRifle
    }

    public class Weapon
    {
        public WeaponType Type { get; private set; }
        public int AmmoMax { get; private set; }
        public int AmmoCurrent { get; private set; }
        public List<WeaponSkin> Skins { get; private set; }

        public Weapon()
        {
            Skins = new List<WeaponSkin>();
        }
        public Weapon(WeaponType type, int ammoMax, int ammoCurrent) : this()
        {
            Type = type;
            AmmoMax = ammoMax;
            AmmoCurrent = ammoCurrent > -1 ? ammoCurrent : ammoMax;
        }
    }

    public enum WeaponSkinType
    {
        JungleCamo,
        BlueLightning,
        SwampApe
    }
    
    public class WeaponSkin
    {
        public WeaponSkinType Type { get; private set; }
        public int[] Measurements { get; private set; }

        public WeaponSkin(WeaponSkinType type)
        {
            Type = type;
            Measurements = new [] {3, 8, 12, 4};
        }
    }

    public enum PetType
    {
        Tyranosaurus,
        Narwhal
    }

    public class Pet
    {
        public string Name { get; private set; }
        public PetType Type { get; private set; }
        public Weapon[] Weapons { get; private set; }

        public Pet(string name, PetType type)
        {
            Name = name;
            Type = type;
            Weapons = new Weapon[]
            {
                new Weapon(WeaponType.Pistol, 16, 16),
                new Weapon(WeaponType.Shotgun, 12, 12) 
            };
        }
    }

    [TestFixture]
    public class Test
    {
        [Test]
        public void Simple()
        {
            var character = new Character("Bill");
            var weapon = new Weapon(WeaponType.Pistol, 10, 6);
            weapon.Skins.Add(new WeaponSkin(WeaponSkinType.JungleCamo));
            weapon.Skins.Add(new WeaponSkin(WeaponSkinType.BlueLightning));
            character.Weapons.Add(weapon);
            var pet = new Pet("Rexi", PetType.Tyranosaurus);
            character.Pets.Add(pet.Name, pet);

            Expression<Func<Character, int>> expression = data => data.Pets["Rexi"].Weapons[1].AmmoMax;

            var chain = Expressions.GetExpressionChain(expression);
            Debug.Log($"path: {chain.Path}");
            
            var func = ((Expression<Func<Character, int>>)chain.Expression).Compile();
            var result = func(character);
            Debug.Log($"The character's pet's second weapon has max ammo: {result}");

            // Member
            var link = chain.Links[chain.Links.Count - 1];
            Debug.Log($"link key: {link.Key}, type: {link.Type}");
            var propertyInfoPets = character.GetType().GetProperty(link.Key);
            var pets = (Dictionary<string, Pet>)propertyInfoPets.GetValue(character);
            Debug.Log($"Pet count: {pets.Count}");

            // CollectionItemAccessor
            link = chain.Links[chain.Links.Count - 2];
            Debug.Log($"link key: {link.Key}, type: {link.Type}");
            var call = (MethodCallExpression)link.Expression;
            var index = ((ConstantExpression)call.Arguments[0]).Value;
            var petInstance = (Pet)call.Method.Invoke(pets, new object[] { index });
            Debug.Log($"Pet type: {petInstance.Type}");
            
            // Array
            link = chain.Links[chain.Links.Count - 3];
            Debug.Log($"link key: {link.Key}, type: {link.Type}");
            var index2 = (int)((ConstantExpression)((BinaryExpression)link.Expression).Right).Value;
            var expressionArray = (MemberExpression)((BinaryExpression)link.Expression).Left;
            link.Key = $"{expressionArray.Member.Name}[{index2}]";
            var propertyInfoArray = petInstance.GetType().GetProperty(expressionArray.Member.Name);
            var scoreArray = propertyInfoArray.GetValue(petInstance);
            var expressionArrayIndex = Expression.ArrayIndex(expressionArray, new Expression[] { ((BinaryExpression)link.Expression).Right });
            Debug.Log($"score: {expressionArrayIndex.Method.Invoke(scoreArray, new object[] { index2 }) }");

            Debug.Log($"scoreArray type: {scoreArray.GetType().GetElementType()}");

            chain.Set(character, 2);
            Debug.Log($"Rexi's new max ammo: {character.Pets["Rexi"].Weapons[1].AmmoMax}, previous: {chain.ValuePrevious}, new: {chain.ValueNew}");
        }

        [Test]
        public void ArrayAccess()
        {
            // This parameter expression represents a variable that will hold the array.
            var arrayExpr = Expression.Parameter(typeof(object[]), "Array");
            
            // This parameter expression represents an array index.            
            var indexExpr = Expression.Parameter(typeof(int), "Index");

            // This parameter represents the value that will be added to a corresponding array element.
            var valueExpr = Expression.Parameter(typeof(object), "Value");

            // This expression represents an array access operation.
            // It can be used for assigning to, or reading from, an array element.
            Expression arrayAccessExpr = Expression.ArrayAccess(
                arrayExpr,
                indexExpr
            );

            // This lambda expression assigns a value provided to it to a specified array element.
            // The array, the index of the array element, and the value to be added to the element
            // are parameters of the lambda expression.
            var lambdaExpr = Expression.Lambda<Func<object[], int, object, object>>(
                Expression.Assign(arrayAccessExpr, valueExpr),
                arrayExpr,
                indexExpr,
                valueExpr
            );

            // Print out expressions.
            Debug.Log("Array Access Expression:");
            Debug.Log(arrayAccessExpr.ToString());

            Debug.Log("Lambda Expression:");
            Debug.Log(lambdaExpr.ToString());

            Debug.Log("The result of executing the lambda expression:");

            // The following statement first creates an expression tree,
            // then compiles it, and then executes it.
            // Parameters passed to the Invoke method are passed to the lambda expression.
            Debug.Log(lambdaExpr.Compile().Invoke(new object[] { 10, 20, 30 }, 0, 5));
        }
    }
}