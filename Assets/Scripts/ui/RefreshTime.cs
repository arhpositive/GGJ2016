using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.ui
{
    public class RefreshTime : MonoBehaviour
    {
        LevelManager _levelManagerScript;
        Text _timeRemainingText;

        // Use this for initialization
        void Start()
        {
            _levelManagerScript = Camera.main.GetComponent<LevelManager>();
            _timeRemainingText = gameObject.GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            _timeRemainingText.text = "Time : " + _levelManagerScript.GameTimer.ToString("F1");
        }
    }
}


