using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Emit;
using Tetris;
using static Tetris.GameState;
using Microsoft.Xna.Framework.Audio;

class GameWorld
{
    public static Random Random { get { return random; } }
    static Random random;

    private SpriteFont font;
    public TetrisGrid grid;
    private Score score;
    private GameState gameState;

    private BlockSpawner blockSpawner;
    private TetrisBlock currentBlock, nextBlock;
    private Vector2 nextBlockPosition;

    private int currentXOffset = 0, currentYOffset = 0;
    private float currentMoveDelayY, moveDelayX = 0.075f, currentMoveDelayX, startMoveDelayY = 1f, moveDelayY, minMoveDelayY = 0.75f, currentPlayerMoveDelay;

    private SoundEffect lockSound, gameOverSound;

    private bool screenIsShaking = false;
    private Vector2 shakeAmount = new Vector2(3, 1);
    private int currentShakeAmount, maxShakes = 3;

    public GameWorld()
    {
        random = new Random();

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        lockSound = TetrisGame.ContentManager.Load<SoundEffect>("Lock Block Sound");
        gameOverSound = TetrisGame.ContentManager.Load<SoundEffect>("Game Over Sound");

        score = new Score();
        grid = new TetrisGrid();

        blockSpawner = new BlockSpawner();
        blockSpawner.gameWorld = this;
        blockSpawner.grid = grid;
        blockSpawner.SpawnBlock();

        gameState = new GameState();
        grid.score = score;

        score.font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        gameState.font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        moveDelayY = startMoveDelayY;

        nextBlockPosition = new Vector2(670, grid.screenHeight * 0.2f);
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        if ((inputHelper.KeyPressed(Keys.R) || inputHelper.KeyPressed(Keys.Up)) && currentBlock != null)
        {
            bool[,] newShape = currentBlock.GetNextRotation();
            if (!CheckBlockCollision(currentXOffset, currentYOffset, newShape))
            {
                currentBlock.Rotate();
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Space) && gameState.currentState == GameState.state.GameOver)
        {
            Reset();
        }

        if (screenIsShaking && currentShakeAmount < maxShakes)
        {
            shakeAmount *= -1;
            currentShakeAmount++;
        }
        else
        {
            currentShakeAmount = 0;
            screenIsShaking = false;
        }

        if (gameState.currentState == GameState.state.GameOver)
        {
            return;
        }

        currentBlock = blockSpawner.GetNewBlock();
        nextBlock = blockSpawner.GetNextBlock();

        if (currentMoveDelayY <= 0 || (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S) && currentPlayerMoveDelay <= 0) && currentBlock != null)
        {
            if (!CheckBlockCollision(currentXOffset, currentYOffset + 1))
            {
                currentYOffset++;
                currentMoveDelayY = moveDelayY;
                currentPlayerMoveDelay = startMoveDelayY * 0.75f;
            }
            else
            {
                score.IncreaseBlockScore();
                LockBlock();
                currentMoveDelayY = moveDelayY;
                blockSpawner.SpawnBlock();
            }
        }

        if (currentBlock != null)
        {
            currentMoveDelayY -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentMoveDelayX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            currentMoveDelayY = moveDelayY;
            currentMoveDelayX = moveDelayX;
        }

