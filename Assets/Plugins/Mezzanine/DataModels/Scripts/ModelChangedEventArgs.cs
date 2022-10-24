#if (!UNITY_IOS && !UNITY_WEBGL)
using System.Collections.Concurrent;
#endif

using System.Collections.Generic;

namespace Mz.Models
{
    public class ModelChangedEventArgs<TData> where TData : class, new()
    {
        public ModelChangedEventArgs(Model<TData> model)
        {
            Model = model;
            
#if (!UNITY_IOS && !UNITY_WEBGL)
            _changes = new ConcurrentDictionary<string, ModelChange>();
#else
            _changes = new Dictionary<string, ModelChange>();
#endif
        }

        public Model<TData> Model { get; }
        
#if (!UNITY_IOS && !UNITY_WEBGL)
        private ConcurrentDictionary<string, ModelChange> _changes;
#else
        private Dictionary<string, ModelChange> _changes;
#endif
        
        public List<ModelChange> Changes => new List<ModelChange>(_changes.Values);

        public void Add(ModelChange change)
        {
            if (_changes.ContainsKey(change.PropertyPath))
            {
                _changes.TryGetValue(change.PropertyPath, out var changeExisting);
                if (changeExisting != null) changeExisting.ValueCurrent = change.ValueCurrent;
            }
            else
            {
#if (!UNITY_IOS && !UNITY_WEBGL)
                _changes.TryAdd(change.PropertyPath, change);
#else
                _changes.Add(change.PropertyPath, change);
#endif
            }
        }
    }
}