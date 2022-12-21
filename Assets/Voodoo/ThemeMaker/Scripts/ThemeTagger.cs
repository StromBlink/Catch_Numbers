using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voodoo.LevelDesign
{
    public class ThemeTagger : MonoBehaviour
    {
        [ThemeTagReference]
        public string m_Tag;

        [Header("Objects")]
        public MeshRenderer[] m_Meshes;
        public SkinnedMeshRenderer[] m_SkinnedMeshes;
        public SpriteRenderer[] m_Sprites;

        [Header("Effects")]
        public LineRenderer[] m_Lines;
        public TrailRenderer[] m_Trails;
        public ParticleSystem[] m_Particles;

        [Header("UI")]
        public Image[] m_Images;
        public Text[] m_Texts;
        public TextMeshProUGUI[] m_TextMeshPros;

        [Header("Lighting")]
        public Light[] m_Lights;

        private ThemeTag m_CurrentThemeTag;
        private ThemeTag m_PrevThemeTag;

        private Gradient m_GradientBuffer = new Gradient();
        private Material[] m_MatsBuffer = new Material[0];
        private ParticleSystem.MainModule m_PartMainBuffer;
        private ParticleSystem.ColorBySpeedModule m_PartSpeedBuffer;
        private ParticleSystem.ColorOverLifetimeModule m_PartOverLifetimeBuffer;
        private ParticleSystemRenderer m_PartRenderer;

        private void OnEnable()
        {
            if (ThemesManager.IsValid)
            {
                ThemesManager.Instance.onThemeChanged += OnThemeChanged;
                //Debug.Log("Registering event");
            }
            ApplyThemeTag(ThemesManager.s_CurrentTheme.m_ThemeDict[m_Tag]);
        }

        private void OnDisable()
        {
            if (ThemesManager.IsValid)
            {
                ThemesManager.Instance.onThemeChanged -= OnThemeChanged;
                //Debug.Log("Unregistering event");
            }
        }

        private void OnThemeChanged(bool _Instant)
        {
            if (m_CurrentThemeTag == null || _Instant)
                ApplyThemeTag(ThemesManager.GetTag(m_Tag));
            else
            {
                m_PrevThemeTag = m_CurrentThemeTag;
                m_CurrentThemeTag = ThemesManager.GetTag(m_Tag);
            }

#if UNITY_EDITOR
            if (ThemesManager.Instance.m_ApplyThemeChangesAtRuntime && !_Instant)
                ApplyThemeTag(m_CurrentThemeTag);
#endif
        }

        private void Update()
        {
            if (m_CurrentThemeTag != m_PrevThemeTag)
                ApplyThemeTag(m_CurrentThemeTag);
        }

        public void ChangeTag(string _NewTag)
        {
            m_Tag = _NewTag;
            ApplyThemeTag(ThemesManager.GetTag(m_Tag));
        }

        private void ApplyThemeTag(ThemeTag _NewThemeTag)
        {
            //Debug.Log("Changing to " + _NewTheme.name);
            if (_NewThemeTag == null)
                return;

            m_CurrentThemeTag = _NewThemeTag;
            m_PrevThemeTag = m_CurrentThemeTag;

            //Main
            SetMeshes();
            SetSprites();

            //Effects
            SetLines();
            SetTrails();
            SetParticles();

            //UI
            SetImages();
            SetTexts();
            SetTextMeshPros();

            //Lights
            SetLights();
        }

        private void SetMeshes()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_Meshes == null)
                return;

            for (int i = 0; i < m_Meshes.Length; i++)
            {
                if (!m_Meshes[i])
                    continue;

                if (m_CurrentThemeTag.m_Meshes.m_Application == TagApplications.Basic || m_CurrentThemeTag.m_Meshes.m_Mats == null)
                {
                    m_Meshes[i].material.color = m_CurrentThemeTag.m_MainColor;
                    continue;
                }

                if (m_CurrentThemeTag.m_Meshes.m_Mats.Length == 0)
                {
                    m_Meshes[i].material.color = m_CurrentThemeTag.m_MainColor;
                    continue;
                }

                if (m_MatsBuffer.Length != m_Meshes[i].materials.Length)
                    m_MatsBuffer = new Material[m_Meshes[i].materials.Length];

                for (int j = 0; j < m_MatsBuffer.Length && j < m_CurrentThemeTag.m_Meshes.m_Mats.Length; j++)
                    m_MatsBuffer[j] = m_CurrentThemeTag.m_Meshes.m_Mats[j];

                m_Meshes[i].materials = m_MatsBuffer;
            }
            
            if (m_SkinnedMeshes == null)
                return;

            for (int i = 0; i < m_SkinnedMeshes.Length; i++)
            {
                if (!m_SkinnedMeshes[i])
                    continue;

                if (m_CurrentThemeTag.m_Meshes.m_Application == TagApplications.Basic || m_CurrentThemeTag.m_Meshes.m_Mats == null)
                {
                    m_SkinnedMeshes[i].material.color = m_CurrentThemeTag.m_MainColor;
                    continue;
                }

                if (m_CurrentThemeTag.m_Meshes.m_Mats.Length == 0)
                {
                    m_SkinnedMeshes[i].material.color = m_CurrentThemeTag.m_MainColor;
                    continue;
                }

                if (m_MatsBuffer.Length != m_SkinnedMeshes[i].materials.Length)
                    m_MatsBuffer = new Material[m_SkinnedMeshes[i].materials.Length];

                for (int j = 0; j < m_MatsBuffer.Length && j < m_CurrentThemeTag.m_Meshes.m_Mats.Length; j++)
                    m_MatsBuffer[j] = m_CurrentThemeTag.m_Meshes.m_Mats[j];

                m_SkinnedMeshes[i].materials = m_MatsBuffer;
            }
        }

        private void SetSprites()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_Sprites == null)
                return;

            for (int i = 0; i < m_Sprites.Length; i++)
            {
                if (!m_Sprites[i])
                    continue;

                m_Sprites[i].color = m_CurrentThemeTag.m_MainColor;

                if (m_CurrentThemeTag.m_Sprites.m_Application == TagApplications.Basic || m_CurrentThemeTag.m_Sprites.m_Mat == null)
                    continue;

                m_Sprites[i].material = m_CurrentThemeTag.m_Sprites.m_Mat;
            }
        }

        private void SetLines()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_Lines == null)
                return;

            m_GradientBuffer.SetKeys(m_CurrentThemeTag.m_LineRenderers.m_Gradient.colorKeys, m_CurrentThemeTag.m_LineRenderers.m_Gradient.alphaKeys);

            for (int i = 0; i < m_Lines.Length; i++)
            {
                if (!m_Lines[i])
                    continue;

                if (m_CurrentThemeTag.m_LineRenderers.m_Application == TagApplications.Basic)
                {
                    m_GradientBuffer.SetKeys(m_Lines[i].colorGradient.colorKeys, m_Lines[i].colorGradient.alphaKeys);
                    ThemeUtils.SetGradientToColor(m_GradientBuffer, m_CurrentThemeTag.m_MainColor);
                }
                else
                {
                    if (m_CurrentThemeTag.m_LineRenderers.m_Mat)
                        m_Lines[i].material = m_CurrentThemeTag.m_LineRenderers.m_Mat;
                }

                m_Lines[i].colorGradient = m_GradientBuffer;
            }
        }

        private void SetTrails()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_Trails == null)
                return;

            m_GradientBuffer.SetKeys(m_CurrentThemeTag.m_TrailRenderers.m_Gradient.colorKeys, m_CurrentThemeTag.m_TrailRenderers.m_Gradient.alphaKeys);

            for (int i = 0; i < m_Trails.Length; i++)
            {
                if (!m_Trails[i])
                    continue;

                if (m_CurrentThemeTag.m_TrailRenderers.m_Application == TagApplications.Basic)
                {
                    m_GradientBuffer.SetKeys(m_Trails[i].colorGradient.colorKeys, m_Trails[i].colorGradient.alphaKeys);
                    ThemeUtils.SetGradientToColor(m_GradientBuffer, m_CurrentThemeTag.m_MainColor);
                }
                else
                {
                    if (m_CurrentThemeTag.m_TrailRenderers.m_Mat)
                        m_Trails[i].material = m_CurrentThemeTag.m_TrailRenderers.m_Mat;
                }

                m_Trails[i].colorGradient = m_GradientBuffer;
            }
        }

        private void SetParticles()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_Particles == null)
                return;

            for (int i = 0; i < m_Particles.Length; i++)
            {
                if (!m_Particles[i])
                    continue;

                m_PartMainBuffer = m_Particles[i].main;
                m_PartSpeedBuffer = m_Particles[i].colorBySpeed;
                m_PartOverLifetimeBuffer = m_Particles[i].colorOverLifetime;
                m_PartRenderer = m_Particles[i].GetComponent<ParticleSystemRenderer>();

                if (m_CurrentThemeTag.m_Particles.m_Application == TagApplications.Basic)
                    m_PartMainBuffer.startColor = m_CurrentThemeTag.m_MainColor;
                else
                {
                    m_PartMainBuffer.startColor = ThemeUtils.SetupMinMaxGradient(m_PartMainBuffer.startColor, m_CurrentThemeTag.m_Particles.m_StartColorA, m_CurrentThemeTag.m_Particles.m_StartColorB);

                    if (m_PartSpeedBuffer.enabled && m_CurrentThemeTag.m_Particles.m_AffectColorOverSpeed)
                        m_PartSpeedBuffer.color = ThemeUtils.SetupMinMaxGradient(m_PartSpeedBuffer.color, m_CurrentThemeTag.m_Particles.m_ColorOverSpeedA, m_CurrentThemeTag.m_Particles.m_ColorOverSpeedB);

                    if (m_PartOverLifetimeBuffer.enabled && m_CurrentThemeTag.m_Particles.m_AffectColorOverLifetime)
                        m_PartOverLifetimeBuffer.color = ThemeUtils.SetupMinMaxGradient(m_PartOverLifetimeBuffer.color, m_CurrentThemeTag.m_Particles.m_ColorOverLifetimeA, m_CurrentThemeTag.m_Particles.m_ColorOverLifetimeB);

                    if (m_CurrentThemeTag.m_Particles.m_Mat)
                        m_PartRenderer.material = m_CurrentThemeTag.m_Particles.m_Mat;

                    if (m_CurrentThemeTag.m_Particles.m_TrailsMat)
                        m_PartRenderer.trailMaterial = m_CurrentThemeTag.m_Particles.m_TrailsMat;
                }
            }
        }

        private void SetImages()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_Images == null)
                return;

            for (int i = 0; i < m_Images.Length; i++)
            {
                if (!m_Images[i])
                    continue;

                m_Images[i].color = m_CurrentThemeTag.m_MainColor;
            }
        }

        private void SetTexts()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_Texts == null)
                return;

            for (int i = 0; i < m_Texts.Length; i++)
            {
                if (!m_Texts[i])
                    continue;

                m_Texts[i].color = m_CurrentThemeTag.m_MainColor;
            }
        }

        private void SetTextMeshPros()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_TextMeshPros == null)
                return;

            for (int i = 0; i < m_TextMeshPros.Length; i++)
            {
                if (!m_TextMeshPros[i])
                    continue;

                m_TextMeshPros[i].color = m_CurrentThemeTag.m_MainColor;

                if (m_CurrentThemeTag.m_TextMeshPros.m_Application == TagApplications.Basic || m_CurrentThemeTag.m_TextMeshPros.m_Mat == null)
                    continue;

                m_TextMeshPros[i].fontMaterial = m_CurrentThemeTag.m_TextMeshPros.m_Mat;
            }
        }

        private void SetLights()
        {
            if (m_CurrentThemeTag == null)
                return;

            if (m_Lights == null)
                return;

            for (int i = 0; i < m_Lights.Length; i++)
            {
                if (!m_Lights[i])
                    continue;

                m_Lights[i].color = m_CurrentThemeTag.m_MainColor;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

        }

        private void OnDrawGizmosSelected()
        {

        }

        private void DoGizmos()
        {

        }
#endif
    }
}
