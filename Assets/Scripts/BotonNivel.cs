using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonNivel : MonoBehaviour {

    [HideInInspector] public int nivel = 0;
    public Sprite nivelBloqueado;
    public Image star;    

    public void AsignarNivel(int level)
    {
        //Comprobar si este nivel esta desbloqueado
        //En funcion de esto, cambiar imagen y stats
        if (!Game.currentGame.playedLevels.ContainsKey(level))
        {
            GetComponent<Button>().enabled = false; //No carga nivel 
            GetComponent<Image>().sprite = nivelBloqueado;
        }
        else
        {
            Game.currentGame.score = (int)Game.currentGame.playedLevels[level][0];
            Game.currentGame.stars = Game.currentGame.playedLevels[level][1];
            for (int i = 1; i <= Game.currentGame.stars; i++)
            {
                Image newobj = Instantiate(star);
                newobj.transform.SetParent(this.gameObject.transform);
                int posX = (i < 2) ? -1 : 1;
                if (i == 2) posX = 0;
                newobj.rectTransform.localScale = new Vector3(1, 1, 0);
                newobj.rectTransform.localPosition = new Vector3(posX * 30, -30, 0);
            }
            nivel = level;
            GetComponentInChildren<Text>().text = nivel.ToString();
        }
    }

    public void CargarNivel()
    {        
        LevelManager.instance.CargarNivel(nivel);
    }
}
