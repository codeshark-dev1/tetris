using System;
using System.Net.Mime;
using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    class TetrisBlock
    {
        public bool[,] shape = new bool[4, 4];
        public string currentShape = "O";

        private int[] shapeOffsets = { 2, 0, 1, 1, 2, 2, 2 };
        public int currentShapeOffset;
        private int currentRotation = 0;

        #region shapes

        private bool[][,] OShape = new bool[1][,]
        {
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {true, true, false, false },
                {true, true, false, false }
            }
        };

        private bool[][,] IShape = new bool[2][,]
        {
            new bool[4, 4]
            {
                {true, false, false, false },
                {true, false, false, false },
                {true, false, false, false },
                {true, false, false, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {false, false, false, false},
                {true, true, true, true }
            }
        };

        private bool[][,] JShape = new bool[4][,]
        {
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, true, false, false },
                {false, true , false, false },
                {true, true, false, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {true, false , false, false },
                {true, true, true, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {true, true, false, false },
                {true, false , false, false },
                {true, false, false, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {true, true , true, false },
                {false, false, true, false }
            }
        };

        private bool[][,] LShape = new bool[4][,]
        {
            new bool[4, 4]
            {
                {false, false, false, false },
                {true, true, false, false },
                {false, true , false, false },
                {false, true, false, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {false, false , true, false },
                {true, true, true, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {true, false, false, false },
                {true, false , false, false },
                {true, true, false, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {true, true , true, false },
                {true, false, false, false }
            }
        };

        private bool[][,] SShape = new bool[2][,]
        {
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {false, true , true, false },
                {true, true, false, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {true, false, false, false },
                {true, true , false, false },
                {false, true, false, false }
            }
        };

        private bool[][,] ZShape = new bool[2][,]
        {
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {true, true , false, false },
                {false, true, true, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, true, false, false },
                {true, true , false, false },
                {true, false, false, false }
            }
        };

        private bool[][,] TShape = new bool[4][,]
        {
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {true, true , true, false },
                {false, true, false, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, true, false, false },
                {true, true , false, false },
                {false, true, false, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, false, false, false },
                {false, true , false, false },
                {true, true, true, false }
            },
            new bool[4, 4]
            {
                {false, false, false, false },
                {false, true, false, false },
                {false, true , true, false },
                {false, true, false, false }
            }
        };

        #endregion

        public Color color;

        private float rotationDelay = 0.25f;
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
                    shape = OShape[0];
                    color = Color.Yellow;
                    currentShapeOffset = shapeOffsets[0];
                    break;
                case "I":
                    shape = IShape[0];
                    color = Color.Cyan;
                    currentShapeOffset = shapeOffsets[1];
                    break;
                case "S":
                    shape = SShape[0];
                    color = Color.Coral;
                    currentShapeOffset = shapeOffsets[4];
                    break;
                case "Z":
                    shape = ZShape[0];
                    color = Color.Green;
                    currentShapeOffset = shapeOffsets[5];
                    break;
                case "L":
                    shape = LShape[0];
                    color = Color.Orange;
                    currentShapeOffset = shapeOffsets[3];
                    break;
                case "J":
                    shape = JShape[0];
                    color = Color.Pink;
                    currentShapeOffset = shapeOffsets[2];
                    break;
                case "T":
                    shape = TShape[0];
                    color = Color.Purple;
                    currentShapeOffset = shapeOffsets[6];
                    break;
            }
        }

        public bool[,] GetNextRotation()
        {
            if (currentShape == "O")
            {
                return shape;
            }

            int nextRotation = (currentRotation + 1) % GetRotationArrayLength(currentShape);
            return GetShapeForRotation(currentShape, nextRotation);
        }

        public void Rotate()
        {
            if (currentRotationDelay > 0)
            {
                return;
            }

            currentRotation = (currentRotation + 1) % GetRotationArrayLength(currentShape);
            shape = GetShapeForRotation(currentShape, currentRotation);
        }

        private bool[,] GetShapeForRotation(string shapeType, int rotationIndex)
        {
            switch (shapeType)
            {
                case "I": return IShape[rotationIndex];
                case "L": return LShape[rotationIndex];
                case "S": return SShape[rotationIndex];
                case "Z": return ZShape[rotationIndex];
                case "T": return TShape[rotationIndex];
                case "J": return JShape[rotationIndex];
            }
            return OShape[0];
        }

        private int GetRotationArrayLength(string shapeType)
        {
            switch (shapeType)
            {
                case "I": return IShape.Length;
                case "L": return LShape.Length;
                case "S": return SShape.Length;
                case "Z": return ZShape.Length;
                case "T": return TShape.Length;
                case "J": return JShape.Length;
            }
            return OShape.Length;
        }
    }
}