﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PoolGame;
using PoolGame.Classes.Screens;

namespace PoolGame.Classes
{
    internal class CueBall : PoolBall
    {
        private MouseState previousMouseState;

        public CueBall(Texture2D texture, float radius) : base(texture, radius)
        {
            acceleration = Vector2.Zero;
            position = new Vector2(MainMenu.windowWidth / 5, MainMenu.windowHeight / 2);
        }

        public void Shoot()
        {
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Vector2 movementVector = mousePosition - position;

            velocity += movementVector * VelocityMultiplier;

        }

        public void DoCircleCircleCollision()
        {
            foreach (PoolBall poolBall in Match1.poolBalls)
            {
                if (poolBall == this) // no need to check if it collides with itself
                { continue; }
                else
                {
                    if (Vector2.Distance(position, poolBall.position) <= radius * 2)
                    {
                        // WIP [update to use FindFinalVelocityAfterCircleCircleCollision() and be in PoolBall.cs]

                        if (Vector2.Distance(position, poolBall.position) <= radius * 2)
                        {
                            Vector2 relativePositionVector = position - poolBall.position; // the vector that is normal to the collision surface (aka other ball)
                            Vector2 unitNormalVector = relativePositionVector / relativePositionVector.Length(); // normalised to have a magnitude of 1

                            Vector2 newVelocity = velocity + (Vector2.Dot(poolBall.velocity - velocity, unitNormalVector) * unitNormalVector);
                            Vector2 newOtherVelocity = poolBall.velocity + (Vector2.Dot(velocity - poolBall.velocity, unitNormalVector) * unitNormalVector);

                            poolBall.velocity = newOtherVelocity;
                            velocity = newVelocity;
                        }
                    }
                }
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DoCircleCircleCollision();

            MouseState currentMouseState = Mouse.GetState();

            if ((currentMouseState.LeftButton == ButtonState.Pressed) & (velocity == Vector2.Zero)) // only allowed to input movement when stationary
            {
                Shoot();
            }

            previousMouseState = currentMouseState;
        }
    }
}
