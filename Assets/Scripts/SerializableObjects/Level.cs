using System.Collections;
using UnityEngine;

[System.Serializable]
public class Level
{
    public static Level currentLevel; //Referencia estatica a una instancia de la clase

    public int levelID;
    public int score;
    public int maxScore;
    public int stars;

    public Level()
    {
        levelID = 0;
        score = 0;
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
                        Debug.Log("-----------------------------PRIMERA ESTRELLA " + Time.time + " con combo: " + combo);
                    }
                }
                else if (score > maxScore * 0.7f)
                {
                    stars++;
                    Debug.Log("-----------------------------SEGUNDA ESTRELLA " + Time.time + " con combo: " + combo);
                }
            }
            else if (score > maxScore)
            {
                stars++;
                Debug.Log("-----------------------------TERCERA ESTRELLA " + Time.time + " con combo: " + combo);

            }
        }
    }
}