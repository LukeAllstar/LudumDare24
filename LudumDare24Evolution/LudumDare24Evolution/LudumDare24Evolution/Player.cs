using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LudumDare24Evolution
{
    class Player
    {
        float movespeed = 1.5f;
        float jumpspeed = 1.5f;
        Vector2 playerPosition = new Vector2(10f, 10f);
        int direction;


        int evolutionLevel = 1;
        int jumpEvolution = 1;
        int defenseEvolution = 1;
        int attackEvolution = 1;
        int speedEvolution = 1;

        Runstate runstate = Runstate.FALLING;

        public void updatePosition(int x, int y)
        {
            playerPosition.X = x;
            playerPosition.Y = y;
        }

        public float getPositionX()
        {
            return playerPosition.X;
        }

        public float getPositionY()
        {
            return playerPosition.Y;
        }

        public void setDirection(int direction)
        {
            this.direction = direction;
        }

        public int getDirection()
        {
            return direction;
        }

        public float getMovespeed()
        {
            return movespeed;
        }

        public void setMovespeed(float movespeed)
        {
            this.movespeed = movespeed;
        }

        public float getJumpspeed()
        {
            return jumpspeed;
        }

        public void setMovespeed(float jumpspeed)
        {
            this.jumpspeed = jumpspeed;
        }

        public void changeRunstate(Runstate state)
        {
            runstate = state;
        }

        public Runstate getRunstate()
        {
            return runstate;
        }

    }
}
