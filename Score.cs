using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace TetrisTemplate
{
    class Score
    {
        public SpriteFont font;
        public SpriteBatch spriteBatch;
        GameWorld gameWorld;

        //constants
        public static int blockScore = 50;
        public static int lineScore = 500;

        //variables
        public static int score = 0;
        public static int level = 1;
        public static int levelGoal = 500;

        public void checkLevelUp()
        {
            if (score >= levelGoal)
            {
                levelGoal *= 2;
                GameWorld.moveDelayY *= 0.9f;
                level += 1;
            }
        }

        public void GameScore()
        {
            spriteBatch.DrawString(font, $"level: {level}", new Vector2(800 - 10 - font.MeasureString($"level: {level}").X, 10), Color.Black);
            spriteBatch.DrawString(font, $"next level: {levelGoal}", new Vector2(800 - 10 - font.MeasureString($"next level: {levelGoal}").X, 30), Color.Black);
            spriteBatch.DrawString(font, $"score: {score}", new Vector2(800 - 10 - font.MeasureString($"score: {score}").X, 50), Color.Black);
        }

        public void Draw()
        {
            spriteBatch.Begin();
            GameScore();
            spriteBatch.End();
        }
    }
}