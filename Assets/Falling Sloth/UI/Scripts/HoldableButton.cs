using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FallingSloth.UI
{
#if UNITY_EDITOR
    [AddComponentMenu("FallingSloth/UI/Holdable Button")]
#endif
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class HoldableButton : Button, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsDown { get; private set; }

        public override void OnPointerDown(PointerEventData data)
        {
            IsDown = true;
            Debug.Log("Button pressed!");

            base.OnPointerDown(data);
        }
        
        public override void OnPointerUp(PointerEventData data)
        {
            IsDown = false;
            Debug.Log("Button released!");

            base.OnPointerUp(data);
        }
    }
}