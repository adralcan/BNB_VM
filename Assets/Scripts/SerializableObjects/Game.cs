using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]  // Gracias a esto se puede serializar este objeto. 
                       // Podemos guardar todas las variables de este script.

public class Game {
    public static Game currentGame; //Referencia estatica a una instancia de la clase

    // Las clases que queramos serializar
    // public Niveles nivelesJugados;
    public Dictionary<int, int> playedLevels;
    public int monedas;

    public Game()
    {
        playedLevels = new Dictionary<int, int>();
        monedas = 0;
    }
	
}
