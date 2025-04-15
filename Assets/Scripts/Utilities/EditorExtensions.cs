using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    namespace Utilities
    {
        public static class EditorExtensions
        {
            public static T FindFirst<T>(this Transform[] transforms, string name)
            {
                foreach (Transform transform in transforms)
                {
                    if (transform.name.ToLower().Contains(name.ToLower()) &&
                        transform.TryGetComponent(out T component))
                        return component;
                }
                    
                return default;
            }

            public static Button FindFirst(this Button[] buttons, string name)
            {
                return buttons.FirstOrDefault(x => x.name.ToLower().Contains(name.ToLower()));
            }

            public static TextMeshProUGUI FindFirst(this TextMeshProUGUI[] textMeshProUGUIs, string name)
            {
                return textMeshProUGUIs.FirstOrDefault(x => x.name.ToLower().Contains(name.ToLower()));
            }
        }
    }
#endif