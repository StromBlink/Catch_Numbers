using UnityEngine;

namespace Voodoo.LevelDesign
{
    public static class ThemeUtils
    {
        /*Needed:
         * Apply color to whole gradient
         * LATER: Lerp gradient */

        private static GradientColorKey[] m_ColorKeysBuffer;
        private static GradientAlphaKey[] m_AlphaKeysBuffer;

        public static void MultiplyGradient(Gradient _Gradient, Color _Color)
        {
            if (m_ColorKeysBuffer == null)
                m_ColorKeysBuffer = new GradientColorKey[_Gradient.colorKeys.Length];
            else if (m_ColorKeysBuffer.Length != _Gradient.colorKeys.Length)
                m_ColorKeysBuffer = new GradientColorKey[_Gradient.colorKeys.Length];

            if (m_AlphaKeysBuffer == null)
                m_AlphaKeysBuffer = new GradientAlphaKey[_Gradient.alphaKeys.Length];
            else if (m_AlphaKeysBuffer.Length != _Gradient.alphaKeys.Length)
                m_AlphaKeysBuffer = new GradientAlphaKey[_Gradient.alphaKeys.Length];

            _Gradient.colorKeys.CopyTo(m_ColorKeysBuffer, 0);
            _Gradient.alphaKeys.CopyTo(m_AlphaKeysBuffer, 0);

            for (int i = 0; i < m_ColorKeysBuffer.Length; i++)
                m_ColorKeysBuffer[i].color *= _Color;

            for (int i = 0; i < m_AlphaKeysBuffer.Length; i++)
                m_AlphaKeysBuffer[i].alpha *= _Color.a;

            _Gradient.SetKeys(m_ColorKeysBuffer, m_AlphaKeysBuffer);
        }

        public static void SetGradientToColor(Gradient _Gradient, Color _Color)
        {
            if (m_ColorKeysBuffer == null)
                m_ColorKeysBuffer = new GradientColorKey[2];
            else if (m_ColorKeysBuffer.Length != 2)
                m_ColorKeysBuffer = new GradientColorKey[2];

            if (m_AlphaKeysBuffer == null)
                m_AlphaKeysBuffer = new GradientAlphaKey[_Gradient.alphaKeys.Length];
            else if (m_AlphaKeysBuffer.Length != _Gradient.alphaKeys.Length)
                m_AlphaKeysBuffer = new GradientAlphaKey[_Gradient.alphaKeys.Length];

            _Gradient.alphaKeys.CopyTo(m_AlphaKeysBuffer, 0);

            for (int i = 0; i < m_AlphaKeysBuffer.Length; i++)
                m_AlphaKeysBuffer[i].alpha *= _Color.a;

            m_ColorKeysBuffer[0] = new GradientColorKey(_Color, 0);
            m_ColorKeysBuffer[1] = new GradientColorKey(_Color, 1);

            _Gradient.SetKeys(m_ColorKeysBuffer, m_AlphaKeysBuffer);
        }

        public static void LerpGradient(Gradient _Target, Gradient _A, Gradient _B, float _T)
        {
            if (_T <= 0f)
            {
                _Target.SetKeys(_A.colorKeys, _A.alphaKeys);
                _Target.mode = _A.mode;
                return;
            }
            else if (_T >= 1f)
            {
                _Target.SetKeys(_B.colorKeys, _B.alphaKeys);
                _Target.mode = _B.mode;
                return;
            }

            m_ColorKeysBuffer = new GradientColorKey[8];
            m_AlphaKeysBuffer = new GradientAlphaKey[8];
            float currentPos = 0;

            for (int i = 0; i < 8; i++)
            {
                currentPos = (float)i / 7;

                m_ColorKeysBuffer[i] = new GradientColorKey(Color.Lerp(_A.Evaluate(currentPos), _B.Evaluate(currentPos), _T), currentPos);
                m_AlphaKeysBuffer[i] = new GradientAlphaKey(Mathf.Lerp(_A.Evaluate(currentPos).a, _B.Evaluate(currentPos).a, _T), currentPos);
            }

            _Target.mode = _T <= 0.5f ? _A.mode : _B.mode;
            _Target.SetKeys(m_ColorKeysBuffer, m_AlphaKeysBuffer);
        }

