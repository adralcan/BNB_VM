using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]  // Gracias a esto se puede serializar este objeto. 
                       // Podemos guardar todas las variables de este script.
public class Game : System.Object
{
    public static Game currentGame; //Referencia estatica a una instancia de la clase

    // Las clases que queramos serializar
    // public Niveles nivelesJugados;

    // 1 Score 2 Stars
    public int score, stars;

    public Dictionary<int, int[]> playedLevels;
    public int monedas { get; set; }
    public int powerUp { get; set; } //Numero de veces que podemos usar el powerUp
    public int gemas { get; set; } //Numero de veces que podemos usar el powerUp

    public Game()
    {
        playedLevels = new Dictionary<int, int[]>();
        score = 0;
        stars = 0;
        monedas = 0;
        gemas = 0;
    }
	
}
