using UnityEngine;

namespace Voodoo.Meta
{
    public class Demo_VirtualKeyboard : MonoBehaviour
    {
        void Start()
        {
            VirtualKeyboardView.Instance.Show();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                VirtualKeyboardView.Instance.Show();
            else if (Input.GetKeyDown(KeyCode.H))
                VirtualKeyboardView.Instance.Hide();
        }
    }
}
