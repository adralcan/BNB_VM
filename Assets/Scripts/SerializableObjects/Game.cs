using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]  // Gracias a esto se puede serializar este objeto. 
                       // Podemos guardar todas las variables de este script.
public class Game : System.Object
{
    // Las clases que queramos serializar
    // public Niveles nivelesJugados;
  

    public int score;
    public int stars;

    // 1 Score 2 Stars
    public int [] stats;

    public Dictionary<int, int[]> playedLevels;
    public int monedas { get; set; }
    public int powerUp { get; set; }//Numero de veces que podemos usar el powerUp

    public Game()
    {
        playedLevels = new Dictionary<int, int[]>();
        stats = new int[2];
        monedas = 0;
    }
	
}
