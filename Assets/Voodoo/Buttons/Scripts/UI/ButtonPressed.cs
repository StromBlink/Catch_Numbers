using UnityEngine;

namespace Voodoo.Visual.UI.Button
{
    public class ButtonPressed : MonoBehaviour
    {
        public bool animated = true;

        // LERP
        public float timeOfTravel = 1; //time after object reach a target place
        public float delayBetweenLoop;
        private float currentTime = 0; // actual floting time 
        private float normalizedValue;

        public RectTransform mainRectTransform;
        private RectTransform rectTransform;

        private Vector3 startPosition;
        private Vector3 endPosition;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            //mainRectTransform = transform.root.GetComponent<RectTransform>();

            Vector3 _mainPosition = mainRectTransform.transform.localPosition;

            Debug.Log(mainRectTransform.rect.width);

            startPosition = new Vector3(
                _mainPosition.x - (mainRectTransform.rect.width / 2 + rectTransform.rect.width / 2),
                _mainPosition.y,
                _mainPosition.z
            );

            transform.localPosition = startPosition;

            endPosition = new Vector3(
                _mainPosition.x + (mainRectTransform.rect.width / 2 + rectTransform.rect.width / 2),
                _mainPosition.y,
                _mainPosition.z
            );
        }

        public void Animated(bool _isAnimated)
        {
            animated = _isAnimated;
            transform.localPosition = startPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if (animated == false)
            {
                return;
            }

            if (currentTime <= timeOfTravel)
            {
                currentTime += Time.deltaTime;
                normalizedValue = currentTime / timeOfTravel;
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, normalizedValue);
            }
            else
            {
                currentTime = 0f;
                transform.localPosition = startPosition;
            }
        }
    }
}