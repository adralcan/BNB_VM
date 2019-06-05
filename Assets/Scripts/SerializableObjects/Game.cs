using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]  // Gracias a esto se puede serializar este objeto. 
                       // Podemos guardar todas las variables de este script.

public class Game {
    public static Game currentGame; //Referencia estatica a una instancia de la clase

    // Las clases que queramos serializar
    // public Niveles nivelesJugados;
    [System.Serializable]
    public struct Puntuacion
    {
        public int score;
        public int stars;
    };

    public Puntuacion stats;

    public Dictionary<int, Puntuacion> playedLevels;
    public int monedas;
    public int powerUp; //Numero de veces que podemos usar el powerUp

    public Game()
    {
        playedLevels = new Dictionary<int, Puntuacion>();
        monedas = 0;
    }
	
}
