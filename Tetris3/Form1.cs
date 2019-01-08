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
        Point[] nextCoord = new Point[4];


        //create a shape rotation method, working with the shape array


        /*
        /// <summary>
        /// Draws the given tetragram according to its location and position
        /// </summary>
        /// <param name="origin">Coordinates of the square on the grid</param>
        /// <param name="shape">Shape of the tetragram</param>
        /// <param name="position">Orientation of the tetragram 0-3</param>
        /// <param name="color">Color of the square</param>
        public void ShapeDraw(Point origin, char shape, int position)
        {
            //problems
            grid.FillRectangle(drawBrush, origin.X, origin.Y, 20, 20);
        }
        */

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            /*/
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
                    Point tempCoord = new Point(i, j);

                    drawBrush.Color = squareColor[i, j];

                    if (i == 0 || j == 0 || i == 11 || j == 19)
                    {
                        squareColor[i, j] = Color.Black;
                    }
                    else if (nextCoord.Contains(tempCoord) == true)
                    {
                        squareColor[i, j] = Color.Red;
                    }
                    else
                    {
                        squareColor[i, j] = Color.White;
                        drawBrush.Color = squareColor[i, j];
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[i, j].X, squareOrigin[i, j].Y, 20, 20);
                    }


                    if (i == 11 && j == 19)
                    {
                        drawBrush.Color = squareColor[i, j];
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 20, 419);
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 0].X, squareOrigin[0, 0].Y, 251, 20);
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[11, 0].X, squareOrigin[11, 0].Y, 20, 419);
                        e.Graphics.FillRectangle(drawBrush, squareOrigin[0, 19].X, squareOrigin[0, 19].Y, 251, 20);
                    }
                }
            }

            for (int i = 0; i<4; i++)
            {
                drawBrush.Color = squareColor[nextCoord[i].X, nextCoord[i].Y];
                e.Graphics.FillRectangle(drawBrush, squareOrigin[nextCoord[i].X, nextCoord[i].X].X, squareOrigin[nextCoord[i].X, nextCoord[i].X].Y, 20, 20);
            }

            /*
            if (nextCoord.Contains(tempCoord) == true && squareColor[nextCoord[squaresCounter].X, nextCoord[squaresCounter].Y] != Color.Black) //if that square is inside the shapeCoord array change color
            {
                //squareColor[shapeCoord[squaresCounter].X, shapeCoord[squaresCounter].Y] = Color.White;
                //shapeCoord[squaresCounter] = nextCoord[squaresCounter];
                squareColor[i, j] = Color.Red;
                //squaresCounter++;
            }
            squaresCounter = 0;
            */
        }

        //I will have to work on the temporary variables here
        private void movesTimer_Tick(object sender, EventArgs e)
        {
            if (upArrowDown == true)
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
            }

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
            Refresh();
        }

        public Form1()
        {
            InitializeComponent();

            grid = this.CreateGraphics();

            //*temporary solution until I implement the chooseShape and rotate method
            shapeCoord[0] = new Point(5, 1);
            shapeCoord[1] = new Point(6, 1);
            shapeCoord[2] = new Point(5, 2);
            shapeCoord[3] = new Point(4, 2);

            //*/

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
