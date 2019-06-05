using System.Collections;
using UnityEngine;

[System.Serializable]
public class Level : System.Object
{
    public int levelID { get; set; }
    public float score { get; set; }//Lo necesito float para la barra de puntos
    public float maxScore { get; set; }
    public int stars { get; set; }

    public Level()
    {
        levelID = 1;
        score = 0;
        stars = 0;
    }

    public Level(int levelID, int score)
    {
        this.levelID = levelID;
        this.score = score;
        this.stars = 0;
    }

    public void AddStar()
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