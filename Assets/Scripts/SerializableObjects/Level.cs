using System.Collections;
using UnityEngine;

[System.Serializable]
public class Level
{
    public static Level currentLevel; //Referencia estatica a una instancia de la clase

    public int levelID = 1;
    public float score = 0; //Lo necesito float para la barra de puntos
    public float maxScore;
    public int stars = 0;

    public Level()
    {
        currentLevel = this;
        levelID = 1;
        score = 0;
        stars = 0;
    }

    public Level(int levelID, int score)
    {
        currentLevel = this;
        this.levelID = levelID;
        this.score = score;
        this.stars = 0;
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