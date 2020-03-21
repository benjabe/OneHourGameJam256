using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace Yeeter
{
    /// <summary>
    /// Handles Lua stuff.
    /// </summary>
    [Preserve, MoonSharpUserData]
    public class LuaManager
    {
        private static Dictionary<int, LuaObjectComponent> _objects = new Dictionary<int, LuaObjectComponent>();

        /// <summary>
        /// True signals to Lua methods that they're being executed from the console. This is useful as the method may
        /// wish to log to the console if the comand is ran from the console but not otherwise.
        /// </summary>
        public static bool RunningConsoleCommand { get; set; } = false;

        /// <summary>
        /// Used so that stuff can be shared essentially.
        /// </summary>
        public static Script GlobalScript { get; set; } = CreateScript();

        /// <summary>
        /// Sets up a LuaObjectComponent.
        /// </summary>
        /// <param name="path">T</param>
        [MoonSharpHidden]
        public static void SetupLuaObject(LuaObjectComponent luaObject)
        {
            luaObject.Id = ObjectBuilder.GetId(luaObject.gameObject);
            _objects.Add(luaObject.Id, luaObject);
            luaObject.Script = GlobalScript;
        }

        /// <summary>
        /// Sets a table value in a LuaObject.
        /// </summary>
        /// <param name="id">The object's id.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to associate with the key.</param>
        public static void Set(int id, string key, DynValue value)
        {
            if (!_objects.ContainsKey(id))
            {
                Debug.LogError(
                    "LuaManager.Set(): " +
                    "LuaObject with id " + id + "was not found in LuaManager. " +
                    "Either the id was invalid or the LuaObject was created with LuaManager.CreateLuaObject().");
                return;
            }
            _objects[id].Script.Globals.Set(key, value);
        }

        /// <summary>
        /// Gets a table value in a LuaObject.
        /// </summary>
        /// <param name="id">The object's id.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the key.</returns>
        public static DynValue Get(int id, string key)
        {
            if (!_objects.ContainsKey(id))
            {
                Debug.LogError(
                    "LuaManager.Get(): " +
                    "LuaObject with id " + id + " was not found in LuaManager. " +
                    "Either the id was invalid or the LuaObject was created with LuaManager.CreateLuaObject().");
                return DynValue.Nil;
            }
            var table = _objects[id].Script.Globals;
            var value = table.Get(key);
            if (value.IsNil())
            {
                Debug.LogError(
                    "LuaManager.Get(): " +
                    "LuaObject with id " + id + ": " + key + " was not found in table.");
                return DynValue.Nil;
            }

            return value;
        }

        /// <summary>
        /// Gets the LuaObject id of a GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject's id.</param>
        /// <returns>The LuaObject id.</returns>
        public static DynValue GetLuaObjectId(int id)
        {
            var luaObject = _objects[id];
            return DynValue.NewNumber(ObjectBuilder.Get(id).GetComponent<LuaObjectComponent>().Id);
        }

        /// <summary>
        /// Loads and executes a string.
        /// </summary>
        /// <param name="code">The code to run.</param>
        /// <returns>The return value from the executed code.</returns>
        public static DynValue DoString(string code)
        {
            return GlobalScript.DoString(code);
            //var script = CreateScript();
            //return script.DoString(code);
        }
        /// <summary>
        /// Executes a function.
        /// </summary>
        /// <param name="function">The function to run.</param>
        /// <returns>The return value from the executed function.</returns>
        public static DynValue Call(DynValue function)
        {
            return GlobalScript.Call(function);
        }
        /// <summary>
        /// Executes a function.
        /// </summary>
        /// <param name="function">The function to run.</param>
        /// <param name="args">The arguments to pass to the function.</param>
        /// <returns>The return value from the executed function.</returns>
        public static DynValue Call(DynValue function, params DynValue[] args)
        {
            return GlobalScript.Call(function, args);
        }

        /// <summary>
        /// Creates a script set up with the globals we need.
        /// </summary>
        /// <returns>The created script.</returns>
        public static Action<Script> OnAboutToCreateScript;
         public static Script CreateScript()
        {
            UserData.RegisterAssembly();
            UserData.RegisterType<BBInput>();
            UserData.RegisterType<SceneManager>();
            UserData.RegisterType<Application>();
            var script = new Script();
            OnAboutToCreateScript?.Invoke(script);
            script.Globals["LuaManager"] = new LuaManager();
            script.Globals["InGameDebug"] = new InGameDebug();
            script.Globals["Console"] = new InGameDebug();
            script.Globals["ObjectBuilder"] = new ObjectBuilder();
            script.Globals["UI"] = new UI();
            script.Globals["Input"] = new LuaInput();
            script.Globals["SceneManager"] = new SceneManager();
            script.Globals["Application"] = new Application();
            script.Globals["SoundManager"] = new SoundManager();
            script.Globals["Assets"] = new StreamingAssetsDatabase();
            script.Globals["Gravity"] = new Gravity();

            foreach (var pair in script.Globals.Pairs)
            {
                Debug.Log(pair.Key + "   " + pair.Value);
            }
            return script;
        }
    }
}