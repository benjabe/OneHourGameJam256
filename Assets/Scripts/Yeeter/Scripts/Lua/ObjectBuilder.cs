using MoonSharp.Interpreter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Yeeter
{
    [Preserve, MoonSharpUserData]
    public class ObjectBuilder
    {
        private static int _nextId = 0;
        private static Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
        private static Dictionary<GameObject, int> _ids = new Dictionary<GameObject, int>();

        private static DynValue AddObject(GameObject gameObject)
        {
            _objects.Add(_nextId, gameObject);
            _ids.Add(gameObject, _nextId);
            gameObject.name = gameObject.name.Replace("(Clone)", "");
            InGameDebug.Log("Created GameObject '" + gameObject.name + "' with id " + _nextId + ".");
            return DynValue.NewNumber(_nextId++);
        }

        public static DynValue Instantiate(string path)
        {
            var go = Object.Instantiate(Resources.Load<GameObject>(path));
            return AddObject(go);
        }
        public static DynValue InstantiateUIElement()
        {
            var go = Object.Instantiate(new GameObject(), Object.FindObjectOfType<Canvas>().transform);
            go.AddComponent<RectTransform>();
            return AddObject(go);
        }
        public static DynValue InstantiateUIElement(string path)
        {
            var go = Object.Instantiate(
                Resources.Load<GameObject>("Prefabs/UI/" + path),
                Object.FindObjectOfType<Canvas>().transform);
            return AddObject(go);
        }
        public static DynValue Create(int x = 0, int y = 0)
        {
            var go = Object.Instantiate(new GameObject(), new Vector3(x, y), Quaternion.identity, null);
            return AddObject(go);
        }
        public static void Destroy(int id)
        {
            Object.Destroy(Get(id));
        }
        public static void SetName(int id, string name)
        {
            Get(id).name = name;
        }

        public static void AddLuaObjectComponent(int id, string type)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return;
            }
            var luaComponent = _objects[id].AddComponent<LuaObjectComponent>();
            luaComponent.Load(type);
        }
        public static void AddLuaObjectComponent(string id, string type)
        {
            var go = GameObject.Find(id);
            if (go == null)
            {
                InGameDebug.Log("Couldn't find GameObject with name '" + id + "'.");
                return;
            }
            var luaComponent = GameObject.Find(id).AddComponent<LuaObjectComponent>();
            luaComponent.Load(type);
        }

        public static void SetPosition(int id, float x, float y)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return;
            }
            _objects[id].transform.position = new Vector2(x, y);
        }
        public static void SetPosition(string id, float x, float y)
        {
            var go = GameObject.Find(id);
            if (go == null)
            {
                InGameDebug.Log("Couldn't find GameObject with name '" + id + "'.");
                return;
            }
            go.transform.position = new Vector2(x, y);
        }

        public static void SetParent(int childId, int parentId)
        {
            Get(childId).transform.SetParent(Get(parentId).transform, false);
        }

        public static void ToggleEnabled(int id)
        {
            var go = Get(id);
            go.SetActive(!go.activeInHierarchy);
        }
        [MoonSharpHidden]
        public static GameObject Get(int id)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return null;
            }
            return _objects[id];
        }
        [MoonSharpHidden]
        public static int GetId(GameObject gameObject)
        {
            if (!_ids.ContainsKey(gameObject))
            {
                InGameDebug.Log("GameObject '" + gameObject.name + "' has no id.");
                return -1;
            }
            return _ids[gameObject];
        }
    }
}