using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    namespace Utilities
    {
        public static class EditorExtensions
        {
            public static Button FindFrist(this Button[] buttons, string name)
            {
                return buttons.FirstOrDefault(x => x.name.ToLower().Contains(name.ToLower()));
            }
 
            public static T FindFrist<T>(this Transform[] transforms, string name) where T : Component
            {
                foreach (var transform in transforms)
                {
                    if(transform.name.ToLower().Contains(name.ToLower()) && transform.TryGetComponent(out T component)) 
                        return component;
                }

                return default;
            }

            public static List<T> Find<T>(this Transform[] transforms, string name)
            {
                var result = new List<T>();
                foreach (var transform in transforms)
                {
                    if (transform.name.ToLower().Contains(name.ToLower()) && transform.TryGetComponent(out T component))
                    {
                        result.Add(component);
                    }
                }
                return result;
            }
 
            public static TextMeshProUGUI FindFrist(this TextMeshProUGUI[] textMeshProUGUIs, string name)
            {
                return textMeshProUGUIs.FirstOrDefault(x => x.name.ToLower().Contains(name.ToLower()));
            }

            public static T FindComponentInProject<T>(string path)
            {
                return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path).GetComponent<T>();
            }
        }
    }
#endif