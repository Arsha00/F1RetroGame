/*
 * @Author: Arian Sjöström Shaafi
 * B.Sc Computer Science & Mobile IT
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Formula1Retro
{
    public class Car
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Brush Color { get; set; }

        public float CurrentSpeed { get; private set; }
        public bool IsDrsActive { get; private set; }

        private const float MaxSpeed = 330 / 3.6f; // km/h → m/s
        private const float Acceleration = 50f;
        private const float Deceleration = 30f;

        // AI-relaterat
        public bool IsAI { get; set; } = false;
        private float targetLaneX; // positionen AI vill hålla

        public Car()
        {
            // standardfil om inte annat anges
            targetLaneX = 300;
        }

        public void SetTargetLane(float laneX)
        {
            targetLaneX = laneX;
        }

        public void Update(float deltaTime, Services.InputService? input)
        {
            if (IsAI)
            {
                // AI håller sin egen fart
                CurrentSpeed += Acceleration * deltaTime * 0.5f;

                if (CurrentSpeed > MaxSpeed * 0.75f) // AI något långsammare
                    CurrentSpeed = MaxSpeed * 0.75f;

                // Kör framåt
                Y -= CurrentSpeed * deltaTime;

                //AI;n rör sig lite i sidled
                Y += (float)(Math.Sin(Y / 50) * 0.5);


                // Justera X så att bilen håller sig i sin fil
                if (X < targetLaneX)
                {
                    X += 50f * deltaTime; // flytta åt höger
                }
                else if (X > targetLaneX)
                {
                    X -= 50f * deltaTime; // flytta åt vänster
                }
            }
            else
            {
                if (input != null && input.IsKeyPressed(Key.Up))
                {
                    // Accelerera
                    CurrentSpeed += Acceleration * deltaTime;
                    IsDrsActive = true;
                }
                else
                {
                    // Bromsa naturligt
                    CurrentSpeed -= Deceleration * deltaTime;
                    IsDrsActive = false;
                }

                // Begränsa fart
                if (CurrentSpeed > MaxSpeed) CurrentSpeed = MaxSpeed;
                if (CurrentSpeed < 0) CurrentSpeed = 0;

                // Kör framåt
                Y -= CurrentSpeed * deltaTime;

                // Vänster och Höger rörelse för spelare

                if (input.IsKeyPressed(Key.Left))
                    X -= 100 * deltaTime;

                if (input.IsKeyPressed(Key.Right))
                    X += 100 * deltaTime;

            }
        }

        public void Draw(DrawingContext dc)
        {
            dc.DrawRectangle(Color, null, new System.Windows.Rect(X, Y, 40, 60));
        }
    }
}
