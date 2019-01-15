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
        SolidBrush drawBrush = new SolidBrush(Color.White);

        int score = 0;
        int startPos;
        int fallCounter;
        int levelFallFreq = 6;
        Color shapeColor;
        
        //random generator used to get a new shape in NewShape
        Random shapeRandom = new Random();
        
        //character that defines the tetragram. Possible shapes are T, S, Z, I, L, J, O
        char shape;

        //point that to which the shape method referes to when defining the coordinates of the square in the tetragram
        Point shapeFondPoint;

        //I want to prove that I can include the little bump at the beginning! I'll fit the first counterClick block in the key down method, and I'll refresh

        //these booleans make the program run better, I get it :)
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown;

        //this stucture contains the squares' drawing point
        //there are extra rows an columns in order to deal with both te arrays at the same time
        Point[,] squareOrigin = new Point[12, 20];

        //Colors array. White is empty, black is outside border
        Color[,] squareColor = new Color[12, 20];

        //Shape array, contains for cells and is used to check collisions and move the current shape around. Defines tetragram's squares' coordinates
        Point[] pastShapeCoords = new Point[4];

        //I need this, sadly
        Point[] nextShapeCoords = new Point[4];


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            ShapeImplement();
            CollisionCheck();
            ShapeImplement();
            DeleteRows();

            //set the color to the cells belonging to the new shape
            for (int j = 0; j < 4; j++)
            {
                squareColor[nextShapeCoords[j].X, nextShapeCoords[j].Y] = shapeColor;
                pastShapeCoords[j] = nextShapeCoords[j];
            }

            e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 251, 419);

            //drawing process happens here
            for (int i = 1; i < 12; i++)
            {
                for (int j = 1; j < 20; j++)
                {
                    drawBrush.Color = squareColor[i, j];        //brush's color is set to the color of the cell you are drawing
                    e.Graphics.FillRectangle(drawBrush, squareOrigin[i, j].X, squareOrigin[i, j].Y, 20, 20);
                }
            }

            /*
            //black boudaries drawn only once
            e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 20, 419);
            e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 251, 20);
            e.Graphics.FillRectangle(drawBrush, squareOrigin[11, 0].X, squareOrigin[11, 0].Y, 20, 419);
            e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 19].X, squareOrigin[0, 19].Y, 251, 20);
            */

            scoreLabel.Text = "" + score;
        }

        //I will have to work on the temporary variables here
        private void movesTimer_Tick(object sender, EventArgs e)
        {


            //if up arrow is pressed AND piece isn't at the boudaries' sides AND shape different to I, you are allowed to change position
            //if shape IS I it MUSTN'T be at x=8. Otherwise don't change position
            if (upArrowDown == true && shapeFondPoint.X != 1 && shapeFondPoint.X != 10 && (shape != 'I' || shapeFondPoint.X != 9))
            {
                startPos = (startPos + 1) % 4;
                leftArrowDown = false;
                rightArrowDown = false;
                downArrowDown = false;
            }

            fallCounter++;

            if (fallCounter == levelFallFreq)
            {
                shapeFondPoint.Y++;
                fallCounter = 0;
            }
            else if (true)
            {
                if (leftArrowDown == true)
                {
                    shapeFondPoint.X--;
                }
                if (rightArrowDown == true)
                {
                    shapeFondPoint.X++;
                }
                if (downArrowDown == true)
                {
                    shapeFondPoint.Y++;
                }
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
                    else if (pastShapeCoords.Contains(tempCoord) == true)
                    {
                        squareColor[i, j] = shapeColor;
                    }
                    else
                    {
                        squareColor[i, j] = Color.White;
                    }
                    squareOrigin[i, j] = new Point(30 + i * 21, 30 + j * 21);
                }
            }

            shape = NewShape('p');
            ShapeImplement();

            /*
            squareColor[5, 7] = Color.Blue;
            squareColor[4, 7] = Color.Blue;
            squareColor[4, 8] = Color.Blue;
            squareColor[4, 9] = Color.Blue;
            //*/

            movesTimer.Enabled = true;
        }

        public void CollisionCheck()
        {
            bool collisionValue = false;

            //last shape is erased
            for (int j = 0; j < 4; j++)
            {
                squareColor[pastShapeCoords[j].X, pastShapeCoords[j].Y] = Color.White;
            }

            //position of the shape and collision check
            for (int i = 0; i < 4; i++)
            {
                Point tempCoord = nextShapeCoords[i];
 
                if (tempCoord.X < 1 || tempCoord.X > 10)
                {
                    shapeFondPoint = pastShapeCoords[0];
                }

                else if (squareColor[tempCoord.X, tempCoord.Y] != Color.White)
                {
                    if (tempCoord.Y > pastShapeCoords[i].Y)
                    {
                        collisionValue = true;                   
                    }
                    else
                    {
                        shapeFondPoint = pastShapeCoords[0];
                    }
                }
            }

            if (collisionValue == true)
            {
                for (int j = 0; j < 4; j++)
                {
                    squareColor[pastShapeCoords[j].X, pastShapeCoords[j].Y] = shapeColor;
                }
                shape = NewShape(shape);
                collisionValue = false;
            }

        }

        public void DeleteRows()
        {
            bool rowCompleted;

            for (int i = 1; i < 19; i++)
            {
                rowCompleted = true;

                for (int j = 1; j < 11; j++)
                {
                    if (squareColor[j, i] == Color.White)
                    {
                        rowCompleted = false; 
                    }
                }

                if (rowCompleted == true)
                {
                    for (int j = 1; j < 11; j++)
                    {
                        squareColor[j, i] = Color.White;
                    }

                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 1; j < 11; j++)
                        {
                            if (k != 1)
                            {
                                squareColor[j, k] = squareColor[j, k - 1];
                            }
                            else
                            {
                                squareColor[j, k] = Color.White;
                            }
                        }
                    }

                    score++;
                }
            }
        }

        public char NewShape(char lastShape)
        {
            shapeFondPoint = new Point(5, 1);
            startPos = 0;
            fallCounter = 0;

            //replicating the original algorithm, according to Chad Birch's post https://gaming.stackexchange.com/questions/13057/tetris-difficulty#13129
            int shapeNumber = shapeRandom.Next(0, 8);
            switch (shapeNumber)
            {
                case 1:
                    if (lastShape == 'T')
                    {
                        goto default;
                    }
                    return 'T';
                case 2:
                    if (lastShape == 'S')
                    {
                        goto default;
                    }
                    return 'S';
                case 3:
                    if (lastShape == 'Z')
                    {
                        goto default;
                    }
                    return 'Z';
                case 4:
                    if (lastShape == 'I')
                    {
                        goto default;
                    }
                    return 'I';
                case 5:
                    if (lastShape == 'L')
                    {
                        goto default;
                    }
                    return 'L';
                case 6:
                    if (lastShape == 'J')
                    {
                        goto default;
                    }
                    return 'J';
                case 7:
                    if (lastShape == 'O')
                    {
                        goto default;
                    }
                    return 'O';
                default:
                    shapeNumber = shapeRandom.Next(1, 8);
                    switch(shapeNumber)
                    {
                        case 1:
                            return 'T';
                        case 2:
                            return 'S';
                        case 3:
                            return 'Z';
                        case 4:
                            return 'I';
                        case 5:
                            return 'L';
                        case 6:
                            return 'J';
                        default:
                            return 'O';
                    }
            }
        }

        public void ShapeImplement ()
        {
            switch (shape)
            {
                case 'T':
                    switch (startPos)
                    {
                        case 0:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            break;
                        case 1:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            break;
                        case 2:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            break;
                        case 3:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            break;
                    }
                    shapeColor = Color.FromArgb(255, 255, 215, 0);
                    break;
                case 'S':
                    switch (startPos)
                    {
                        case 0:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y + 1);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            break;
                        case 1:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y + 1);
                            break;
                        case 2:
                            goto case 0;
                        case 3:
                            goto case 1;
                    }
                    shapeColor = Color.LightSkyBlue;
                    break;
                case 'Z':
                    switch (startPos)
                    {
                        case 0:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y + 1);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            break;
                        case 1:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y - 1);
                            break;
                        case 2:
                            goto case 0;
                        case 3:
                            goto case 1;
                    }
                    shapeColor = Color.Green;
                    break;
                case 'I':
                    switch (startPos)
                    {
                        case 0:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X + 2, shapeFondPoint.Y);
                            break;
                        case 1:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 2);
                            break;
                        case 2:
                            goto case 0;
                        case 3:
                            goto case 1;
                    }
                    shapeColor = Color.FromArgb(255, 255, 140, 0);
                    break;
                case 'L':
                    switch (startPos)
                    {
                        case 0:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y + 1);
                            break;
                        case 1:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y - 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            break;
                        case 2:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y - 1);
                            break;
                        case 3:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y + 1);
                            break;
                    }
                    shapeColor = Color.FromArgb(255, 0, 0, 205);
                    break;
                case 'J':
                    switch (startPos)
                    {
                        case 0:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y + 1);
                            break;
                        case 1:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y + 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            break;
                        case 2:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X - 1, shapeFondPoint.Y - 1);
                            break;
                        case 3:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X, shapeFondPoint.Y - 1);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y - 1);
                            break;
                    }
                    shapeColor = Color.Purple;
                    break;
                case 'O':
                    switch (startPos)
                    {
                        case 0:
                            nextShapeCoords[0] = new Point(shapeFondPoint.X, shapeFondPoint.Y);
                            nextShapeCoords[1] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y);
                            nextShapeCoords[2] = new Point(shapeFondPoint.X, shapeFondPoint.Y + 1);
                            nextShapeCoords[3] = new Point(shapeFondPoint.X + 1, shapeFondPoint.Y + 1);
                            break;
                        case 1:
                            goto case 0;
                        case 2:
                            goto case 0;
                        case 3:
                            goto case 0;
                    }
                    shapeColor = Color.Red;
                    break;
            }
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
