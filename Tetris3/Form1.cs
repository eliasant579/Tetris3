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
        Point startCoords = new Point(5, 1);
        bool collisionCheck = false;
        char shape = 'S';
        
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
            collisionCheck = false;

            //position of the shape and collision check
            for (int i = 0; i < 4; i++)
            {
                Point tempCoord = nextCoord[i];
                //*
                //if at least one of the squares that should be occupied next are not white AND they don't belong to the existing shape, there is a COLLISION
                if (tempCoord.Y > 18 || squareColor[tempCoord.X, tempCoord.Y] != Color.White && shapeCoord.Contains(tempCoord) == false)
                {
                    collisionCheck = true;
                    startCoords.X = shapeCoord[0].X;
                }
                //*/
                if (i == 3 && collisionCheck == false)
                {
                    //clear the squares occupied by the old shape
                    for (int j = 0; j < 4; j++)
                    {
                        squareColor[shapeCoord[j].X, shapeCoord[j].Y] = Color.White;
                    }

                    //set the color to the cells belonging to the new shape
                    for (int j = 0; j < 4; j++)
                    {
                        tempCoord = nextCoord[j];
                        squareColor[tempCoord.X, tempCoord.Y] = Color.Red;
                        shapeCoord[j] = tempCoord;
                    }
                }
            }

            //drawing process happens here
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    drawBrush.Color = squareColor[i, j];        //brush's color is set to the color of the cell you are drawing
                    e.Graphics.FillRectangle(drawBrush, squareOrigin[i, j].X, squareOrigin[i, j].Y, 20, 20);
                }
            }

            //black boudaries drawn only once
            e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 20, 419);
            e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 251, 20);
            e.Graphics.FillRectangle(drawBrush, squareOrigin[11, 0].X, squareOrigin[11, 0].Y, 20, 419);
            e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 19].X, squareOrigin[0, 19].Y, 251, 20);

            //*
            squareColor[4, 2] = Color.Blue;
            squareColor[4, 3] = Color.Blue;
            //*/

        }

        //I will have to work on the temporary variables here
        private void movesTimer_Tick(object sender, EventArgs e)
        {
            //if up arrow is pressed AND piece isn't at the boudaries' sides AND shape different to I, you are allowed to change position
            //if shape IS I it MUSTN'T be at x=8. Otherwise don't change position
            if (upArrowDown == true && startCoords.X != 1 && startCoords.X != 10 && (shape != 'I' || startCoords.X != 9))
            {
                startPos = (startPos + 1) % 4;
            }
            else
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
                }

                if (leftArrowDown == true)
                {
                    startCoords.X--;
                }
                else if (rightArrowDown == true)
                {
                    startCoords.X++;
                }
                else if (downArrowDown == true)
                {
                    startCoords.Y++;
                }
            }

            fallCounter++;
            if (fallCounter == levelFallFreq)
            {
                for (int i = 0; i < 4; i++)
                {
                    nextCoord[i].X = shapeCoord[i].X;
                    nextCoord[i].Y = shapeCoord[i].Y + 1;
                }
                startCoords.Y++;
                fallCounter = 0;
            }

            switch (shape)
            {
                case 'T':
                    switch (startPos)
                    {
                        case 0:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y + 1);
                            break;
                        case 1:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X, startCoords.Y - 1);
                            nextCoord[2] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y + 1);
                            break;
                        case 2:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y - 1);
                            break;
                        case 3:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X, startCoords.Y - 1);
                            nextCoord[2] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y + 1);
                            break;
                    }
                    break;
                case 'S':
                    switch (startPos)
                    {
                        case 0:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X - 1, startCoords.Y + 1);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y + 1);
                            break;
                        case 1:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X, startCoords.Y - 1);
                            nextCoord[2] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X + 1, startCoords.Y + 1);
                            break;
                        case 2:
                            goto case 0;
                        case 3:
                            goto case 1;
                    }
                    break;
                case 'Z':
                    switch (startPos)
                    {
                        case 0:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X + 1, startCoords.Y + 1);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y + 1);
                            break;
                        case 1:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X, startCoords.Y + 1);
                            nextCoord[2] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X + 1, startCoords.Y - 1);
                            break;
                        case 2:
                            goto case 0;
                        case 3:
                            goto case 1;
                    }
                    break;
                case 'I':
                    switch (startPos)
                    {
                        case 0:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X + 2, startCoords.Y);
                            break;
                        case 1:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X, startCoords.Y - 1);
                            nextCoord[2] = new Point(startCoords.X, startCoords.Y + 1);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y + 2);
                            break;
                        case 2:
                            goto case 0;
                        case 3:
                            goto case 1;
                    }
                    break;
                case 'L':
                    switch (startPos)
                    {
                        case 0:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X - 1, startCoords.Y + 1);
                            break;
                        case 1:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X - 1, startCoords.Y - 1);
                            nextCoord[2] = new Point(startCoords.X, startCoords.Y - 1);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y + 1);
                            break;
                        case 2:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X + 1, startCoords.Y - 1);
                            break;
                        case 3:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X, startCoords.Y - 1);
                            nextCoord[2] = new Point(startCoords.X, startCoords.Y + 1);
                            nextCoord[3] = new Point(startCoords.X + 1, startCoords.Y + 1);
                            break;
                    }
                    break;
                case 'J':
                    switch (startPos)
                    {
                        case 0:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X + 1, startCoords.Y + 1);
                            break;
                        case 1:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X - 1, startCoords.Y + 1);
                            nextCoord[2] = new Point(startCoords.X, startCoords.Y - 1);
                            nextCoord[3] = new Point(startCoords.X, startCoords.Y + 1);
                            break;
                        case 2:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X - 1, startCoords.Y);
                            nextCoord[3] = new Point(startCoords.X - 1, startCoords.Y - 1);
                            break;
                        case 3:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X, startCoords.Y - 1);
                            nextCoord[2] = new Point(startCoords.X, startCoords.Y + 1);
                            nextCoord[3] = new Point(startCoords.X + 1, startCoords.Y - 1);
                            break;
                    }
                    break;
                case 'O':
                    switch (startPos)
                    {
                        case 0:
                            nextCoord[0] = new Point(startCoords.X, startCoords.Y);
                            nextCoord[1] = new Point(startCoords.X + 1, startCoords.Y);
                            nextCoord[2] = new Point(startCoords.X, startCoords.Y + 1);
                            nextCoord[3] = new Point(startCoords.X + 1, startCoords.Y + 1);
                            break;
                        case 1:
                            goto case 0;
                        case 2:
                            goto case 0;
                        case 3:
                            goto case 0;
                    }
                    break;
            }             
            Refresh();
        }

        public Form1()
        {
            InitializeComponent();

            grid = this.CreateGraphics();

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
