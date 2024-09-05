using m039.Common.DependencyInjection;
using UnityEngine;

namespace Game
{
    public class FPSCounter : MonoBehaviour, IDependencyProvider
    {
        const float HighLoadFPS = 10f;

        const float MediumLoadFPS = 45f;

        #region Inspector

        [SerializeField]
        Color _NormalLoadColor = new Color32(0x4f, 0xe7, 0x4c, 0xff);

        [SerializeField]
        Color _MediumLoadColor = new Color32(0xec, 0xf1, 0x4a, 0xff);

        [SerializeField]
        Color _HighLoadColor = new Color32(0xde, 0x62, 0x4f, 0xff);

        #endregion

        [Provide]
        FPSCounter GetFPSCounter() => this;

        TMPro.TMP_Text _text;

        const  int SamplesCount = 5;

        float _totalTime = 0;

        int _count;

        void Awake()
        {
            _text = transform.Find("Text").GetComponent<TMPro.TMP_Text>();
            _text.text = "FPS: ###";
            _text.color = Color.white;
            _count = SamplesCount;
        }

        void Update()
        {
            _count -= 1;
            _totalTime += Time.deltaTime;

            if (_count <= 0)
            {
                var fps = (int)(SamplesCount / _totalTime);
                var color = _NormalLoadColor;

                if (fps < HighLoadFPS)
                {
                    color = _HighLoadColor;
                }
                else if (fps < MediumLoadFPS)
                {
                    color = _MediumLoadColor;
                }

                _text.text = string.Format("FPS: {0,4:f1} ({1,3:0.0}ms)", fps, 1f / fps * 1000f);
                _text.color = color;
                _totalTime = 0;
                _count = SamplesCount;
            }
        }
    }
}