        if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && currentMoveDelayX <= 0 && currentBlock != null)
        {
            if (!CheckBlockCollision(currentXOffset - 1, currentYOffset))
            {
                currentMoveDelayX = moveDelayX;
                currentXOffset--;
            }
        }

        if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && currentMoveDelayX <= 0 && currentBlock != null)
        {
            if (!CheckBlockCollision(currentXOffset + 1, currentYOffset))
            {
                currentMoveDelayX = moveDelayX;
                currentXOffset++;
            }
        }

        if (score.CheckLevelUp())
        {
            IncreaseDifficulty();
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (screenIsShaking)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateTranslation(shakeAmount.X, shakeAmount.Y, 0));
        }
        else
        {
            spriteBatch.Begin();
        }
        

        grid.Draw(gameTime, spriteBatch);
        score.Draw(spriteBatch);
        gameState.Draw(spriteBatch);

        if (currentBlock != null) 
        {
            DrawCurrentBlock(spriteBatch, currentBlock);
        }

        if (nextBlock != null)
        {
            spriteBatch.DrawString(font, "next block", new Vector2(grid.screenWidth - 10 - font.MeasureString("next block").X, grid.screenHeight * 0.4f), Color.Black);
            DrawNextBlock(spriteBatch, nextBlock);
        }

        spriteBatch.End();
    }

    private void DrawCurrentBlock(SpriteBatch spriteBatch, TetrisBlock block) 
    {
        for (int i = 0; i < block.shape.GetLength(0); i++)
        { 
            for (int j = 0;  j < block.shape.GetLength(1); j++)
            {
                if (block.shape[i, j])
                {
                    int blockX = j * grid.cellWidth + (currentXOffset * grid.cellWidth);
                    int blockY = i * grid.cellHeight + (currentYOffset * grid.cellHeight);

                    spriteBatch.Draw(grid.emptyCell, new Rectangle(blockX, blockY, grid.cellWidth, grid.cellHeight), block.color);
                }
            }
        }
    }

    private bool CheckBlockCollision(int xOffset, int yOffset, bool[,] blockShape = null)
    {
        blockShape = blockShape ?? currentBlock.shape;

        for (int i = 0; i < blockShape.GetLength(0); i++)
        {
            for (int j = 0; j < blockShape.GetLength(1); j++)
            {
                if (blockShape[i, j])
                {
                    int gridX = xOffset + j;
                    int gridY = yOffset + i;

                    if (gridX < 0 || gridX >= grid.Width || gridY >= grid.Height)
                    {
                        return true; //collides with grid boundaries
                    }

                    if (gridY >= 0 && grid.cells[gridX, gridY] != Color.White)
                    {
                        return true; //overlap with another block
                    }
                }
            }
        }

        return false;
    }

    private void LockBlock()
    {
        screenIsShaking = true;
        for (int i = 0; i < currentBlock.shape.GetLength(0); i++)
        {
            for (int j = 0; j < currentBlock.shape.GetLength(1); j++)
            {
                if (currentBlock.shape[i, j])
                {
                    int gridX = currentXOffset + j;
                    int gridY = currentYOffset + i;

                    if (gridY <= 0)
                    {
                        maxShakes = 6;
                        screenIsShaking = true;
                        gameOverSound.Play();
                        gameState.currentState = GameState.state.GameOver;
                    }
                    else if (gridX >= 0 && gridX < grid.Width && gridY < grid.Height)
                    {
                        grid.cells[gridX, gridY] = currentBlock.color;
                    }
                }
            }
        }

        lockSound.Play();
        currentBlock = null;
    }

    private void DrawNextBlock(SpriteBatch spriteBatch, TetrisBlock block)
    {
        for (int i = 0; i < block.shape.GetLength(0); i++)
        {
            for (int j = 0; j < block.shape.GetLength(1); j++)
            {
                if (block.shape[i, j])
                {
                    int blockX = (int)nextBlockPosition.X + j * grid.cellWidth;
                    int blockY = (int)nextBlockPosition.Y + i * grid.cellHeight;

                    spriteBatch.Draw(grid.emptyCell, new Rectangle(blockX, blockY, grid.cellWidth, grid.cellHeight), block.color);
                }
            }
        }
    }

    private void IncreaseDifficulty()
    {
        if (moveDelayY - 0.02f > minMoveDelayY)
        {
            moveDelayY -= 0.02f;
        }
    }

    public void SetSpawnPosition(int y)
    {
        currentXOffset = 0;
        currentYOffset = y;
    }

    public void Reset()
    {
        currentMoveDelayX = 0;
        currentMoveDelayY = 0;
        moveDelayY = startMoveDelayY;
        currentXOffset = 0;
        currentYOffset = 0;
        currentPlayerMoveDelay = 0;
        currentBlock = null;
        nextBlock = null;
        screenIsShaking = false;
        maxShakes = 3;
        blockSpawner.Reset();
        grid.Clear();
        score.Reset();
        blockSpawner.SpawnBlock();
        gameState.currentState = GameState.state.Playing;
    }
}