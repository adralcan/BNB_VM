using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PowerUpControl : MonoBehaviour {

    public GameObject powerUp;

	public void ActivarPowerUp() {

        if (GameManager.instance.currentGame.monedas >= 50)
        {
            GameManager.instance.currentGame.monedas -= 50;
            SaveLoad.Save();
            //M-top        
            Renderer rend = LevelManager.instance.spriteField.GetComponent<Renderer>();
            GameObject aux = Instantiate(powerUp);

            float y = LevelManager.instance.listaBloques[LevelManager.instance.listaBloques.Count - 1].gameObject.transform.position.y;
            
            aux.transform.localScale = new Vector3(50, 0.1f, 10);
            aux.transform.position = new Vector3(rend.bounds.center.x, y, 0);
        }

    }
}
