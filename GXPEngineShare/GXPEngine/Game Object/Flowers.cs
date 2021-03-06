﻿using GXPEngine;
using System;

public class Flowers : AnimationSprite
{
    private float _speedX;
    private float _speedY;

    private int numberOffFrames;


    public Flowers(int _startX, int _startY) : base("flower.png", 3, 2)
    {
        this.x = _startX;
        this.y = _startY;

        _speedX = 0.7f;
        _speedY = 5f;

        numberOffFrames = 6;

        scale = 1.8f;

    }

    private void Update()
    {
        movingFlower();
        stoppingFlower();
    }

    private void movingFlower()
    {
        if (this.x > 750)
        {
            x = x - _speedX;
            y = y - _speedY;
        }
        if (this.x <= 750)
        {
            x = x + _speedX;
            y = y - _speedY;
        }

    }

    private void stoppingFlower()
    {
        if (this.y < -100)
        {
            LateDestroy();
        }
    }

    protected void animationFlowers()
    {
        int frame = (int)(this.x / 30) % numberOffFrames;
        SetFrame(frame);
    }

    public void Catched()
    {
        LateDestroy();
    }
}