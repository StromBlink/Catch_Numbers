using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voodoo.Meta;

namespace Voodoo.Pattern
{
	public class PercentView_Launcher : MonoBehaviour
	{
		void Awake()
		{
			PercentBarView.Instance.Show();
			PercentCircleView.Instance.Show();
		}
	}
}
