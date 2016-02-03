using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.ui
{
    public class RefreshScore : MonoBehaviour
    {
        public Image FillImage;
        public Image RightImage;
        Slider _scoreSlider;
        GameObject _leftPlayerGameObject;
        GameObject _rightPlayerGameObject;
        LevelManager _levelManagerScript;

        // Use this for initialization
        void Start()
        {
            _levelManagerScript = Camera.main.GetComponent<LevelManager>();
            _scoreSlider = gameObject.GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_leftPlayerGameObject)
            {
                _leftPlayerGameObject = GameObject.FindGameObjectWithTag("LeftPlayer");
                if (_leftPlayerGameObject)
                {
                    BasePlayer goScript = _leftPlayerGameObject.GetComponent<BasePlayer>();
                    FillImage.color = goScript.TeamColor;
                }
            }

            if (!_rightPlayerGameObject)
            {
                _rightPlayerGameObject = GameObject.FindGameObjectWithTag("RightPlayer");
                if (_rightPlayerGameObject)
                {
                    BasePlayer goScript = _rightPlayerGameObject.GetComponent<BasePlayer>();
                    RightImage.color = goScript.TeamColor;
                }
            }

            _scoreSlider.value = _levelManagerScript.RunePercentage;
        }
    }
}

