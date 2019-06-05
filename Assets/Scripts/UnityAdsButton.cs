using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class UnityAdsButton : MonoBehaviour
{
#if UNITY_IOS
    private string gameId = "1486551";
#elif UNITY_ANDROID || UNITY_EDITOR
    private string gameId = "3159208"; //"2989616";
#endif

    Button m_Button;

    string placementId = "rewardedVideo";

    void Start()
    {
        m_Button = GetComponent<Button>();
        if (m_Button) m_Button.onClick.AddListener(ShowAd);
                
        if (Advertisement.isSupported)        
            Advertisement.Initialize(gameId, true);              
    }

    void Update()
    {
        if (m_Button) m_Button.interactable = Advertisement.IsReady(placementId);
    }

    void ShowAd()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show(placementId, options);
        GameManager.instance.anuncioActivo = true;
    }

    void HandleShowResult(ShowResult result)
    {        
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            GameManager.instance.anuncioActivo = false;

            Game.currentGame.monedas += 100;
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
}