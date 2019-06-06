using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour {

    public Button prefab;

    public int numberToCreate = 10;

    private void Start()
    {
        Populate();
    }

    void Populate()
    {
        Button aux;
        LevelManager.instance.NuevoNivelSerializable(GameManager.instance.level);
        for (int i = 1; i <= numberToCreate; i++)
        {
            aux = Instantiate(prefab, transform);
            aux.GetComponent<BotonNivel>().AsignarNivel(i);            
        }
    }

}
