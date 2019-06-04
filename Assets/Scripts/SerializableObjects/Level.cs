using System.Collections;
using UnityEngine;

[System.Serializable]
public class Level
{
    public static Level currentLevel; //Referencia estatica a una instancia de la clase

    public int levelID;
    public float score; //Lo necesito float para la barra de puntos
    public float maxScore;
    public int stars;

    public Level()
    {
        levelID = 0;
        score = 0;
        stars = 0;
    }
    public Level(int levelID, int score)
    {
        this.levelID = levelID;
        this.score = score;
        stars = 0;
    }

    public void AddStar(int combo)
    {

        if (stars < 3)
        {
            if (stars < 2)
            {
                if (stars < 1)
                {
                    if (score > maxScore * 0.2f)
                    {
                        stars++;
                    }
                }
                else if (score > maxScore * 0.7f)
                {
                    stars++;
                }
            }
            else if (score > maxScore)
            {
                stars++;

            }
        }
    }
}