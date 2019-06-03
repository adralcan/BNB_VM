using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using System.Text;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public bool anuncioActivo = false; //Si está activo, no hacemos caso al input

    [HideInInspector] public int level = 1;

    // Use this for initialization
    void Awake() {        
        instance = this;
        // Serializacion
        if (SaveLoad.Load()) {
            level = ;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    struct infoBloque
    {
        public int x;
        public int y;
        public int tipo;        
    }        

    public void ReadLevel(string nivel)
    {
        //Lectura de nivel
        TextAsset texto = Resources.Load(nivel) as TextAsset;
        string test = texto.text;

        byte[] byteArray = Encoding.ASCII.GetBytes(test);
        MemoryStream stream = new MemoryStream(byteArray);
        StreamReader archivo = new StreamReader(stream);

        int j = 0;
        int indiceInfoBloques = 0;
        char[] delimiterChar = { ',', '.' };
        string fila;
        List<infoBloque> infoBloques = new List<infoBloque>(); //Almacenamos info primera vuelta, en la segunda, creamos los bloques          

        while ((fila = archivo.ReadLine()) != null)
        {
            string[] split = fila.Split(delimiterChar);

            for (int i = 0; i < split.Length - 1; i++)
            {
                if (j >= 3 && j < 14)
                {
                    infoBloque aux;                    
                    aux.x = i; aux.y = j; aux.tipo = Convert.ToInt32(split[i]);
                    infoBloques.Add(aux);
                }
                if (j > 16 && j < 27) //Para que solo lea 11 filas
                {
                    CrearBloque(infoBloques[indiceInfoBloques].x - 5, infoBloques[indiceInfoBloques].y + 1, infoBloques[indiceInfoBloques].tipo,
                         Convert.ToInt32(split[i]));
                    indiceInfoBloques++;
                }
            } //Fin del for

            j++;
        }
    }

    void CrearBloque(int x, int y, int tipo, int golpes)
    {
        if (tipo > 0 && tipo <= 6)
        {
            Bloque nuevoBloque;
            //Tipos del 1 al 6
            nuevoBloque = Instantiate(LevelManager.instance.prefabsBloques[tipo - 1]);

            nuevoBloque.transform.SetParent(LevelManager.instance.gameField.transform);
            nuevoBloque.SetPosition(new Vector3(x, -y + 9.5f, 0));
            nuevoBloque.setTipo(tipo); //El tipo nos dira si tiene habilidad especial

            nuevoBloque.contGolpes = golpes;
            LevelManager.instance.listaBloques.Add(nuevoBloque);
            LevelManager.instance.numBloques++;
            
        }
        if (tipo == 21)
        {
            Bloque nuevoBloque = Instantiate(LevelManager.instance.prefabsBloques[6]);

            nuevoBloque.transform.SetParent(LevelManager.instance.gameField.transform);
            nuevoBloque.SetPosition(new Vector3(x, -y + 9.5f, 0));
            nuevoBloque.setTipo(tipo); //El tipo nos dira si tiene habilidad especial

            nuevoBloque.contGolpes = 1;
            LevelManager.instance.listaBloques.Add(nuevoBloque);
        }
    }  
    
}

