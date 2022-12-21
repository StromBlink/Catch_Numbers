using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voodoo.Visual.UI
{
    public class Quit : MonoBehaviour
    {
        public GameObject toSetUnactive;

        public void OnClick()
        {
            toSetUnactive.SetActive(false);
        }
    }
}