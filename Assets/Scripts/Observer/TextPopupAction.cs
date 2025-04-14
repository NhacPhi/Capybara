using System;
using UnityEngine;

namespace Observer
{
    public static class TextPopupAction
    {
        public static Action<float, Vector3> DamagePopup;
        public static Action<float, Vector3> HealPopup;
    }
}
