using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Omedya.Lib.Extensions;

namespace Omedya.Scripts.Core.Shared.ModuleSystem
{
    public class ModuleStore<TBaseModule>
        where TBaseModule : class
    {
        private readonly IList<TBaseModule> _modules = new List<TBaseModule>();
        private readonly Dictionary<string, TBaseModule> _moduleDict = new Dictionary<string, TBaseModule>();

        public TModule GetModule<TModule>() where TModule : TBaseModule
        {
            var moduleType = typeof(TModule);

            if (_moduleDict.TryGetValue(moduleType.Name, out TBaseModule module))
            {
                return (TModule)module;
            }

            foreach (var moduleItem in _modules)
            {
                var moduleItemType = moduleItem.GetType();
                if (!moduleType.IsAssignableFrom(moduleItemType) &&
                    (!moduleType.IsGenericType || moduleItemType.IsGenericImplemented(moduleType)))
                    continue;

                module = moduleItem;
                _moduleDict.Add(moduleType.Name, module);
                return (TModule)module;
            }

            return default;
        }

        public void AddModule<TModule>(TModule module) where TModule : TBaseModule
        {
            _modules.Add(module);
        }

        public int Count => _modules.Count;

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public readonly struct Enumerator : IEnumerator<TBaseModule>
        {
            private readonly IEnumerator<TBaseModule> _enumerator;

            public Enumerator(ModuleStore<TBaseModule> moduleStore)
            {
                _enumerator = moduleStore._modules.GetEnumerator();
            }

            public TBaseModule Current => _enumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose() => _enumerator.Dispose();
            public bool MoveNext() => _enumerator.MoveNext();
            public void Reset() => _enumerator.Reset();
        }
    }
}