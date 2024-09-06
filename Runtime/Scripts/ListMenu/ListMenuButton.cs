using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor.UI;
using UnityEditor;
#endif

namespace m039.UIToolbox
{
    public class ListMenuButton : Button
    {
        #region Inspector

        [SerializeField]
        TransitionState _NormalState;

        [SerializeField]
        TransitionState _HighlightedState;

        [SerializeField]
        TransitionState _PressedState;

        [SerializeField]
        TransitionState _SelectedState;

        [SerializeField]
        TransitionState _DisabledState;

        [SerializeField]
        SelectionState _DebugSelectionState;

        #endregion

        [Serializable]
        class TransitionState
        {
            public Color textColor = Color.white;
            public TMPro.TMP_FontAsset font;
            public Sprite iconSprite;
            public Color backgroundColor = Color.white;
            public Color innerBorderColor = Color.white;
            public Color outerBorderColor = Color.black;
        }

        TMPro.TMP_Text _text;

        Image _outerBorderLayer;

        Image _innerBorderLayer;

        Image _topLayer;

        Image _icon;

        bool _inited = false;

        protected override void Awake()
        {
            base.Awake();

            Init();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (Application.isPlaying)
            {
                return;
            }

            DoStateTransitionInternal(_DebugSelectionState);
        }
#endif

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            DoStateTransitionInternal(state);
        }

        void DoStateTransitionInternal(SelectionState state)
        {
            Init();

            TransitionState transition;

            switch (state)
            {
                case SelectionState.Disabled:
                    transition = _DisabledState;
                    break;
                case SelectionState.Highlighted:
                    transition = _HighlightedState;
                    break;
                case SelectionState.Pressed:
                    transition = _PressedState;
                    break;
                case SelectionState.Selected:
                    transition = _SelectedState;
                    break;
                default:
                case SelectionState.Normal:
                    transition = _NormalState;
                    break;
            }

            if (_text != null)
            {
                _text.color = transition.textColor;
                _text.font = transition.font;
            }

            if (_outerBorderLayer != null)
            {
                _outerBorderLayer.color = transition.outerBorderColor;
            }

            if (_innerBorderLayer != null)
            {
                _innerBorderLayer.color = transition.innerBorderColor;
            }

            if (_topLayer != null)
            {
                _topLayer.color = transition.backgroundColor;
            }

            if (_icon != null)
            {
                _icon.sprite = transition.iconSprite;
            }
        }

        void Init()
        {
            if (_inited)
                return;

            _text = transform.Find("Text")?.GetComponent<TMPro.TMP_Text>();
            _outerBorderLayer = transform.Find("OuterBorderLayer")?.GetComponent<Image>();
            _innerBorderLayer = transform.Find("InnerBorderLayer")?.GetComponent<Image>();
            _topLayer = transform.Find("TopLayer")?.GetComponent<Image>();
            _icon = transform.Find("Icon")?.GetComponent<Image>();

            _inited = true;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ListMenuButton), true)]
    [CanEditMultipleObjects]
    public class ListMenuButtonEditor : ButtonEditor
    {
        readonly List<SerializedProperty> _properties = new();

        protected override void OnEnable()
        {
            base.OnEnable();

            _properties.Clear();

            foreach (var propertyPath in new [] {
                "_DebugSelectionState",
                "_NormalState",
                "_HighlightedState",
                "_PressedState",
                "_SelectedState",
                "_DisabledState"
            })
            {
                _properties.Add(serializedObject.FindProperty(propertyPath));
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            foreach (var property in _properties)
            {
                EditorGUILayout.PropertyField(property);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
