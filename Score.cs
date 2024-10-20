using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Tetris
{
    class Score
    {
        public SpriteFont font;
        public SpriteBatch spriteBatch;

        //constants
        public const int blockScore = 10;
        public const int lineScore = 100;

        //variables
        private int currentScore = 0;
        private int level = 1;
        private int levelGoal = 100;

        public bool CheckLevelUp()
        {
            if (currentScore >= levelGoal)
            {
                levelGoal *= 2;
                level++;
                return true;
            }

            return false;
        }

        private void DrawGameScore(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, $"level: {level}", new Vector2(800 - 10 - font.MeasureString($"level: {level}").X, 10), Color.Black);
            spriteBatch.DrawString(font, $"next level: {levelGoal}", new Vector2(800 - 10 - font.MeasureString($"next level: {levelGoal}").X, 30), Color.Black);
            spriteBatch.DrawString(font, $"score: {currentScore}", new Vector2(800 - 10 - font.MeasureString($"score: {currentScore}").X, 50), Color.Black);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawGameScore(spriteBatch);
        }

        public void IncreaseBlockScore()
        {
            currentScore += blockScore;
        }

        public void IncreaseLineScore()
        {
            currentScore += lineScore;
        }

        public void Reset()
        {
            currentScore = 0;
            level = 1;
            levelGoal = 100;
        }
    }
}