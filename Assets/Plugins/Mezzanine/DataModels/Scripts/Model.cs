using System;
using System.IO;
using System.Linq.Expressions;

#if (!UNITY_IOS && !UNITY_WEBGL)
using System.Threading.Tasks;
#endif

using Mz.ExpressionTools;
using Mz.Identifiers;
using Mz.Serializers;
using Mz.TypeTools;

namespace Mz.Models
{
    public class ModelDefaults
    {
        public bool IsTriggerChangedEvent = true;
        public bool IsTriggerAutoSave = false;
    }
    
    public class Model<TData> where TData : class, new()
    {
        public Model()
        {
            Delimiter = '.';
            Key = Identifier.Get();
            Defaults = new ModelDefaults();

#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
            _constructorData = Model.GetConstructor<TData>();
#endif
            
            Initialize(_getDataInstance());
        }
        
        public Model(object key, ModelSaveOptions saveOptions = null) : this()
        {
            Key = key;
            SaveOptions = saveOptions ?? new ModelSaveOptions(Key?.ToString());
        }
        
        public Model(TData initialValues, object key = null, ModelSaveOptions saveOptions = null) : this()
        {
            if (key != null) Key = key;
            if (initialValues != null) Initialize(initialValues);
            SaveOptions = saveOptions ?? new ModelSaveOptions(Key.ToString());
        }

        public void Initialize(
            TData data, 
            object key = null, 
            ModelSaveOptions saveOptions = null
        )
        {
            _data = data ?? new TData();
            _parallel = new ModelParallel<TData>(this, _data);
            if (key != null) Key = key;
            SaveOptions = saveOptions ?? new ModelSaveOptions(Key.ToString());
        }
        
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
        private Types.ConstructorDelegate<TData> _constructorData;
#endif
        
        public ModelDefaults Defaults { get; private set; }
        
        public ModelSaveOptions SaveOptions { get; private set; }

        public bool IsInitialized => _data != null && _data.GetType() == typeof(TData);
        public bool IsAutoSave { get; set; } = true;

        private TData _data;

        public bool IsCloneDataOnAccess { get; set; }
        public TData Data => IsCloneDataOnAccess ? CloneData() : _data;

        public object Key { get; private set; }

        private ModelParallel<TData> _parallel;
        public ModelParallel<TData> Parallel => _parallel;

        public char Delimiter { get; set; }

        public delegate void SetEventHandler(ModelChangedEventArgs<TData> args);
        public event SetEventHandler Changed;
        public void DoChanged(ModelChangedEventArgs<TData> changedArgs)
        {
            var handlerChanged = Changed;
            handlerChanged?.Invoke(changedArgs);
        }
        
        public delegate void SaveEventHandler(Model<TData> model, SerializeResult args);
        public event SaveEventHandler Saved;

        /// <summary>
        /// Set a single value on the data held by this model.
        /// </summary>
        /// <param name="propertyAccessorFunc">Indicates the property you wish to modify in the form <code>current => current.PropertyName</code></param>
        /// <param name="value">The value you'd like to apply to the given property</param>
        /// <param name="isTriggerChangedEvent">Set to `false` if you don't want to trigger a `Changed` event.</param>
        /// /// <param name="isTriggerAutoSave">Set to `false` if you don't want to trigger a `Save` action.</param>
        /// <returns>Returns a clone of the modified object</returns>
        public Model<TData> Set<TValue>(
            Expression<Func<TData, TValue>> propertyAccessorFunc, 
            TValue value,
            bool? isTriggerAutoSave = null,
            bool? isTriggerChangedEvent = null
        )
        {
            isTriggerChangedEvent = isTriggerChangedEvent ?? Defaults.IsTriggerChangedEvent;
            isTriggerAutoSave = isTriggerAutoSave ?? Defaults.IsTriggerAutoSave;
            
            var chain = Expressions.GetExpressionChain(propertyAccessorFunc);
            chain.Set(_data, value);

            if (!isTriggerChangedEvent.Value) return this;
            
            var modelChange = new ModelChange(chain.Path, chain.ValueNew, chain.ValuePrevious);
            var changedArgs = new ModelChangedEventArgs<TData>(this);
            changedArgs.Add(modelChange);
            Changed?.Invoke(changedArgs);
            
            if (IsAutoSave && isTriggerAutoSave.Value) Save();

            return this;
        }
        
