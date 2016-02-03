/* 
 * Game: GGJ2016
 * Author: Arhan Bakan
 * 
 * RuneTile.cs
 * Handles the rune tiles in battle scene.
 */

using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class RuneTile : MonoBehaviour
    {
        SpriteRenderer _leftPlayerRenderer;
        BasePlayer _leftPlayerScript;
        LevelManager _levelManagerScript;
        SpriteRenderer _spriteRenderer;
        public bool OnLeftTeam { get; private set; }
        
        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            OnLeftTeam = transform.position.x <= 14.5 ? true : false;
            _leftPlayerScript = GameObject.FindGameObjectWithTag("LeftPlayer").GetComponent<BasePlayer>();
            _levelManagerScript = Camera.main.GetComponent<LevelManager>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Ball")
            {
                SetSpriteColor(other.gameObject.GetComponent<SpriteRenderer>().color, false);
            }
        }

        public void SetSpriteColor(Color newColor, bool initMode)
        {
            if (_spriteRenderer.color == newColor)
            {
                return;
            }

            if (newColor == _leftPlayerScript.TeamColor)
            {
                OnLeftTeam = true;
            }
            else
            {
                OnLeftTeam = false;
            }
            if (!initMode)
            {
                _levelManagerScript.OnRuneTileSwitch(OnLeftTeam);
            }            
            _spriteRenderer.color = newColor;
        }
    }
}


