/* 
 * Game: GGJ2016
 * Author: Arhan Bakan
 * 
 * BasePlayer.cs
 * Handles the player shooting the ball in scene.
 */

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BasePlayer : MonoBehaviour
    {
        const float ForceCoef = 200.0f;
        const float RotateCoef = 180.0f;
        const float JetpackSpendCoef = 0.4f;
        const float JetpackGainCoef = 1.0f;
        public Color TeamColor;
        public string HorizontalAxisName;
        public string VerticalAxisName;
        public KeyCode ShieldButton;
        public bool IsFlipped;

        public float JetpackValue { get; private set; }

        Rigidbody2D _rb2D;
        bool _shieldMode;
        GameObject _playerShield;
        GameObject _nozzleBottom;
        GameObject _nozzleBack;
         
        
        void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _nozzleBack = transform.FindChild("nozzleBack").gameObject;
            _nozzleBottom = transform.FindChild("nozzleBottom").gameObject;
            _shieldMode = false;
            JetpackValue = 1.0f;

            //find shield object in children
            foreach (Transform tr in transform)
            {
                if (tr.tag == "LeftShield" || tr.tag == "RightShield")
                {
                    _playerShield = tr.gameObject;
                    break;
                }
            }
        }

        void FixedUpdate()
        {
            if (!_shieldMode && Time.timeScale != 0.0f)
            {
                //recover jetpack charge if not propelling

                Vector2 movementDir = GetMoveDirFromInput();

                if (movementDir.y <= 0.0f)
                {
                    JetpackValue = Mathf.Clamp01(JetpackValue + Time.deltaTime * JetpackGainCoef);
                }

                _rb2D.AddForce(movementDir * Time.fixedDeltaTime * ForceCoef, ForceMode2D.Impulse);
            }            
        }

        void Update()
        {
            if (Time.timeScale == 0.0f) return;

            //TODO need to perform a transition to signify changes visually on the player character
            if (Input.GetKeyDown(ShieldButton))
            {
                ToggleShieldMode();
            }

            //TODO NEXT now we need to rotate shield around our character if shield mode is enabled

            if (_shieldMode)
            {
                float shieldMoveDir = Input.GetAxisRaw(HorizontalAxisName);

                if (IsFlipped)
                {
                    shieldMoveDir *= -1.0f;
                }

                _playerShield.transform.Rotate(Vector3.forward * -shieldMoveDir * Time.deltaTime * RotateCoef);
            }
        }

        private void ToggleShieldMode()
        {
            //TODO problems will appear if we allow shield to be toggled inside boundaries
            // alternatively, we may put collision flag on shield to allow going through walls

            _playerShield.transform.rotation = transform.rotation;

            _shieldMode = !_shieldMode;
            _rb2D.isKinematic = _shieldMode;

            _playerShield.SetActive(_shieldMode);
            _nozzleBottom.SetActive(_shieldMode);
        }

        Vector2 GetMoveDirFromInput()
        {
            float horizontalMoveDir = Input.GetAxisRaw(HorizontalAxisName);
            float verticalMoveDir = 0.0f;

            //if you have jetpack charge, spend it if you're going upwards
            if (JetpackValue > 0.0f)
            {
                verticalMoveDir = Mathf.Max(Input.GetAxisRaw(VerticalAxisName), 0.0f);
            }

            Vector2 moveDir = new Vector2(horizontalMoveDir, verticalMoveDir);
            moveDir.Normalize();
            if (moveDir.y > 0.0f)
            {
                _nozzleBottom.SetActive(true);
            }
            else
            {
                _nozzleBottom.SetActive(false);
            }
            if (moveDir.x < 0.0f)
            {
                if (!IsFlipped)
                {
                    transform.Rotate(0, -180, 0);
                    IsFlipped = true;
                }
                _nozzleBack.SetActive(true);
            }
            else if (moveDir.x > 0.0f)
            {
                if (IsFlipped)
                {
                    transform.Rotate(0, 180, 0);
                    IsFlipped = false;
                }
                _nozzleBack.SetActive(true);
            }
            else
            {
                _nozzleBack.SetActive(false);
            }
            JetpackValue = Mathf.Clamp01(JetpackValue - moveDir.y * Time.deltaTime * JetpackSpendCoef);

            return moveDir;
        }
    }
}

