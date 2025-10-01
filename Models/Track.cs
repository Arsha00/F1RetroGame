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

        public Track(int width = 800, int height = 600) // default storlek
        {
            this.width = width;
            this.height = height;
        }

        public void Draw(DrawingContext dc)
        {
            // Gräs på båda sidor
            dc.DrawRectangle(Brushes.Green, null, new System.Windows.Rect(0, 0, width, height));

            // Vägen (400px bred i mitten)
            int roadWidth = 400;
            int roadX = (width - roadWidth) / 2;
            dc.DrawRectangle(Brushes.DarkSlateGray, null, new System.Windows.Rect(roadX, 0, roadWidth, height));

            // Kantlinjer (vita linjer på sidorna)
            dc.DrawRectangle(Brushes.White, null, new System.Windows.Rect(roadX, 0, 4, height));
            dc.DrawRectangle(Brushes.White, null, new System.Windows.Rect(roadX + roadWidth - 4, 0, 4, height));

            // Mittlinje (prickad)
            for (int y = 0; y < height; y += 40)
            {
                dc.DrawRectangle(Brushes.White, null, new System.Windows.Rect(roadX + roadWidth / 2 - 2, y, 4, 20));
            }

            // Start- och mållinje (svartvit rutig längst ned)
            int lineHeight = 20;
            int squareSize = 20;
            for (int x = 0; x < roadWidth; x += squareSize)
            {
                Brush brush = (x / squareSize) % 2 == 0 ? Brushes.White : Brushes.Black;
                dc.DrawRectangle(brush, null, new System.Windows.Rect(roadX + x, height - lineHeight, squareSize, lineHeight));
            }
        }
    }
}
