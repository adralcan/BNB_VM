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
        if (!GameManager.instance.currentGame.playedLevels.ContainsKey(level))
        {
            GetComponent<Button>().enabled = false; //No carga nivel 
            GetComponent<Image>().sprite = nivelBloqueado;
        }
        else
        {
            GameManager.instance.currentGame.stats[0] = (int)GameManager.instance.currentGame.playedLevels[level][0];
            GameManager.instance.currentGame.stats[1] = GameManager.instance.currentGame.playedLevels[level][1];
            for (int i = 1; i <= GameManager.instance.currentGame.stats[1]; i++)
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
        Debug.Log("NIVEL: " + nivel);
        LevelManager.instance.CargarNivel(nivel);
    }
}
