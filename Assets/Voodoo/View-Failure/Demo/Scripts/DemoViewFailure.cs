using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voodoo.Pattern;

namespace Voodoo.Meta
{
	public class DemoViewFailure : MonoBehaviour
	{
		public FailureView failureView;

		// Start is called before the first frame update
		void Start()
		{
			failureView.Show();
		}
	}
}