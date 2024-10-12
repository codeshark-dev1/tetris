using System;
using System.Net.Mime;
using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TetrisTemplate
{
    class TetrisBlock
    {
        public bool[,] shape = new bool[4,4];
        public string currentShape = "O";

        public Color color;

        private float rotationDelay = 0.2f;
        private float currentRotationDelay = 0f;

        public TetrisBlock(string blockType)
        {
            currentShape = blockType;
            Initialize();
        }

        public void Initialize()
        {
            switch (currentShape) 
            {
                case "O":
                    shape = new bool[4,4]
                    {
                        {false, false, false, false },
                        {false, true, true, false }, 
                        {false, true, true, false },
                        {false, false, false, false },
                    };
                    color = Color.Yellow;
                    break;
                case "I":
                    shape = new bool[4, 4]
                    {
                        {false, true, false, false },
                        {false, true, false, false },
                        {false, true, false, false },
                        {false, true, false, false },
                    };
                    color = Color.Cyan;
                    break;
                case "S":
                    shape = new bool[4, 4]
                    {
                        {false, true, true, false },
                        {true, true, false, false },
                        {false, false, false, false },
                        {false, false, false, false },
                    };
                    color = Color.Coral;
                    break;
                case "Z":
                    shape = new bool[4, 4]
                    {
                        {true, true, false, false },
                        {false, true, true, false },
                        {false, false, false, false },
                        {false, false, false, false },
                    };
                    color = Color.Green;
                    break;
                case "L":
                    shape = new bool[4, 4]
                    {
                        {true, false, false, false },
                        {true, false, false, false },
                        {true, true, false, false },
                        {false, false, false, false },
                    };
                    color = Color.Orange;
                    break;
                case "J":
                    shape = new bool[4, 4]
                    {
                        {false, false, false, true },
                        {false, false, false, true },
                        {false, false, true, true },
                        {false, false, false, false },
                    };
                    color = Color.Pink;
                    break;
                case "T":
                    shape = new bool[4, 4]
                    {
                        {true, true, true, false },
                        {false, true, false, false },
                        {false, false, false, false },
                        {false, false, false, false },
                    };
                    color = Color.Purple;
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            //TODO: do input using InputHelper
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.R) && currentRotationDelay <= 0)
            {
                Rotate();
            }

            currentRotationDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Rotate()
        {
            if (currentShape == "O")
            {
                return;
            }

            bool[,] newShape = new bool[4, 4];
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    newShape[i, j] = shape[j, i];
                }
            }

            for (int i = 0; i < newShape.GetLength(0); i++)
            {
                for (int j = 0; j < newShape.GetLength(1) / 2; j++)
                {
                    bool tempShape = newShape[i, j];
                    newShape[i, j] = newShape[i, newShape.GetLength(1) - 1 - j];
                    newShape[i, newShape.GetLength(1) - 1 - j] = tempShape;
                }
            }

            shape = newShape;
            currentRotationDelay = rotationDelay;
        }
    }
}