/*
 * @Author: Arian Sjöström Shaafi
 * B.Sc Computer Science & Mobile IT
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Formula1Retro
{
    public class Track
    {
        private readonly int width;
        private readonly int height;

        public int FinishLineY { get; }

        public Track(int width = 800, int height = 3000) // längre bana
        {
            this.width = width;
            this.height = height;

            // Placera mållinjen högt upp
            FinishLineY = 50;
        }

        public void Draw(DrawingContext dc)
        {
            // Gräs på båda sidor
            dc.DrawRectangle(Brushes.Green, null, new Rect(0, 0, width, height));

            // Vägen (400px bred i mitten)
            int roadWidth = 400;
            int roadX = (width - roadWidth) / 2;
            dc.DrawRectangle(Brushes.DarkSlateGray, null, new Rect(roadX, 0, roadWidth, height));

            // Kantlinjer (vita linjer på sidorna)
            dc.DrawRectangle(Brushes.White, null, new Rect(roadX, 0, 4, height));
            dc.DrawRectangle(Brushes.White, null, new Rect(roadX + roadWidth - 4, 0, 4, height));

            // Mittlinje (prickad)
            for (int y = 0; y < height; y += 40)
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(roadX + roadWidth / 2 - 2, y, 4, 20));
            }

            // Startlinje (nere vid botten)
            int lineHeight = 20;
            int squareSize = 20;
            for (int x = 0; x < roadWidth; x += squareSize)
            {
                Brush brush = (x / squareSize) % 2 == 0 ? Brushes.White : Brushes.Black;
                dc.DrawRectangle(brush, null, new Rect(roadX + x, height - lineHeight, squareSize, lineHeight));
            }

            // Mållinje (uppe vid toppen)
            for (int x = 0; x < roadWidth; x += squareSize)
            {
                Brush brush = (x / squareSize) % 2 == 0 ? Brushes.White : Brushes.Black;
                dc.DrawRectangle(brush, null, new Rect(roadX + x, FinishLineY, squareSize, lineHeight));
            }
        }
    }
}

