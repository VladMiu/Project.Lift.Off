﻿using GXPEngine;
using System;

public class Collectables : AnimationSprite
{
    private float _speedX = 0.7f;
    private float _speedY = 5f;

    private int numberOffFrames = 7;
    private int startFrame = 0;
    private float frameInterval = 90f;

    private float _animationTimer = 0;

 


    public Collectables(int _startX, int _startY) : base("barry.png", 7, 1)
    {
        this.x = _startX;
        this.y = _startY;

        scale = 1.8f;

        Timer timer = new Timer(5000, LateDestroy);
        AddChild(timer);

        /// Pick up needs to appear at a certain score, and then dissappear after a few seconds;

    }

    private void Update()
    {
        animationCollectable();
    }


    protected void animationCollectable()
    {
        _animationTimer += Time.deltaTime;
        int currentFrame = (int)(_animationTimer / frameInterval) % numberOffFrames + startFrame;
        SetFrame(currentFrame);
    }

    public void destroyTheCollectable()
    {
       LateDestroy();
    }
}