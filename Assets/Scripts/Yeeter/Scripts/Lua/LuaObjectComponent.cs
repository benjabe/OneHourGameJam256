using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yeeter
{
    /// <summary>
    /// A component which runs Unity methods from Lua.
    /// </summary>
    public class LuaObjectComponent : MonoBehaviour
    {
        public static Action<LuaObjectComponent> OnCreate;
        public static Action<LuaObjectComponent> OnClick;

        public Script Script { get; set; }
        public int Id { get; set; }

        private Table _table;
        private DynValue _start;
        private DynValue _update;

        [SerializeField] string _script;

        public void Load(string path)
        {
            LuaManager.SetupLuaObject(this);
            Script = LuaManager.GlobalScript;
            _table = Script.DoFile(path).Table;
            _start = _table.Get("Start");
            _update = _table.Get("Update");
        }

        private void Awake()
        {
            if (_script != "" && _script != null)
            {
                Load(_script);
            }
        }

        private void Start()
        {
            if (_start == null || _update == null)
            {
                Debug.LogError("LuaObjectComponent: Functions not loaded. Call Load() upon instantiation.");
            }
            if (_start.IsNotNil())
            {
                Script.Call(_start.Function, _table, Id);
            }
        }

        private void Update()
        {
            if (_update.IsNotNil())
            {
                Script.Call(_update.Function, Id);
            }
        }

        /// <summary>
        /// Gets a value from the Actor's table.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the key.</returns>
        public DynValue Get(string key)
        {
            return Script.Globals.Get(key);
        }

        /// <summary>
        /// Returns every pair in the actor's table.
        /// </summary>
        /// <returns>The pairs.</returns>
        public IEnumerable<TablePair> GetTablePairs()
        {
            return Script.Globals.Pairs;
        }
    }
}