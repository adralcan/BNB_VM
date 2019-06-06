using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class UnityAdsButton : MonoBehaviour
{
#if UNITY_IOS
    private string gameId = "1486551";
#elif UNITY_ANDROID || UNITY_EDITOR
    private string gameId = "3159208"; //"2989616";
#endif

    Button m_Button;
    Image imagen;
    Color colorOriginal;

    public int flashSpeed = 5;

    string placementId = "rewardedVideo";

    void Start()
    {
        m_Button = GetComponent<Button>();
        if (m_Button)
            m_Button.onClick.AddListener(ShowAd);
                
        if (Advertisement.isSupported)        
            Advertisement.Initialize(gameId, true);

        if (GetComponent<Image>())
        {
            imagen = GetComponent<Image>();
            colorOriginal = imagen.color;
        }
    }    

    //Listener, metodo llamado al hacer click
    void ShowAd()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show(placementId, options);       

        if (LevelManager.instance.textoGemas != null)
            LevelManager.instance.textoGemas.text = Game.currentGame.gemas.ToString();
        
        GameManager.instance.anuncioActivo = true; 
    }

    void HandleShowResult(ShowResult result)
    {        
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            GameManager.instance.anuncioActivo = false;

            Game.currentGame.monedas += 100;

            if (LevelManager.instance.gemasParticulas != null)
                LevelManager.instance.gemasParticulas.Play();

            SaveLoad.savedGame = Game.currentGame;
            SaveLoad.Save();
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");
            GameManager.instance.anuncioActivo = false;
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
            GameManager.instance.anuncioActivo = false;
        }
    }

    IEnumerator BrillaImagen()
    {
        imagen.color = imagen.color*1.2f;

        while (imagen.color != colorOriginal)
        {
            imagen.color = Color.Lerp(Color.white, imagen.color, flashSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        yield break;        
    }

    
}