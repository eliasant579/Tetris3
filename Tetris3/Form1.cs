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
        int startPos = 0;
        int fallCounter = 0;
        int levelFallFreq = 10;
        
        //I want to prove that I can include the little bump at the biginning! I'll fit the first counterClick block in the key down method, and I'll refresh
        
        //this booleans make the program run better, I get it :)
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown; //whats the difference with bool ?

        //this stucture contains the squares' drawing point
        //there are extra rows an columns in order to deal with both te arrays at the same time
        Point[,] squareOrigin = new Point[12, 20];

        //Colors array. White is empty, black is outside border
        Color[,] squareColor = new Color[12, 20];

        //Shape array, contains for cells and is used to check collisions and move the current shape around. Defines tetragram's squares' coordinates
        Point[] shapeCoord = new Point[4];

        //I need this, sadly
        Point[] nextCoord = new Point[4];


        //create a shape rotation method, working with the shape array

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            bool collisionCheck = false;

            for (int i = 0; i < 4; i++)
            {
                Point tempCoord = new Point(nextCoord[i].X, nextCoord[i].Y);

                if (squareColor[tempCoord.X, tempCoord.Y] != Color.White && shapeCoord.Contains(tempCoord) == false) { collisionCheck = true; }

                if (i == 3)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        tempCoord = nextCoord[j];
                        if (collisionCheck == false)
                        {
                            squareColor[shapeCoord[j].X, shapeCoord[j].Y] = Color.White;                          
                        }
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        tempCoord = nextCoord[j];
                        if (collisionCheck == false)
                        {
                            squareColor[tempCoord.X, tempCoord.Y] = Color.Red;
                            shapeCoord[j] = tempCoord;
                        }
                    }
                    collisionCheck = false;
                }
            }
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    drawBrush.Color = squareColor[i, j];
                    e.Graphics.FillRectangle(drawBrush, squareOrigin[i, j].X, squareOrigin[i, j].Y, 20, 20);

                    if (i == 11 && j == 19)
                    {
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 20, 419);
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 251, 20);
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[11, 0].X, squareOrigin[11, 0].Y, 20, 419);
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 19].X, squareOrigin[0, 19].Y, 251, 20);
                    }
                }
            }
        }

        //I will have to work on the temporary variables here
        private void movesTimer_Tick(object sender, EventArgs e)
        {
            /*
            if (upArrowDown == true)
            {
                //startPos = (startPos + 1) % 4;
                nextCoord[i].Y = shapeCoord[i].Y + 1;
            }
            else*/
            {
                for (int i = 0; i < 4; i++)
                {
                    nextCoord[i].X = shapeCoord[i].X;
                    nextCoord[i].Y = shapeCoord[i].Y;

                    if (leftArrowDown == true)
                    {
                        nextCoord[i].X = shapeCoord[i].X - 1;
                    }
                    else if (rightArrowDown == true)
                    {
                        nextCoord[i].X = shapeCoord[i].X + 1;
                    }
                    else if (downArrowDown == true)
                    {
                        nextCoord[i].Y = shapeCoord[i].Y + 1;
                    }
                    /*
                    else if (upArrowDown == true)
                    {
                        nextCoord[i].Y = shapeCoord[i].Y - 1;
                    }
                    //*/
                }
            }
            //*
            fallCounter++;

            if (fallCounter == levelFallFreq)
            {
                for (int i = 0; i < 4; i++)
                {
                    nextCoord[i].X = shapeCoord[i].X;
                    nextCoord[i].Y = shapeCoord[i].Y + 1;
                }
                fallCounter = 0;
            }
            //*/
            Refresh();
        }

        public Form1()
        {
            InitializeComponent();

            grid = this.CreateGraphics();

            //*temporary solution until I implement the chooseShape and rotate method
            shapeCoord[0] = new Point(5, 1);
            shapeCoord[1] = new Point(6, 1);
            shapeCoord[2] = new Point(4, 1);
            shapeCoord[3] = new Point(5, 2);

            //*/

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Point tempCoord = new Point(i, j);
                    if (i == 0 || i == 11 || j == 0 || j == 19)
                    {
                        squareColor[i, j] = Color.Black;
                    }
                    else if (shapeCoord.Contains(tempCoord) == true)
                    {
                        squareColor[i, j] = Color.Red;
                    }
                    else
                    {
                        squareColor[i, j] = Color.White;
                    }
                    squareOrigin[i, j] = new Point(30 + i * 21, 30 + j * 21);
                }
            }
            movesTimer.Enabled = true;
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

            //Refresh();    //works!
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
