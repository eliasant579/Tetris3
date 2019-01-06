﻿using System;
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
        Point startPoint = new Point(5, 1);
        
        //this booleans make the program run better, I get it :)
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown; //whats the difference with bool ?

        //this stucture contains the squares' drawing point
        //there are extra rows an columns in order to deal with both te arrays at the same time
        Point[,] squareOrigin = new Point[12, 20];

        //Are the squares empty? There are two extra rows and columns
        bool[,] squareEmpty = new bool[12, 20];

        //I'm implementing the colors last, I don't really care right now


            /// <summary>
            /// Draws the given tetragram according to its location and position
            /// </summary>
            /// <param name="origin"></param>
            /// <param name="shape"></param>
            /// <param name="position"></param>
        public void ShapeDraw(Point origin, char shape, int position)
        {
            grid.FillRectangle(drawBrush, squareOrigin[origin.X, origin.Y].X, squareOrigin[origin.X, origin.Y].Y, 21, 21);
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
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (i != 0 || i != 11 || j != 0 || j != 19)
                    {
                        squareOrigin[i, j] = new Point(30 + i * 21, 30 + j * 21);
                        squareEmpty[i, j] = true;
                    }
                }
            }
            ShapeDraw(startPoint, 't', 0);
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
