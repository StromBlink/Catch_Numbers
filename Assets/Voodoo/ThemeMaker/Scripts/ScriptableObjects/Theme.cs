using System.Collections.Generic;
using UnityEngine;

namespace Voodoo.LevelDesign
{
	[CreateAssetMenu(fileName = "New Theme", menuName = "Voodoo/ThemeMaker/Theme")]
	public class Theme : ScriptableObject
	{
		public List<ThemeTag> m_Tags = new List<ThemeTag>();
		public ThemeLighting m_Lighting;

		public Dictionary<string, ThemeTag> m_ThemeDict;

		private ThemesManager m_Manager;

		public void Init()
		{
			//Debug.Log("Initializing theme " + this.name);

			m_Manager = ThemesManager.Instance;

			m_ThemeDict = new Dictionary<string, ThemeTag>(m_Manager.m_AllTags.Count);

			for (int i = 0; i < m_Manager.m_AllTags.Count; i++)
				m_ThemeDict.Add(m_Manager.m_AllTags[i], GetTagByName(m_Manager.m_AllTags[i]));
		}

		private ThemeTag GetTagByName(string _Name)
		{
			for (int i = 0; i < m_Tags.Count; i++)
			{
				if (m_Tags[i].m_TagName == _Name)
					return m_Tags[i];
			}

			return new ThemeTag(_Name);
		}

		public string Save()
		{
			string save = name + "+";

			foreach (ThemeTag themeTag in m_Tags)
			{
				save += themeTag.m_TagName + "-";
				save += "#" + ColorUtility.ToHtmlStringRGBA(themeTag.m_MainColor) + "_";
			}
			save = save.Remove(save.Length - 1);

			save += "+";
			save += "#" + ColorUtility.ToHtmlStringRGBA(m_Lighting.m_TopColor) + "_";
			save += "#" + ColorUtility.ToHtmlStringRGBA(m_Lighting.m_HorizonColor) + "_";
			save += "#" + ColorUtility.ToHtmlStringRGBA(m_Lighting.m_BottomColor);

			save += "\n";
			return save;
		}

		public void Load(string _save)
		{
			string[] colorsSaved = _save.Split('+')[1].Split('_');
			
			for (int i = 0 ; i < colorsSaved.Length ; ++i)
			{
				string[] values = colorsSaved[i].Split('-');
				m_Tags[i].m_TagName = values[0];
				ColorUtility.TryParseHtmlString(values[1], out m_Tags[i].m_MainColor);
			}
			
			colorsSaved = _save.Split('+')[2].Split('_');
			ColorUtility.TryParseHtmlString(colorsSaved[0], out m_Lighting.m_TopColor);
			ColorUtility.TryParseHtmlString(colorsSaved[1], out m_Lighting.m_HorizonColor);
			ColorUtility.TryParseHtmlString(colorsSaved[2], out m_Lighting.m_BottomColor);
		}
	}

	[System.Serializable]
	public class ThemeTag
	{
		public string m_TagName;
		public Color m_MainColor = Color.white;
		public MeshOptions m_Meshes;
		public SpriteOptions m_Sprites;
		public LineRendererOptions m_LineRenderers;
		public LineRendererOptions m_TrailRenderers;
		public ParticleSystemOptions m_Particles;
		public SpriteOptions m_TextMeshPros;

		public ThemeTag(string _Name)
		{
			m_TagName = _Name;
			m_MainColor = Color.white;
			m_Meshes = new MeshOptions();
			m_Sprites = new SpriteOptions();
			m_LineRenderers = new LineRendererOptions();
			m_TrailRenderers = new LineRendererOptions();
			m_Particles = new ParticleSystemOptions();
			m_TextMeshPros = new SpriteOptions();
		}
	}

	[System.Serializable]
	public class ThemeLighting
	{
		public Material m_Skybox;
		public Color m_TopColor;
		public Color m_HorizonColor;
		public Color m_BottomColor;
	}

	public enum TagApplications
	{
		Basic,
		Advanced
	}

	public class TagOptions
	{
		public TagApplications m_Application;
	}

	[System.Serializable]
	public class MeshOptions : TagOptions
	{
		public Material[] m_Mats;

		public MeshOptions()
		{
			m_Mats = new Material[0];
		}
	}

	[System.Serializable]
	public class SpriteOptions : TagOptions
	{
		public Material m_Mat;
	}

	[System.Serializable]
	public class LineRendererOptions : TagOptions
	{
		public Gradient m_Gradient;
		public Material m_Mat;

		public LineRendererOptions()
		{
			m_Gradient = new Gradient();
			m_Mat = null;
		}
	}

	[System.Serializable]
	public class ParticleSystemOptions : TagOptions
	{
		public Gradient m_StartColorA;
		public Gradient m_StartColorB;
		public bool m_AffectColorOverSpeed;
		public Gradient m_ColorOverSpeedA;
		public Gradient m_ColorOverSpeedB;
		public bool m_AffectColorOverLifetime;
		public Gradient m_ColorOverLifetimeA;
		public Gradient m_ColorOverLifetimeB;
		public Material m_Mat;
		public Material m_TrailsMat;

		public ParticleSystemOptions()
		{
			m_StartColorA = new Gradient();
			m_StartColorB = new Gradient();
			m_ColorOverSpeedA = new Gradient();
			m_ColorOverSpeedB = new Gradient();
			m_ColorOverLifetimeA = new Gradient();
			m_ColorOverLifetimeB = new Gradient();
		}
	}
}