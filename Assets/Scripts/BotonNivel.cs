using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonNivel : MonoBehaviour {

    [HideInInspector] public int nivel = 0;
    public Sprite nivelBloqueado;

    public void AsignarNivel(int level)
    {
        //Comprobar si este nivel esta desbloqueado
        //En funcion de esto, cambiar imagen y stats
        if (!Game.currentGame.playedLevels.ContainsKey(level)) //Creo que es key y no value
        {
            GetComponent<Button>().enabled = false; //No carga nivel 
            GetComponent<Image>().sprite = nivelBloqueado;
        }
        else
        {
            nivel = level;
            GetComponentInChildren<Text>().text = nivel.ToString();
        }
    }

    public void CargarNivel()
    {
        LevelManager.instance.CargarNivel(nivel);
    }
}
