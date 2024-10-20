using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tetris;

class TetrisGrid
{
    public Texture2D emptyCell;

    Vector2 gridPosition;

    public int Width { get { return 10; } }
    public int Height { get { return 20; } }
    public int cellWidth = 40, cellHeight = 40, screenWidth = 800, screenHeight = 800;
    public Color[,] cells = new Color[10, 20];

    public List<TetrisBlock> currentBlocks = new List<TetrisBlock>();

    public Score score;

    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        gridPosition = Vector2.Zero;
        Clear();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        CheckRow();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                spriteBatch.Draw(emptyCell, new Rectangle(j * cellWidth, i * cellHeight, cellWidth, cellWidth), cells[j, i]);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                cells[j, i] = Color.White;
            }
        }
    }

    public void CheckRow()
    {
        for (int i = 0; i < Height; i++)
        {
            bool rowIsFull = true;
            for (int j = 0; j < Width; j++)
            {
                if (cells[j, i] == Color.White)
                {
                    rowIsFull = false;
                }
            }

            if (rowIsFull)
            {
                score.IncreaseLineScore();
                ClearCompletedRows(i);
            }
        }
    }

    private void ClearCompletedRows(int rowHeight)
    {
        for (int i = rowHeight; i > 0; i--)
        {
            for (int j = 0; j < Width; j++)
            {
                if (i - 1 > 0)
                {
                    cells[j, i] = cells[j, i - 1];
                }
            }
        }
    }
}