using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.ui
{
    public class OverTextScript : MonoBehaviour
    {
        public void WriteTime(float time, bool start)
        {
            Text textObj = gameObject.GetComponent<Text>();

            if (time <= 0.0f && start)
                textObj.text = "GO!";
            else
                textObj.text = (Mathf.CeilToInt(time)).ToString();
        }
    }
}

