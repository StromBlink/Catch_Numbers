using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Voodoo.LevelDesign
{
    public class ThemesManager : ScriptableObject
    {
        private List<Theme> m_Themes = new List<Theme>();
        public List<string> m_AllTags = new List<string>();

        public static Theme s_CurrentTheme { get; private set; }

        public delegate void OnThemeChanged(bool _InstantRefresh);
        public event OnThemeChanged onThemeChanged;

        private bool m_Initialized;
        private List<Theme> m_RandomCandidates = new List<Theme>();
        private static ThemesManager s_Instance;

#if UNITY_EDITOR
        public bool m_ApplyThemeChangesAtRuntime;
#endif

        private const string c_PlayerPrefSaveAcces = "CreativetestApp.Theme";
        
        private void Initialize()
        {
            //Debug.Log("Initializing Manager");

            if (s_CurrentTheme && m_Initialized)
                return;

            m_Initialized = true;

            m_Themes = new List<Theme>();
            m_Themes.AddRange(Resources.LoadAll<Theme>("GameThemes"));

            if (m_Themes.Count == 0)
                s_CurrentTheme = null;
            else
            {
                for (int i = m_Themes.Count - 1; i >= 0; i--)
                {
                    if (!m_Themes[i])
                        m_Themes.RemoveAt(i);
                }

                if (m_Themes.Count == 0)
                    s_CurrentTheme = null;
                else
                    s_CurrentTheme = m_Themes[0];
            }

            if (s_CurrentTheme)
            {
                for (int i = 0; i < m_Themes.Count; i++)
                    m_Themes[i].Init();

                SetLightingSettings();
            }
        }

        private static void GetInstance()
        {
            if (s_Instance)
            {
                s_Instance.Initialize();
                return;
            }

            s_Instance = Resources.Load<ThemesManager>("ThemesManager");

            if (s_Instance)
                s_Instance.Initialize();
            else
                Debug.LogError("There is no ThemeManager in the project! Create one by clicking on 'Voodoo -> ThemeMaker/Edit Manager'");
        }

        public static ThemesManager Instance
        {
            get
            {
                GetInstance();
                return s_Instance;
            }
        }

        public static bool IsValid
        {
            get
            {
                GetInstance();

                if (!s_Instance)
                    return false;

                return s_CurrentTheme != null;
            }
        }

        public static void ChangeTheme()
        {
            GetInstance();

            if (!s_Instance)
                return;

            s_Instance.m_RandomCandidates.Clear();
            s_Instance.m_RandomCandidates.AddRange(s_Instance.m_Themes);
            if (s_Instance.m_RandomCandidates.Contains(s_CurrentTheme))
                s_Instance.m_RandomCandidates.Remove(s_CurrentTheme);

            if (s_Instance.m_RandomCandidates.Count > 0)
                ChangeTheme(s_Instance.m_RandomCandidates[Random.Range(0, s_Instance.m_RandomCandidates.Count)]);
        }

        public static void RefreshTheme()
        {
            GetInstance();

            if (!s_Instance)
                return;

            if (!s_CurrentTheme)
                return;

            ChangeTheme(s_CurrentTheme, true);
        }

        public static Theme CreateNewTheme()
        {
            GetInstance();

            if (!s_Instance)
                return null;
            
            Theme asset = CreateInstance<Theme>();
            s_Instance.m_Themes.Add(asset);
            asset.Init();
            return asset;
        }
        
        public static void ChangeTheme(Theme _NewTheme, bool _Instant = false)
        {
            ChangeTheme(_NewTheme.name, _Instant);
        }

        public static void ChangeTheme(string _themeName, bool _instant = false)
        {
            GetInstance();

            if (!s_Instance)
                return;

            Theme newSelected = null;
            for (int i = 0 ; i < s_Instance.m_Themes.Count ; ++i)
            {
                if (s_Instance.m_Themes[i].name == _themeName)
                {
                    newSelected = s_Instance.m_Themes[i]; 
                    break;
                }
            }
            if (newSelected == null)
            { return; }
            
            newSelected.Init();

            s_CurrentTheme = newSelected;

            SetLightingSettings();

            if (s_Instance.onThemeChanged != null)
                s_Instance.onThemeChanged.Invoke(_instant);
        }
        
        public static void ChangeTheme(int _themeIndex, bool _instant = false)
        {
            GetInstance();

            if (!s_Instance)
                return;
            if (_themeIndex < 0)
            {
                Debug.LogError($"Trying to get a negative index in Thememanager");
                return;
            }
            
            Theme newSelected = _themeIndex < s_Instance.m_Themes.Count ? s_Instance.m_Themes[_themeIndex] : null;
            if (newSelected == null)
            { return; }
            
            newSelected.Init();

            s_CurrentTheme = newSelected;

            SetLightingSettings();

            if (s_Instance.onThemeChanged != null)
                s_Instance.onThemeChanged.Invoke(_instant);
        }
        
        public static ThemeTag GetTag(string _TagName)
        {
            if (!IsValid)
                return null;

            if (!s_CurrentTheme.m_ThemeDict.ContainsKey(_TagName))
                return null;

            return s_CurrentTheme.m_ThemeDict[_TagName];
        }

        public static Color GetColor(string _TagName)
        {
            if (!IsValid)
                return Color.clear;

            if (!s_CurrentTheme.m_ThemeDict.ContainsKey(_TagName))
                return Color.clear;

            return s_CurrentTheme.m_ThemeDict[_TagName].m_MainColor;
        }

        private static void SetLightingSettings()
        {
            if (s_CurrentTheme.m_Lighting.m_Skybox)
                RenderSettings.skybox = s_CurrentTheme.m_Lighting.m_Skybox;

            RenderSettings.skybox.SetColor("_SkyColor1", s_CurrentTheme.m_Lighting.m_TopColor);
            RenderSettings.skybox.SetColor("_SkyColor2", s_CurrentTheme.m_Lighting.m_HorizonColor);
            RenderSettings.skybox.SetColor("_SkyColor3", s_CurrentTheme.m_Lighting.m_BottomColor);
            
            /*/
            switch (RenderSettings.ambientMode)
            {
                case UnityEngine.Rendering.AmbientMode.Flat:
                    RenderSettings.ambientLight = s_CurrentTheme.m_Lighting.m_AmbientColor;
                    break;

                case UnityEngine.Rendering.AmbientMode.Trilight:
                    RenderSettings.ambientGroundColor = s_CurrentTheme.m_Lighting.m_GroundColor;
                    RenderSettings.ambientEquatorColor = s_CurrentTheme.m_Lighting.m_EquatorColor;
                    RenderSettings.ambientSkyColor = s_CurrentTheme.m_Lighting.m_SkyColor;
                    break;
            }

            RenderSettings.fogColor = s_CurrentTheme.m_Lighting.m_Fog;
            /**/
        }

        public static string[] GetThemesName()
        {
            GetInstance();

            if (!s_Instance)
                return null;

            return s_Instance.m_Themes.Select(_theme => _theme.name).ToArray();
        }

        public static string SaveAllThemes()
        {
            string save = "";
            foreach (Theme theme in s_Instance.m_Themes)
            {
                save += theme.Save();
            }
            save = save.Remove(save.Length - 1);
            return save;
        }

        public static void LoadTheme(string _saveSting)
        {
            string[] save = _saveSting.Split('\n');
            for (int i = 0 ; i < save.Length ; ++i)
            {
                s_Instance.m_Themes[i].Load(save[i]);
            }
        }
    }
}