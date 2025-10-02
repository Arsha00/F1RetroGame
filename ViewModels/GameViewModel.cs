/*
 * @Author: Arian Sjöström Shaafi
 * B.Sc Computer Science & Mobile IT
 */

using Formula1Retro.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Formula1Retro
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly InputService _inputService;

        public Car Player { get; }
        public Car Enemy { get; }
        public Track Track { get; }

        // HUD
        private int _playerSpeed;
        public int PlayerSpeed
        {
            get => _playerSpeed;
            private set { _playerSpeed = value; OnPropertyChanged(nameof(PlayerSpeed)); }
        }

        private string _raceStatus = "Waiting...";
        public string RaceStatus
        {
            get => _raceStatus;
            private set { _raceStatus = value; OnPropertyChanged(nameof(RaceStatus)); }
        }

        // Kamera
        public float CameraX { get; private set; }
        public float CameraY { get; private set; }

        // Startljus
        private double _countdownTime;
        public bool RedLightOn { get; private set; }
        public bool YellowLightOn { get; private set; }
        public bool GreenLightOn { get; private set; }

        public bool RaceStarted { get; private set; }
        public bool RaceFinished { get; private set; }

        public GameViewModel(InputService inputService)
        {
            _inputService = inputService;

            // Initiera bilar
            Player = new Car { X = 250, Y = 550, Color = Brushes.Blue, IsAI = false };
            Enemy = new Car { X = 450, Y = 550, Color = Brushes.Red, IsAI = true };
            Enemy.SetTargetLane(450);

            // Bana
            Track = new Track();

            // Startstatus
            _countdownTime = 0;
            RaceStarted = false;
            RaceFinished = false;
            RaceStatus = "Waiting...";
        }

        public void Update(float deltaTime)
        {
            if (RaceFinished)
            {
                // Spelet är slut, låt bilarna stå still
                return;
            }

            if (!RaceStarted)
            {
                HandleCountdown(deltaTime);
                return;
            }

            // Uppdatera bilar
            Player.Update(deltaTime, _inputService);
            Enemy.Update(deltaTime, null);

            // Kamera följer spelaren
            CameraX = Player.X - 400;
            CameraY = Player.Y - 300;

            // HUD
            PlayerSpeed = (int)(Player.CurrentSpeed * 3.6f);
            RaceStatus = Player.IsDrsActive ? "DRS Open" : "DRS Closed";

            // Kontrollera mål
            CheckFinish();
        }

        private void HandleCountdown(float deltaTime)
        {
            _countdownTime += deltaTime;

            if (_countdownTime < 1)
            {
                RedLightOn = true; YellowLightOn = false; GreenLightOn = false;
                RaceStatus = "Ready...";
            }
            else if (_countdownTime < 2)
            {
                RedLightOn = false; YellowLightOn = true; GreenLightOn = false;
                RaceStatus = "Set...";
            }
            else if (_countdownTime < 3)
            {
                RedLightOn = false; YellowLightOn = false; GreenLightOn = true;
                RaceStatus = "Go!";
            }
            else
            {
                RaceStarted = true;
                RedLightOn = YellowLightOn = GreenLightOn = false;
                RaceStatus = "Race!";
            }

            OnPropertyChanged(nameof(RedLightOn));
            OnPropertyChanged(nameof(YellowLightOn));
            OnPropertyChanged(nameof(GreenLightOn));
            OnPropertyChanged(nameof(RaceStatus));
        }

        public string WinnerText { get; private set; } = "";
        private void CheckFinish()
        {
            if (Player.Y <= Track.FinishLineY)
            {
                RaceFinished = true;
                RaceStatus = "You Win!";
                WinnerText = "PLAYER WIN";
            }
            else if (Enemy.Y <= Track.FinishLineY)
            {
                RaceFinished = true;
                RaceStatus = "AI Wins!";
                WinnerText = "AI WINS";
            }

            if (RaceFinished)
            {
                OnPropertyChanged(nameof(RaceStatus));
            }
        }

        public void Render(DrawingContext context)
        {
            context.PushTransform(new TranslateTransform(-CameraX, -CameraY));

            Track.Draw(context);
            Player.Draw(context);
            Enemy.Draw(context);

            context.Pop();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
