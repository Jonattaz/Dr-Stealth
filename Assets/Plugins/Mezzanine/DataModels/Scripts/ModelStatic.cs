using System;
using Mz.Serializers;
using Mz.Unity;
using System.Collections.Generic;
using Mz.TypeTools;

namespace Mz.Models
{
    public static class Model
    {
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
        static Model()
        {
            _constructorDataCache = new Dictionary<Type, object>();
        }
        
        public static Dictionary<Type, object> _constructorDataCache;
        public static Types.ConstructorDelegate<TData> GetConstructor<TData>()
        {
            _constructorDataCache.TryGetValue(typeof(TData), out var constructorData);
            if (constructorData != null) return (Types.ConstructorDelegate<TData>)constructorData;
        
            constructorData = Types.GetConstructor<TData>();
            _constructorDataCache.Add(typeof(TData), constructorData);
            return (Types.ConstructorDelegate<TData>)constructorData;
        }
#endif
        
        private static Mz.Unity.Files _files;
        public static Mz.Unity.Files Files
        {
            get
            {
                if (_files == null) _files = new Files();
                return _files;
            }
        }

        public static bool IsUnity { get; set; } = true;

        public static Model<TData> Create<TData>(
            ModelSaveOptions saveOptions = null,
            object key = null
        ) where TData : class, new()
        {
            var model = new Model<TData>(key, saveOptions);
            return model;
        }
        public static Model<TData> Create<TData>(
            TData initialValues,
            ModelSaveOptions saveOptions = null,
            object key = null
        ) where TData : class, new()
        {
            var model = new Model<TData>(initialValues, key, saveOptions);
            return model;
        }
        
        public static TModel Create<TModel, TData>(
            ModelSaveOptions saveOptions = null,
            object key = null
        ) where TData : class, new()
            where TModel : Model<TData>
        {
            var model = Activator.CreateInstance<TModel>();
            model.Initialize(new TData(), key, saveOptions);
            return model;
        }
        public static TModel Create<TModel, TData>(
            TData initialValues,
            ModelSaveOptions saveOptions = null,
            object key = null
        )
            where TData : class, new()
            where TModel : Model<TData>
        {
            var model = Activator.CreateInstance<TModel>();
            model.Initialize(initialValues, key, saveOptions);
            return model;
        }
        
        public static Model<TData> Load<TData>(
            string fileName,
            ModelSaveOptions saveOptions = null,
            DataFormat dataFormat = DataFormat.Json,
            bool isThrowExceptions = true,
            object key = null
        ) where TData : class, new()
        {
            var instance = _Deserialize<TData>(
                fileName,
                "",
                true,
                dataFormat,
                "txt",
                isThrowExceptions
            );

            return new Model<TData>(instance, key, saveOptions);
        }
        
        public static Model<TData> LoadResource<TData>(
            string fileName,
            string directoryPath = null,
            ModelSaveOptions saveOptions = null,
            DataFormat dataFormat = DataFormat.Json,
            bool isThrowExceptions = true,
            object key = null
        ) where TData : class, new()
        {
            var instance = _Deserialize<TData>(
                fileName,
                directoryPath,
                true,
                dataFormat,
                "txt",
                isThrowExceptions,
                FileLocation.Resources
            );

            return new Model<TData>(instance, key, saveOptions);
        }
        
        public static TModel Load<TModel, TData>(
            string fileName,
            ModelSaveOptions saveOptions = null,
            DataFormat dataFormat = DataFormat.Json,
            bool isThrowExceptions = true,
            object key = null
        ) 
            where TData : class, new()
            where TModel : Model<TData>
        {
            var instance = _Deserialize<TData>(
                fileName,
                "",
                true,
                dataFormat,
                "txt",
                isThrowExceptions
            );
                
            var model = Activator.CreateInstance<TModel>();
            model.Initialize(instance, key, saveOptions);
            return model;
        }

        public static TModel LoadResource<TModel, TData>(
            string fileName,
            string directoryPath = null,
            ModelSaveOptions saveOptions = null,
            DataFormat dataFormat = DataFormat.Json,
            bool isThrowExceptions = true,
            object key = null
        )
            where TData : class, new()
            where TModel : Model<TData>
        {
            var instance = _Deserialize<TData>(
                fileName,
                directoryPath,
                true,
                dataFormat,
                "txt",
                isThrowExceptions,
                FileLocation.Resources
            );

            var model = Activator.CreateInstance<TModel>();
            model.Initialize(instance, key, saveOptions);
            return model;
        }

        public static TData _Deserialize<TData>(
            string fileName,
            string directoryPath = null,
            bool isRelativePath = true,
            DataFormat dataFormat = DataFormat.Json,
            string fileExtension = null,
            bool isThrowExceptions = true,
            FileLocation unityBaseDirectory = FileLocation.PersistentData
        )
        {
            var isSuccess = false;
            TData instance;
            
            if (IsUnity && unityBaseDirectory != FileLocation.None)
            {
                instance = Files.Deserialize<TData>(
                    isRelativePath ? directoryPath : "", 
                    fileName, 
                    unityBaseDirectory,
                    dataFormat
                );
                
                if (instance != null && instance.GetType() == typeof(TData)) isSuccess = true;
            }
            else
            {
                isSuccess = Mz.Serializers.Data.Deserialize(
                    out instance,
                    fileName,
                    fileExtension,
                    directoryPath,
                    isRelativePath,
                    dataFormat,
                    isThrowExceptions
                );
            }

            if (!isSuccess)
            {
                Console.WriteLine($"Failed to deserialize data for Model of type {typeof(TData)} at {directoryPath}/{fileName}.{fileExtension}");
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
                var constructorData = GetConstructor<TData>();
                instance = constructorData();
#else
                instance = Activator.CreateInstance<TData>();
#endif
            }

            return instance;
        }
    }
}