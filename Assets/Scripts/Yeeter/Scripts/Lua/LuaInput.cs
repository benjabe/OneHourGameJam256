using MoonSharp.Interpreter;
using UnityEngine.Scripting;

namespace Yeeter
{
    [Preserve, MoonSharpUserData]
    public class LuaInput
    {
        public static void AddOnKeyDown(string keyCode, DynValue action, int order = 0)
        {
            BBInput.AddOnKeyDown(keyCode, () => { LuaManager.Call(action); }, order);
        }
        public static void AddOnKeyUp(string keyCode, DynValue action, int order = 0)
        {
            BBInput.AddOnKeyUp(keyCode, () => { LuaManager.Call(action); }, order);
        }
        public static void AddOnKey(string keyCode, DynValue action, int order = 0)
        {
            BBInput.AddOnKey(keyCode, () => { LuaManager.Call(action); }, order);
        }
    }
}