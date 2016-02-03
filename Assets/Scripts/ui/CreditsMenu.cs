using UnityEngine;
using System.Collections;

namespace Assets.Scripts.ui
{
    public class CreditsMenu : MonoBehaviour
    {

        public GameObject PopupMenu;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BackButton()
        {
            PopupMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

