/* 
 * Game: GGJ2016
 * Author: Arhan Bakan
 * 
 * RefreshJetpackCharge.cs
 */

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ui
{
    public class RefreshJetpackCharge : MonoBehaviour
    {
        public string PlayerTag;
        public Image FillImage;
        Slider _jetpackChargeSlider;
        GameObject _playerGameObject;
        BasePlayer _playerScript;

        // Use this for initialization
        void Start()
        {
            _jetpackChargeSlider = gameObject.GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_playerGameObject)
            {
                _playerGameObject = GameObject.FindGameObjectWithTag(PlayerTag);
                if (_playerGameObject)
                {
                    BasePlayer goScript = _playerGameObject.GetComponent<BasePlayer>();
                    FillImage.color = goScript.TeamColor;
                    _playerScript = _playerGameObject.GetComponent<BasePlayer>();

                }               
            }
            else
            {
                _jetpackChargeSlider.value = _playerScript.JetpackValue;
            }
        }
    }
}


