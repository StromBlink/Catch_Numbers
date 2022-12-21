using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Voodoo.Visual.UI.Button
{
    public class IconRV : MonoBehaviour
	{
		public Image image;

		[Space]
		public Sprite rvSprite;
		public Sprite loadingSprite;

		[Space]
		public Color rvColor;
		public Color loadingColor;
		
        [Space]
        public float rotateSpeed = 75f;

		private bool isLoading;

        /// <summary>
        /// Update the display depending on the state of the Rewarded Video
        /// </summary>
        /// <param name="isRVLoaded"></param>
        public void UpdateDisplay(bool isRVLoaded)
		{
            if (isRVLoaded)
            {
                image.sprite = rvSprite;
                image.color = rvColor;

                isLoading = false;
                transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                image.sprite = loadingSprite;
				image.color = loadingColor;

                if (isLoading == false)
                {
                    LoadingFx();
                }
            }
        }

        async void LoadingFx()
        {
            isLoading = true;
            while (isLoading)
            {
                await Task.Delay(1);
                if (image != null && image.transform != null)
                {
	                image.transform.Rotate(Vector3.back, Time.fixedDeltaTime * rotateSpeed);
                }
            }
        }

    }
}