        public static void LerpGradientAdditive(Gradient _Target, Gradient _A, Gradient _B, float _T, Color _TransitionColor)
        {
            if (_T <= 0f)
            {
                _Target.SetKeys(_A.colorKeys, _A.alphaKeys);
                _Target.mode = _A.mode;
                return;
            }
            else if (_T >= 1f)
            {
                _Target.SetKeys(_B.colorKeys, _B.alphaKeys);
                _Target.mode = _B.mode;
                return;
            }

            if (_T < 0.5f)
            {
                m_ColorKeysBuffer = new GradientColorKey[_A.colorKeys.Length];
                m_AlphaKeysBuffer = new GradientAlphaKey[_A.alphaKeys.Length];

                for (int i = 0; i < m_ColorKeysBuffer.Length; i++)
                    m_ColorKeysBuffer[i] = new GradientColorKey(Color.Lerp(_A.colorKeys[i].color, _TransitionColor, _T / 0.5f), _A.colorKeys[i].time);

                for (int i = 0; i < m_AlphaKeysBuffer.Length; i++)
                    m_AlphaKeysBuffer[i] = new GradientAlphaKey(Mathf.Lerp(_A.alphaKeys[i].alpha, _TransitionColor.a, _T / 0.5f), _A.alphaKeys[i].time);

            }
            else if (_T > 0.5f)
            {
                m_ColorKeysBuffer = new GradientColorKey[_B.colorKeys.Length];
                m_AlphaKeysBuffer = new GradientAlphaKey[_B.alphaKeys.Length];

                for (int i = 0; i < m_ColorKeysBuffer.Length; i++)
                    m_ColorKeysBuffer[i] = new GradientColorKey(Color.Lerp(_TransitionColor, _B.colorKeys[i].color, (_T - 0.5f) / 0.5f), _B.colorKeys[i].time);

                for (int i = 0; i < m_AlphaKeysBuffer.Length; i++)
                    m_AlphaKeysBuffer[i] = new GradientAlphaKey(Mathf.Lerp(_TransitionColor.a, _B.alphaKeys[i].alpha, (_T - 0.5f) / 0.5f), _B.alphaKeys[i].time);
            }
            else
            {
                m_ColorKeysBuffer = new GradientColorKey[2];
                m_AlphaKeysBuffer = new GradientAlphaKey[2];

                m_ColorKeysBuffer[0] = new GradientColorKey(_TransitionColor, 0f);
                m_ColorKeysBuffer[1] = new GradientColorKey(_TransitionColor, 1f);

                m_AlphaKeysBuffer[0] = new GradientAlphaKey(_TransitionColor.a, 0f);
                m_AlphaKeysBuffer[1] = new GradientAlphaKey(_TransitionColor.a, 1f);
            }

            _Target.mode = _T <= 0.5f ? _A.mode : _B.mode;
            _Target.SetKeys(m_ColorKeysBuffer, m_AlphaKeysBuffer);
        }

        public static ParticleSystem.MinMaxGradient SetupMinMaxGradient(ParticleSystem.MinMaxGradient _Target, Gradient _A, Gradient _B)
        {
            switch (_Target.mode)
            {
                case ParticleSystemGradientMode.Gradient:
                case ParticleSystemGradientMode.Color:
                    return _A;

                case ParticleSystemGradientMode.TwoColors:
                case ParticleSystemGradientMode.TwoGradients:
                    return new ParticleSystem.MinMaxGradient(_A, _B);

                default:
                    return _Target;
            }
        }
    }
}