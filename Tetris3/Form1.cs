using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris3
{
    public partial class Form1 : Form
    {
        Graphics grid;
        Pen gridPen = new Pen(Color.Black);
        SolidBrush drawBrush = new SolidBrush(Color.Red);
        Point startLocation = new Point(5, 1);
        int startPos = 0;
        int fallCounter;
        int levelFallFreq = 10;
        
        //this booleans make the program run better, I get it :)
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown; //whats the difference with bool ?

        //this stucture contains the squares' drawing point
        //there are extra rows an columns in order to deal with both te arrays at the same time
        Point[,] squareOrigin = new Point[12, 20];

        //Colors array. White is empty, black is outside border
        Color[,] squareColor = new Color[12, 20];

        /// <summary>
        /// Draws the given tetragram according to its location and position
        /// </summary>
        /// <param name="origin">Coordinates of the square on the grid</param>
        /// <param name="shape">Shape of the tetragram</param>
        /// <param name="position">Orientation of the tetragram 0-3</param>
        /// <param name="color">Color of the square</param>
        public void ShapeDraw(Point origin, char shape, int position, string color)
        {
            squareColor[origin.X, origin.Y] = Color.Red;
            grid.FillRectangle(drawBrush, origin.X, origin.Y, 21, 21);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //*/
            for (int i = 50; i <= 260; i += 21)
            {
                e.Graphics.DrawLine(gridPen, i, 50, i, 428);
            }

            for (int i = 50; i <= 428; i += 21)
            {
                e.Graphics.DrawLine(gridPen, 50, i, 260, i);
            }
            //*/

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (i == 0 || i == 11 || j == 0 || j == 19)
                    {
                        drawBrush.Color = squareColor[i, j];
                    }
                    else
                    {
                        squareOrigin[i, j] = new Point(30 + i * 21, 30 + j * 21);
                        drawBrush.Color = squareColor[i, j];
                    }
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (i == 0 || i == 11 || j == 0 || j == 19)
                    {
                        squareColor[i, j] = Color.Black;
                    }
                    else
                    {
                        squareOrigin[i, j] = new Point(30 + i * 21, 30 + j * 21);
                        squareColor[i, j] = Color.White;
                    }
                }
            }
            movesTimer.Enabled = true;
            ShapeDraw(squareOrigin[startLocation.X, startLocation.Y], 't', 0, "red");
        }

        private void movesTimer_Tick(object sender, EventArgs e)
        {
            if (upArrowDown == true)
            {
                 startPos = ( startPos + 1) % 4;
            }
            else if (leftArrowDown == true)
            {
                startLocation.X--;
            }
            else if (rightArrowDown == true)
            {
                startLocation.X++;
            }
            else if (downArrowDown == true)
            {
                startLocation.Y++;
            }

            fallCounter++;

            if (fallCounter == levelFallFreq)
            {
                startLocation.Y++;
                fallCounter = 0;
            }
            Refresh();
            ShapeDraw(squareOrigin[startLocation.X, startLocation.Y], 'T', startPos, "");
        }

        public Form1()
        {
            InitializeComponent();

            grid = this.CreateGraphics();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                downArrowDown = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                leftArrowDown = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                rightArrowDown = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                upArrowDown = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                downArrowDown = false;
            }
            else if (e.KeyCode == Keys.Left)
            {
                leftArrowDown = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                rightArrowDown = false;
            }
            else if (e.KeyCode == Keys.Up)
            {
                upArrowDown = false;
            }
        }
    }
}
