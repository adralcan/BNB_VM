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
        for (int i = 0; i < numberToCreate; i++)
        {
            aux = Instantiate(prefab, transform);
            aux.GetComponent<BotonNivel>().AsignarNivel(i);
            //Asignar numero y nivel
            //go.GetComponent<Image>().color = Random.ColorHSV();
        }
    }

}
