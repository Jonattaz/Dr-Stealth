using NUnit.Framework;
using System;
using System.Linq.Expressions;
using Mz.Models;
using UnityEngine;

namespace Plugins.Mezzanine.DataModels.Editor.Tests.Packs
{
    public class ModelPacks : Model<PacksData>
    {
        public int Count => Data.Packs.Length;
        public ModelPack Pack { get; private set; }
        
        public void SetLevel(int pack, int level)
        {
            if (pack > Count) pack = Count;
            if (pack < 1) pack = 1;

            var packData = Data.Packs[pack - 1];
            Pack = Model.Create<ModelPack, PackData>(packData);

            if (level > Pack.LevelCount) level = Pack.LevelCount;
            if (level < 1) level = 1;
                
            Parallel.Set(data => data.PackCurrent, pack).
                Set(data => data.LevelCurrent, level).
                Build();
        }
        
        public void NextLevel()
        {
            SetLevel(Data.PackCurrent, Data.LevelCurrent + 1);
        }
    }

    public class ModelPack : Model<PackData>
    {
        public int LevelCount => Data.Levels.Length;
    }

    public class PacksData
    {
        public PackData[] Packs { get; private set; }
        public int PackCurrent { get; private set; }
        public int LevelCurrent { get; private set; }
    }
    
    public class PackData
    {
        public int Key { get; private set; }
        public string Label { get; private set; }
        public bool IsPurchased { get; private set; }
        public int[] Levels { get; private set; }
        public string Description { get; private set; }
    }

    [TestFixture]
    public class TestPacks
    {
        [Test]
        public void Simple()
        {
            var fileName = "packs";
            Model.IsUnity = true;
            var model = Model.LoadResource<PacksData>(fileName, "Mz_Data_Tests");

            Debug.Log($"Pack count: {model.Data.Packs}");
            Debug.Log($"Packs[0].Levels[0]: {model.Data.Packs[0].Levels[0]}");
            model.Set(data => data.Packs[0].Levels[0], 2);
            Debug.Log($"Packs[0].Levels[0]: {model.Data.Packs[0].Levels[0]}");
        }

        [Test]
        public void ArrayAccessExpression()
        {
            var arrayAssign = CreateArrayAssignmentExpression<int>();

            // The following statement first creates an expression tree,
            // then compiles it, and then executes it.
            // Parameters passed to the Invoke method are passed to the lambda expression.
            var array = new int[] {10, 20, 30};
            arrayAssign.Compile().Invoke(array, 0, 5);
            Debug.Log($"new value: {array[0]}");

            var arrayAccess = CreateArrayAccessExpression<int>();
            var value = arrayAccess.Compile().Invoke(array, 1);
            Debug.Log($"value at index 1: {value}");
        }
        
        public static Expression<Func<TArrayElement[], int, TArrayElement>> CreateArrayAccessExpression<TArrayElement>() 
        {
            // This parameter expression represents a variable that will hold the array.
            ParameterExpression arrayExpr = Expression.Parameter(typeof(TArrayElement[]), "Array");

            // This parameter expression represents an array index.            
            ParameterExpression indexExpr = Expression.Parameter(typeof(int), "Index");

            // This expression represents an array access operation.
            // It can be used for assigning to, or reading from, an array element.
            Expression arrayAccessExpr = Expression.ArrayAccess(
                arrayExpr,
                indexExpr
            );
                
            // This lambda expression assigns a value provided to it to a specified array element.
            // The array, the index of the array element, and the value to be added to the element
            // are parameters of the lambda expression.
            Expression<Func<TArrayElement[], int, TArrayElement>> lambdaExpr = Expression.Lambda<Func<TArrayElement[], int, TArrayElement>>(
                arrayAccessExpr,
                arrayExpr,
                indexExpr
            );

            return lambdaExpr;
        }

        public static Expression<Action<TArrayElement[], int, TArrayElement>> CreateArrayAssignmentExpression<TArrayElement>() 
        {
            // This parameter expression represents a variable that will hold the array.
            ParameterExpression arrayExpr = Expression.Parameter(typeof(TArrayElement[]), "Array");

            // This parameter expression represents an array index.            
            ParameterExpression indexExpr = Expression.Parameter(typeof(int), "Index");

            // This parameter represents the value that will be added to a corresponding array element.
            ParameterExpression valueExpr = Expression.Parameter(typeof(TArrayElement), "Value");

            // This expression represents an array access operation.
            // It can be used for assigning to, or reading from, an array element.
            Expression arrayAccessExpr = Expression.ArrayAccess(
                arrayExpr,
                indexExpr
            );
                
            // This lambda expression assigns a value provided to it to a specified array element.
            // The array, the index of the array element, and the value to be added to the element
            // are parameters of the lambda expression.
            Expression<Action<TArrayElement[], int, TArrayElement>> lambdaExpr = Expression.Lambda<Action<TArrayElement[], int, TArrayElement>>(
                Expression.Assign(arrayAccessExpr, valueExpr),
                arrayExpr,
                indexExpr,
                valueExpr
            );

            return lambdaExpr;
        }
    }
}