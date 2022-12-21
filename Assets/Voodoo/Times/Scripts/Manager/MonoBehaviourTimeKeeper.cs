using System;
using UnityEngine;

namespace Voodoo.Events
{
	internal sealed class MonoBehaviourTimeKeeper : MonoBehaviour
	{
		public event Action fixedUpdate;

		private void Awake() => DontDestroyOnLoad(this);

		private void FixedUpdate()
		{
			fixedUpdate?.Invoke();
		}

		private void OnDestroy()
		{
			fixedUpdate = null;
		}
	}
}