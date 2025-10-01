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

        // HUD-bindings
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

        public GameViewModel(InputService inputService)
        {
            _inputService = inputService;

            // Initiera bilar
            Player = new Car { X = 250, Y = 550, Color = Brushes.Blue, IsAI = false };
            Enemy = new Car { X = 450, Y = 550, Color = Brushes.Red, IsAI = true };
            Enemy.SetTargetLane(450);

            // Bana
            Track = new Track();

            // Startljus
            _countdownTime = 0;
            RaceStarted = false;
            RedLightOn = YellowLightOn = GreenLightOn = false;
        }

        public void Update(float deltaTime)
        {
            if (!RaceStarted)
            {
                HandleCountdown(deltaTime);
                return; // stoppa bilarna innan racet börjar
            }

            // Uppdatera spelare
            Player.Update(deltaTime, _inputService);

            // Uppdatera AI
            Enemy.Update(deltaTime, null);

            // Kamera
            CameraX = Player.X - 400;
            CameraY = Player.Y - 300;

            // HUD
            PlayerSpeed = (int)(Player.CurrentSpeed * 3.6f);
            RaceStatus = Player.IsDrsActive ? "DRS Open" : "DRS Closed";
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
                GreenLightOn = false; // släck lamporna
                RaceStatus = "Race!";
            }

            OnPropertyChanged(nameof(RedLightOn));
            OnPropertyChanged(nameof(YellowLightOn));
            OnPropertyChanged(nameof(GreenLightOn));
            OnPropertyChanged(nameof(RaceStatus));
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
