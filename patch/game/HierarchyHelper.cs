using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SCPlus.patch.hierarchy
{
    internal static class HierarchyHelper
    {
        internal static GameObject root;
        internal static GameObject Root => root ??= GameObject.Find("UI Root (2D)");

        internal static bool TryFindWithLogging(Transform transform, string name, out Transform result)
        {
            if (!TryFind(transform, name, out result))
            {
                Plugin.Logger.LogError($"Could not find \"{name}\" in {transform}");
                return false;
            }

            return true;
        }

        internal static bool TryFind(Transform transform, string name, out Transform result)
        {
            result = transform.Find(name);
            return result != null;
        }

        internal static bool TryFindComponentRoot<T>(out T result)
        {
            return TryFindComponent(Root.transform, out result);
        }

        internal static bool TryFindComponent<T>(Transform root, out T result)
        {
            Queue<Transform> queue = new();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Transform current = queue.Dequeue();
                if (current.TryGetComponent(out result))
                {
                    return true;
                }

                foreach (Transform child in current)
                {
                    queue.Enqueue(child);
                }
            }

            result = default;
            return false;
        }

        internal static bool TryFindComponentWithLogging<T>(Transform root, out T result)
        {
            if (!TryFindComponent(root, out result))
            {
                Plugin.Logger.LogError($"Could not find component {nameof(T)} in {root}");
                return false;
            }

            return true;
        }

        internal static void Parent(Transform child, Transform parent)
        {
            child.SetParent(parent);
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            child.localScale = Vector3.one;
        }

        internal static T EnsureComponent<T>(GameObject gameObject) where T : MonoBehaviour
        {
            if (gameObject == null)
            {
                return null;
            }

            if (!gameObject.TryGetComponent(out T component))
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        internal static void SwitchComponentWithSub<TParent, TChild>(bool addFields, bool addProperties, TParent oldComponent, out TChild component) where TParent : MonoBehaviour where TChild : TParent
        {
            GameObject gameObject = oldComponent.gameObject;
            component = gameObject.AddComponent<TChild>();

            Type parentType = typeof(TParent);

            if (addFields)
            {
                FieldInfo[] fields = parentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    field.SetValue(component, field.GetValue(oldComponent));
                }
            }

            if (addProperties)
            {
                PropertyInfo[] properties = parentType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanWrite || !property.CanRead)
                    {
                        continue;
                    }

                    property.SetValue(component, property.GetValue(oldComponent));
                }
            }

            GameObject.DestroyImmediate(oldComponent);
        }
    }
}
