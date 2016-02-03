/* 
 * Game: GGJ2016
 * Author: Arhan Bakan
 * 
 * LevelManager.cs
 * Handles the level in battle scene.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.ui;
using Random = UnityEngine.Random;
using System.Collections;

namespace Assets.Scripts
{
    public class Platform
    {
        public GameObject PlatformObject;
        public float PlatformTimer;
        public bool IsVisible;
        public bool LeftSide;

        public Platform(GameObject platformObj, float platformTimer, bool leftSide)
        {
            PlatformObject = platformObj;
            PlatformTimer = platformTimer;
            IsVisible = false;
            LeftSide = leftSide;
        }
    }

    public class LevelManager : MonoBehaviour
    {
        public float GameTime;
        public float PlatformTimerMin;
        public float PlatformTimerMax;
        public int NumPlatforms;
        public GameObject WallPrefab;
        public GameObject ColumnPrefab;
        public GameObject Column2Prefab;
        public GameObject BorderPrefab;
        public GameObject LeftPrefab;
        public GameObject RightPrefab;
        public GameObject LeftBallPrefab;
        public GameObject RightBallPrefab;
        public GameObject PlatformPrefab;
        public Sprite[] RuneSpritesArray;
        public GameObject runeBasisPrefab;
        

        public GameObject _popupMenu;
        public GameObject _overText;

        GameObject _leftPlayer;
        GameObject _rightPlayer;
        GameObject _leftBall;
        GameObject _rightBall;
        bool _isStartingGame;
        const float startTimer = 3.0f;
        float _timeAtStart;

        List<Platform> _platforms;

        List<RuneTile> _runeTilesList;
        public int LeftTeamRuneCount { get; private set; }
        public int TotalRuneCount { get; private set; }
        public float RunePercentage { get; private set; }

        public float GameTimer { get; private set; }

        void Start()
        {
            Time.timeScale = 0.0f;
            _isStartingGame = true;
            _overText.SetActive(true);
            _timeAtStart = Time.realtimeSinceStartup;

            LeftTeamRuneCount = 0;
            TotalRuneCount = 0;

            GameTimer = GameTime;
            _runeTilesList = new List<RuneTile>();
            _platforms = new List<Platform>();
            GenerateInitialLevel();
            SpawnBalls();
            CreateRunes();
            InitPlatforms();
        }

        private void GenerateInitialLevel()
        {
            //TODO tiling all by magic numbers is certainly a bad idea and will cause headaches if camera size changes

            //tile all walls
            for (float fx = -0.5f; fx < 30.0f; fx += 1.0f)
            {
                Instantiate(Column2Prefab, new Vector2(fx, 1.75f), Quaternion.identity);
                Instantiate(Column2Prefab, new Vector2(fx, 19.25f), Quaternion.identity);
            }

            for (float fy = 2.5f; fy < 19.0f; fy += 1.0f)
            {
                Instantiate(ColumnPrefab, new Vector2(-0.25f, fy), Quaternion.identity);
                Instantiate(ColumnPrefab, new Vector2(29.25f, fy), Quaternion.identity);
                Instantiate(BorderPrefab, new Vector2(14.5f, fy), Quaternion.identity);
            }

            _leftPlayer = Instantiate(LeftPrefab, new Vector2(0.765f, 2.765f), Quaternion.identity) as GameObject;
            _rightPlayer = Instantiate(RightPrefab, new Vector2(28.235f, 2.765f), Quaternion.Euler(0,180,0)) as GameObject;
        }

        private void SpawnBalls()
        {
            float randomX = Random.Range(3.0f, 6.0f);
            float randomY = Random.Range(8.0f, 13.0f);
            float randomYdiversify = Random.Range(-0.1f, 0.1f);

            _leftBall = Instantiate(LeftBallPrefab, new Vector2(randomX, randomY + randomYdiversify), Quaternion.identity) as GameObject;
            _rightBall = Instantiate(RightBallPrefab, new Vector2(29.0f - randomX, randomY - randomYdiversify), Quaternion.identity) as GameObject;
        }

        private void CreateRunes()
        {
            //TODO do not go over the line :)

            //create 512x512 grid of 8x8 rune tiles out of files (64 x 64 runes)
            //
            

            int runeSpriteIndex = Random.Range(0, RuneSpritesArray.Length);
            Sprite runeSprite = RuneSpritesArray[runeSpriteIndex];
            
            Color[] pix = runeSprite.texture.GetPixels((int)runeSprite.textureRect.x,
                                                    (int)runeSprite.textureRect.y,
                                                    (int)runeSprite.textureRect.width,
                                                    (int)runeSprite.textureRect.height);

            float initial_x = 6.625f;
            float initial_y = 2.625f;

            for (int y = 0; y < 64; ++y)
            {
                for (int x = 0; x < 64; ++x)
                {
                    if (pix[x + 64 * y] == Color.black)
                    {
                        bool initForLeftTeam = (x < 32) ? true : false;

                        GameObject go = 
                            Instantiate(runeBasisPrefab, 
                            new Vector2(initial_x + x * 0.25f, initial_y + y * 0.25f), Quaternion.identity) as GameObject;
                        go.GetComponent<RuneTile>().SetSpriteColor(initForLeftTeam ? _leftPlayer.GetComponent<BasePlayer>().TeamColor
                            : _rightPlayer.GetComponent<BasePlayer>().TeamColor, true);
                        _runeTilesList.Add(go.GetComponent<RuneTile>());

                        if (initForLeftTeam)
                        {
                            ++LeftTeamRuneCount;
                        }
                        ++TotalRuneCount;
                    }
                }
            }
        }

        void Update()
        {
            float timeUntilStart = startTimer - (Time.realtimeSinceStartup - _timeAtStart);
            if (Time.timeScale == 0.0f)
            {
                if (_isStartingGame)
                {
                    

                    OverTextScript scriptText = _overText.GetComponent<OverTextScript>();
                    scriptText.WriteTime(timeUntilStart, true);

                    if (timeUntilStart <= 0.0f)
                    {
                        StartCoroutine(LateCall());
                        _isStartingGame = false;
                        Time.timeScale = 1.0f;
                    }
                }
                return;
            }

            UpdatePlatforms();

            GameTimer -= Time.deltaTime;

            if (GameTimer <= 3.0f)
            {
                _overText.SetActive(true);
                _overText.GetComponent<OverTextScript>().WriteTime(GameTimer, false);

                if (GameTimer <= 0.0f)
                {
                    _overText.SetActive(false);
                    GameEnded(RunePercentage > 0.5f, true);
                }
            }
            
        }

        private IEnumerator LateCall()
        {
            yield return new WaitForSeconds(1.0f);

            _overText.SetActive(false);
        }

        private void InitPlatforms()
        {
            for (int i = 0; i < NumPlatforms; ++i)
            {
                GameObject go = Instantiate(PlatformPrefab, Vector2.zero, Quaternion.identity) as GameObject;
                go.SetActive(false);
                float spawnTimer = Random.Range(PlatformTimerMin, PlatformTimerMax);
                _platforms.Add(new Platform(go, spawnTimer, ((i % 2) == 0)));
            }
        }

        private void UpdatePlatforms()
        {
            foreach (Platform p in _platforms)
            {
                p.PlatformTimer -= Time.deltaTime;

                if (p.PlatformTimer < 0.0f)
                {
                    float timer = Random.Range(PlatformTimerMin, PlatformTimerMax);
                    if (p.IsVisible)
                    {
                        //disable platform
                        p.PlatformObject.SetActive(false);
                        p.IsVisible = false;
                    }
                    else
                    {
                        //enable platform
                        SpawnPlatform(p);
                        timer *= 2;
                    }
                    p.PlatformTimer = timer;
                }
            }
        }

        private void SpawnPlatform(Platform p)
        {
            //set new platform position

            //make sure player is not around
            Vector2 platformPos = Vector2.zero;
            bool notAccepted = true;
            while (notAccepted)
            {
                float randX = Random.Range(1.5f, 5.5f);
                if (p.LeftSide)
                {
                    randX = 29.0f - randX;
                }

                platformPos = new Vector2(randX, Random.Range(5.5f, 15.5f));

                if ((p.LeftSide && 
                    Vector2.Distance(_leftPlayer.transform.position, platformPos) > 3.0f &&
                    Vector2.Distance(_leftBall.transform.position, platformPos) > 3.0f &&
                    Vector2.Distance(_rightBall.transform.position, platformPos) > 3.0f) ||
                    (!p.LeftSide && 
                    Vector2.Distance(_rightPlayer.transform.position, platformPos) > 3.0f &&
                    Vector2.Distance(_leftBall.transform.position, platformPos) > 3.0f &&
                    Vector2.Distance(_rightBall.transform.position, platformPos) > 3.0f))
                {
                    notAccepted = false;
                }
            }
            p.IsVisible = true;
            p.PlatformObject.transform.position = platformPos;

            p.PlatformObject.SetActive(true);
        }

        public void OnRuneTileSwitch(bool tileSwitchesToLeft)
        {
            if (tileSwitchesToLeft)
            {
                ++LeftTeamRuneCount;
            }
            else
            {
                --LeftTeamRuneCount;
            }

            if (LeftTeamRuneCount == 0)
            {
                GameEnded(false, false);
            }
            else if (LeftTeamRuneCount == TotalRuneCount)
            {
                GameEnded(true, false);
            }

            RunePercentage = (float)LeftTeamRuneCount / TotalRuneCount;
        }

        private void GameEnded(bool leftSideWon, bool isTimedVictory)
        {
            if (leftSideWon)
            {
                //LEFT WINS
                _popupMenu.GetComponent<GamePopupMenu>().FinalizeGame(false, true,
                    isTimedVictory, _leftPlayer.GetComponent<BasePlayer>().TeamColor);
            }
            else
            {
                if (RunePercentage == 0.5f)
                {
                    //TIE
                    _popupMenu.GetComponent<GamePopupMenu>().FinalizeGame(true, false, false, Color.white);
                }
                else
                {
                    //RIGHT WINS
                    _popupMenu.GetComponent<GamePopupMenu>().FinalizeGame(false, false, 
                        isTimedVictory, _rightPlayer.GetComponent<BasePlayer>().TeamColor);
                }
            }
        }
    }
}
