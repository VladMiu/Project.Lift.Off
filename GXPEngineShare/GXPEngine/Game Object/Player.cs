﻿using GXPEngine;
using System;
using GXPEngine.Core;

public class Player : Animation
{
    private Mouse _mouseHandler;

    private Sound _cheerHitSound;
    private Sound _hitTomatoSound;
    private Sound _hitTomatoSound2;
    private Sound _collectFlower;
    private Sound _hitBalloon;

    Random random;

    private int _randomHitSound;

    private bool _isMoving;

    private float _currentSpeed, _maxSpeed;
    private float _moveSpeed;
    private float _friction;
    
    private float _currentBalance, _maxBalance;
    private float _balanceSpeed;
    private float _balanceDifficulty;

    private float _windSpeed;

    private float _width = 1920, _height = 1080;

    private float _mouseX;

    public int ScorePlayer, HighScorePlayer;
    public int LivesPlayer;

    private int _tomatoPush;

    private float _limit;
    private Sprite _hitBox;

    public Player() : base("FinalAnimation.png", 8, 8) // monocycle
    {
        _mouseHandler = new Mouse();

        _cheerHitSound = new Sound("cheering.wav");
        _hitTomatoSound = new Sound("splat1.wav");
        _hitTomatoSound2 = new Sound("splat2.wav");
        _collectFlower = new Sound("pickupflower.wav");
        _hitBalloon = new Sound("hitBalloon.wav");

        random = new Random();

        valueSetup();

        AddChild(_mouseHandler);

        hitBox();
    }

    public void Update()
    {
        handleInput();
        movement();
        wind();
        balanceClown();
        handleHitBoxCollisions();
        animationHandler();
        limitCheck();
        Console.WriteLine(_mouseX);
    }

    public float GetMouseX()
    {
        return _mouseX;
    }

    public float GetMaxBalance()
    {
        return _maxBalance;
    }

    public int GetScore()
    {
        return ScorePlayer;
    }

    public int GetLives()
    {
        return LivesPlayer;
    }

    private void valueSetup()
    {
        _tomatoPush = 15;
        _isMoving = false;

        _currentSpeed = 0f;
        _moveSpeed = 0.5f;
        _maxSpeed = 15f;
        _friction = 0.2f;

        _limit = 200f;

        _currentBalance = 0f;
        _maxBalance = 60f;
        _balanceSpeed = 0.5f; // (movement)
        _balanceDifficulty = 0.5f;  // Bigger == faster (mouse)

        _windSpeed = 0.5f; // wind pushes in the direction you are tilting

        scale = 0.3f;

        ScorePlayer = 0;
        LivesPlayer = 2;
    }

    private void randomSound()
    {
        _randomHitSound = random.Next(2);
        if (_randomHitSound == 0 )
        {
            _hitTomatoSound.Play();
        }
        else
        {
            _hitTomatoSound2.Play();
        }
        Console.WriteLine(random.Next(2));
    }
  
    private void hitBox()
    {
        _hitBox = new Sprite("hitboxplayer.png");
        AddChild(_hitBox);
        _hitBox.alpha = 0.0f;
        _hitBox.SetXY(-180, -980);
    }

    private void handleHitBoxCollisions()
    {
        foreach (GameObject other in _hitBox.GetCollisions())
        {

            if (other is Tomatoes)
            {
                randomSound();

                Tomatoes tomato = other as Tomatoes;
                tomato.Splash();
                if (_mouseX <= _width/2)
                    _currentBalance -= _tomatoPush;
                else
                    _currentBalance += _tomatoPush;
                
            }
            if (other is Flowers)
            {
                _collectFlower.Play();
                Flowers flowers = other as Flowers;
                flowers.Catched();
                ScorePlayer = ScorePlayer + 200;
            }
            if (other is Collectables)
            {
                _hitBalloon.Play();
                Collectables collectables = other as Collectables;
                collectables.destroyTheCollectable();
                ScorePlayer = ScorePlayer + 600;
            }

        }
    }

    private void animationHandler()
    {
        if (Input.GetKey(Key.D) | (Input.GetKey(Key.A)))
        {
            walkingAnimation(20);
        }
        else 
        {
            idleAnimation(19, 20);
        }
        if (Math.Abs(_currentBalance) > 35 && Math.Abs(_currentBalance) < 60)
        {
            almostFallingAnimation(39, 20);

        }
    }

    private void handleInput()
    {
        _mouseX = Input.mouseX;

        if ((Input.GetKey('d') || Input.GetKey('D')) && (Input.GetKey('a') || Input.GetKey('A')))
        {
            if (this.x <= _width - _limit)
            {
                if (_currentSpeed < _maxSpeed - _currentSpeed)
                {
                    _currentSpeed += _moveSpeed;
                    if (_currentBalance > -_maxBalance)
                    {
                        _currentBalance -= _balanceSpeed;
                    }
                }
            }
        }
        else if (Input.GetKey('d') || Input.GetKey('D'))
        {
            if (this.x <= _width - _limit)
            {
                if (_currentSpeed < _maxSpeed - _currentSpeed)
                {
                    _currentSpeed += _moveSpeed;
                    if (_currentBalance > -_maxBalance)
                    {
                        _currentBalance -= _balanceSpeed;
                    }
                }
            }
            
        }
        else if (Input.GetKey('a') || Input.GetKey('A'))
        {
            if (this.x >= _limit)
            {
                if (_currentSpeed > -_maxSpeed - _currentSpeed)
                {
                    _currentSpeed -= _moveSpeed;
                    if (_currentBalance < _maxBalance)
                    {
                        _currentBalance += _balanceSpeed;
                    }
                }
            }
        }
    }

    private void balanceClown()
    {
        if (_mouseX >= 0 && _mouseX <= 1920)
            _currentBalance += _mouseX.Map(0, _width, -_balanceDifficulty, _balanceDifficulty);

        if (_currentBalance <= -_maxBalance || _currentBalance >= _maxBalance)
        { 
            LivesPlayer--;
            resetPlayer();
        }
        this.rotation = _currentBalance;
    }

    private void movement()
    {
        if (_currentSpeed > 0 && _currentSpeed <= _friction || _currentSpeed < 0 && _currentSpeed >= _friction)
            _isMoving = false;
        else
            _isMoving = true;

        if (_currentSpeed > 0)
            _currentSpeed -= _friction;
        else if (_currentSpeed < 0)
            _currentSpeed += _friction;

        this.x += _currentSpeed;
        
        
    }

    private void limitCheck()
    {
        if (this.x <= _limit)
        {
            this.x = _limit + 0.01f;
            _currentSpeed = 0f;
        }
        else if(this.x >= _width - _limit)
        {
            this.x = _width - _limit - 0.01f;
            _currentSpeed = 0f;
        }
    }

    private void resetPlayer()
    {
        _cheerHitSound.Play();
        this.x = _width / 2;
        this.y = _height / 2 - 113; //87
        _currentBalance = 0;

    }

    private void wind()
    {
        if (_isMoving != false)
        {
            if (_mouseX < _width / 2)
            {
                _currentBalance -= _balanceSpeed * _windSpeed;
            }
            else
            {
                _currentBalance += _balanceSpeed * _windSpeed;
            }
        }
    }

}
