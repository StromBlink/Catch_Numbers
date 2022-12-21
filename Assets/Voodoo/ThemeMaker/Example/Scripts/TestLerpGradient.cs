using UnityEngine;

namespace Voodoo.LevelDesign
{
    public class TestLerpGradient : MonoBehaviour
    {
        public TrailRenderer m_Test;
        public TrailRenderer m_OtherTest;
        [Space]
        public Gradient m_A;
        public Gradient m_B;

        [Space]
        public LerpMethods m_Method;
        public Color m_TransitionColor = Color.white;
        [Range(0f, 1f)]
        public float m_Lerp;

        [Space]
        public float m_MovementAmplitude;
        public float m_MovementFreq;

        public enum LerpMethods
        {
            Lerp,
            TransitionColor
        }

        private Gradient m_Gradient = new Gradient();

        // Use this for initialization
        void Start()
        {
            if (m_OtherTest)
                m_OtherTest.colorGradient = m_Gradient;
        }

        // Update is called once per frame
        void Update()
        {

            if (m_Method == LerpMethods.Lerp)
                ThemeUtils.LerpGradient(m_Gradient, m_A, m_B, m_Lerp);
            else
                ThemeUtils.LerpGradientAdditive(m_Gradient, m_A, m_B, m_Lerp, m_TransitionColor);

            if (m_Test)
            {
                m_Test.colorGradient = m_Gradient;
                m_Test.transform.position = Vector3.right * Mathf.Sin(Time.time * m_MovementFreq) * m_MovementAmplitude;
            }

            if (m_OtherTest)
                m_OtherTest.transform.position = Vector3.up * 5 + Vector3.right * Mathf.Sin(Time.time * m_MovementFreq) * m_MovementAmplitude;
        }
    }
}
