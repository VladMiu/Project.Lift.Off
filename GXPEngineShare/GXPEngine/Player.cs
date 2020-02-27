﻿using GXPEngine;
using System;
using GXPEngine.Core;

public class Player : Animation
{
    private Mouse _mouseHandler;

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

    public Player() : base("FinalAnimation.png", 8, 10) // monocycle
    {
        _mouseHandler = new Mouse();


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



        AddChild(_mouseHandler);

        hitBox();
    }

    public int GetScore()
    {
        return ScorePlayer;
    }

    public int GetLives()
    {
        return LivesPlayer;
    }

  
    private void hitBox()
    {
        _hitBox = new Sprite("hitboxplayer.png");
        AddChild(_hitBox);
        _hitBox.alpha = 0.5f;
        _hitBox.SetXY(-180, -980);
    }

    private void handleHitBoxCollisions()
    {
        foreach (GameObject other in _hitBox.GetCollisions())
        {

            if (other is Tomatoes)
            {
                Tomatoes tomato = other as Tomatoes;
                tomato.Splash();
                if (_mouseX <= _width/2)
                    _currentBalance -= _tomatoPush;
                else
                    _currentBalance += _tomatoPush;
                
            }
            if (other is Flowers)
            {
                Flowers flowers = other as Flowers;
                flowers.Catched();
                ScorePlayer = ScorePlayer + 200;
            }
            if (other is Collectables)
            {
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
        //  if (_mouseHandler.IsMoving() == true)
        if (_mouseX >= 0 && _mouseX <= 1920)
            _currentBalance += _mouseX.Map(0, _width, -_balanceDifficulty, _balanceDifficulty);

        if (_currentBalance <= -_maxBalance || _currentBalance >= _maxBalance)
        { 
            LivesPlayer--;
            resetPlayer();
        }
        this.rotation = _currentBalance;

        //else
        //set cursor to middle of screen
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
        this.x = _width / 2;
        this.y = _height / 2 - 200;
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

    public void Update()
    {
        handleInput();
        movement();
        wind();
        balanceClown();
        handleHitBoxCollisions();
        animationHandler();
        limitCheck();
    }

    public float GetMouseX()
    {
        return _mouseX;
    }

    public float GetMaxBalance()
    {
        return _maxBalance;
    }

}
