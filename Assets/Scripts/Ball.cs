/* 
 * Game: GGJ2016
 * Author: Arhan Bakan
 * 
 * Ball.cs
 * Handles the ball in battle scene.
 */

using UnityEngine;

namespace Assets.Scripts
{
    public class Ball : MonoBehaviour
    {
        public float MaxSpeed;
        Rigidbody2D _rb2D;
        public bool OnLeftTeam;
        public bool PushOnce;

        SpriteRenderer _leftPlayerRenderer;
        SpriteRenderer _rightPlayerRenderer;
        BasePlayer _leftPlayerScript;
        BasePlayer _rightPlayerScript;
        SpriteRenderer _ballRenderer;
        GameObject _leftPlayer;
        GameObject _rightPlayer;
        bool _ballColorChangePending;
        Color _pendingColor;
        bool _initiallyHit;

        float _lastDirectionChangeTime;
        const float _directionChangeThreshold = 5.0f;
        Vector2 _prevDirection;

        // Use this for initialization
        void Start()
        {
            _initiallyHit = false;
            _rb2D = GetComponent<Rigidbody2D>();
            _ballRenderer = GetComponent<SpriteRenderer>();
            _ballColorChangePending = false;

            _leftPlayer = GameObject.FindGameObjectWithTag("LeftPlayer");
            _leftPlayerScript = _leftPlayer.GetComponent<BasePlayer>();
            _leftPlayerRenderer = _leftPlayer.GetComponent<SpriteRenderer>();
            _rightPlayer = GameObject.FindGameObjectWithTag("RightPlayer");
            _rightPlayerScript = _rightPlayer.GetComponent<BasePlayer>();
            _rightPlayerRenderer = _rightPlayer.GetComponent<SpriteRenderer>();

            _ballRenderer.color = OnLeftTeam ? _leftPlayerScript.TeamColor : _rightPlayerScript.TeamColor;

            _pendingColor = _ballRenderer.color;

            _lastDirectionChangeTime = Time.time;
            _prevDirection = Vector2.zero;
        }

        void FixedUpdate()
        {
            //if (PushOnce)
            //{
            //    Vector2 randomDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            //    randomDir.Normalize();
            //    _rb2D.AddForce(randomDir * Time.fixedDeltaTime * 100.0f, ForceMode2D.Impulse);
            //    pushOnce = false;
            //}

            Vector2 normVel = _rb2D.velocity.normalized;
            if ((Mathf.Abs(normVel.x) != 1.0f && Mathf.Abs(normVel.y) != 1.0f))
            {
                if (normVel.magnitude == 0.0f && _initiallyHit)
                {
                    Vector2 newDir = new Vector2(14.5f - transform.position.x, 10.5f - transform.position.y);
                    newDir.Normalize();
                    _rb2D.AddForce(newDir * Time.fixedDeltaTime * 50.0f, ForceMode2D.Impulse);
                }
                _lastDirectionChangeTime = Time.time;
            }

            //cap the velocity magnitude
            _rb2D.velocity = Vector2.ClampMagnitude(_rb2D.velocity, MaxSpeed);
        }

        // Update is called once per frame
        void Update()
        {
            if (_ballColorChangePending)
            {
                _ballRenderer.color = _pendingColor;
                OnLeftTeam = (_ballRenderer.color == _leftPlayerScript.TeamColor) ? true : false;
                _ballColorChangePending = false;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            _initiallyHit = true;
            if (collision.gameObject.tag == "LeftPlayer" || collision.gameObject.tag == "LeftShield")
            {
                _ballRenderer.color = _leftPlayerScript.TeamColor;
            }
            else if (collision.gameObject.tag == "RightPlayer" || collision.gameObject.tag == "RightShield")
            {
                _ballRenderer.color = _rightPlayerScript.TeamColor;
            }
            else if (collision.gameObject.tag == "Ball")
            {
                _ballColorChangePending = true;
                _pendingColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            }

            Vector2 normVel = _rb2D.velocity.normalized;
            if ((Mathf.Abs(normVel.x) == 1.0f || Mathf.Abs(normVel.y) == 1.0f))
            {
                if (Time.time - _lastDirectionChangeTime > _directionChangeThreshold)
                {
                    Vector2 newDir = new Vector2(14.5f - transform.position.x, 10.5f - transform.position.y);
                    newDir.Normalize();
                    _rb2D.AddForce(newDir * Time.fixedDeltaTime * 50.0f, ForceMode2D.Impulse);
                    _lastDirectionChangeTime = Time.time;
                }
            }
        }
    }
}

