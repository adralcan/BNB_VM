using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad {

    public static Game savedGame;

    // Para guardar el estado del juego
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter(); // Gestiona el trabajo de serializacion
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGame.gd"); // puntero al archivo para poder enviar los datos
                                                                                         // Unity cuenta con una ruta por defecto para almacenar
                                                                                         // los archivos del juego. Esta ruta se actualiza segun
                                                                                         // la plataforma para la que generemos el juego.
        bf.Serialize(file, SaveLoad.savedGame);
        file.Close();
    }

    public static bool Load()
    {

        SaveLoad.savedGame = new Game();
        Game.currentGame = new Game();

        if (!File.Exists(Application.persistentDataPath + "/savedGame.gd"))
        {
            return false;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/savedGame.gd", FileMode.Open); // El FileStream es esta vez para leer 
                                                                                                      // los datos desde el archivo
        SaveLoad.savedGame = (Game)bf.Deserialize(file); // La funcion Deserialize busca el archivo en la ubicación que hemos especificado
                                                         // anteriormente y lo deserializa
                                                         // Debemos castear el archivo deserializado al tipo de datos que queremos que sea.
                                                         // Después se lo asignamos a nuestra partida guardada
        savedGame.monedas += 10;
        Game.currentGame = SaveLoad.savedGame;
        Debug.Log(Application.persistentDataPath);
        file.Close();
        return true;
    }
}