        public Model<TData> Set<TValue>(
            IExpressionChain chain, 
            TValue value,
            bool? isTriggerChangedEvent = null,
            bool? isTriggerAutoSave = null
        )
        {
            isTriggerChangedEvent = isTriggerChangedEvent ?? Defaults.IsTriggerChangedEvent;
            isTriggerAutoSave = isTriggerAutoSave ?? Defaults.IsTriggerAutoSave;
            
            chain.Set(_data, value);
  
            if (!isTriggerChangedEvent.Value) return this;
            
            var modelChange = new ModelChange(chain.Path, chain.ValueNew, chain.ValuePrevious);
            var changedArgs = new ModelChangedEventArgs<TData>(this);
            changedArgs.Add(modelChange);
            Changed?.Invoke(changedArgs);

            if (IsAutoSave && isTriggerAutoSave.Value) Save();
            
            return this;
        }

#if (!UNITY_IOS && !UNITY_WEBGL)
        public async Task<Model<TData>> Evaluate(
            Action<TData> action,
            bool? isTriggerChangedEvent = null,
            bool? isTriggerAutoSave = null
        )
        {
            isTriggerChangedEvent = isTriggerChangedEvent ?? Defaults.IsTriggerChangedEvent;
            isTriggerAutoSave = isTriggerAutoSave ?? Defaults.IsTriggerAutoSave;
            
            await Task.Run(() =>
            {
                action.Invoke(_data);
            });
            
            var changedArgs = new ModelChangedEventArgs<TData>(this);

            if (!isTriggerChangedEvent.Value) return this;
            
            Changed?.Invoke(changedArgs);
            
            if (IsAutoSave && isTriggerAutoSave.Value) Save();

            return this;
        }
#endif

        private TData _getDataInstance()
        {
#if (REFLECTION_EMIT && UNITY_IOS && !UNITY_WEBGL)
            return _constructorData();
#else
            return Activator.CreateInstance<TData>();        
#endif
        }
        public TData CloneData()
        {
            if (!IsInitialized) return _getDataInstance();
            
            var properties = Types.GetReadWriteProperties(typeof(TData));
            var newInstance = _getDataInstance();

            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetValue(_data);
                propertyInfo.SetValue(newInstance,  value);
            }

            return newInstance;
        }

        public bool Save(
            string fileName = null,
            string directoryPath = null,
            bool isRelativePath = true,
            DataFormat dataFormat = DataFormat.Json,
            string fileExtension = null,
            bool isPrettyPrint = false,
            bool isThrowExceptions = true,
            bool isUnity = true
        )
        {
            if (SaveOptions == null)
            {
                SaveOptions = new ModelSaveOptions(
                    fileName,
                    directoryPath,
                    isRelativePath,
                    dataFormat,
                    isPrettyPrint,
                    isThrowExceptions
                );

                SaveOptions.FileExtension = fileExtension;
            }
            else
            {
                fileName = fileName ?? SaveOptions.FileName;
                
                if (isUnity) fileExtension = "txt";
                else fileExtension = fileExtension ?? SaveOptions.FileExtension ?? "txt";
                
                directoryPath = directoryPath ?? SaveOptions.DirectoryPath;
                if (fileName == null)
                {
                    isRelativePath = SaveOptions.IsRelativePath;
                    isPrettyPrint = SaveOptions.IsPrettyPrint;
                    isThrowExceptions = SaveOptions.IsThrowExceptions;
                }
            }
            
            var serializeResult = new SerializeResult();
            if (isUnity)
            {
                serializeResult = Model.Files.Serialize(_data, isRelativePath ? directoryPath : "", fileName, dataFormat, isPrettyPrint);
            }
            else
            {
                if (directoryPath != null && isRelativePath)
                    directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath);

                serializeResult = Mz.Serializers.Data.Serialize(
                    _data,
                    fileName ?? Key.ToString(),
                    fileExtension,
                    directoryPath,
                    isRelativePath,
                    dataFormat,
                    isPrettyPrint,
                    isThrowExceptions
                );
            }

            if (serializeResult.IsSuccess)
            {
                Saved?.Invoke(this, serializeResult);
            }

            return serializeResult.IsSuccess;
        }
    }
}