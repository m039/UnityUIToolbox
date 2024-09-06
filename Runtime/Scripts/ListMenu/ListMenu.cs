using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace m039.UIToolbox
{
    public class ListMenu : MonoBehaviour
    {
        public interface ITemplateProvider
        {
            List<(string text, System.Action onClick)> GetTemplates();
        }

        #region Inspector

        [SerializeField]
        bool _Main = false;

        [SerializeField]
        RectTransform _Frame;

        [SerializeField]
        RectTransform _Buttons;

        [SerializeField]
        RectTransform _ButtonTemplate;

        [SerializeField]
        RectTransform _CloseButton;

        [SerializeField]
        RectTransform _MenuButton;

        #endregion

        void Start()
        {
            CreateButtons();

            if (_Main)
            {
                _Frame.gameObject.SetActive(true);
                _CloseButton.gameObject.SetActive(false);
                _MenuButton.gameObject.SetActive(false);
            }
            else
            {
                _Frame.gameObject.SetActive(false);
                _CloseButton.gameObject.SetActive(true);
                _MenuButton.gameObject.SetActive(true);
            }
        }

        void CreateButtons()
        {
            var items = GetComponent<ITemplateProvider>().GetTemplates();

            foreach (Transform child in _Buttons)
            {
                if (child == _ButtonTemplate)
                    continue;

                Destroy(child.gameObject);
            }

            foreach (var (t, onClick) in items)
            {
                var button = Instantiate(_ButtonTemplate);
                button.SetParent(_Buttons, false);
                button.gameObject.SetActive(true);

                var text = button.Find("Text").GetComponent<TMPro.TMP_Text>();
                text.text = t;

                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    onClick?.Invoke();
                });
            }

            _ButtonTemplate.gameObject.SetActive(false);
        }

        public void OnCloseButtonClicked()
        {
            _Frame.gameObject.SetActive(false);
        }

        public void OnMenuClicked()
        {
            _Frame.gameObject.SetActive(!_Frame.gameObject.activeSelf);
        }
    }
}
