using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;
    bool _isInit = false;

    void Start()
    {
        _testMode = false;
        InitializeAds();
        StartCoroutine(initt());
    }

    IEnumerator initt()
    {
        yield return new WaitForSeconds(5);
        if(!_isInit )
        {
            InitializeAds();
            StartCoroutine(initt());
        }
        
    }

    public static bool HasConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = new WebClient().OpenRead("http://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }


    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (HasConnection() && !Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
            _isInit = true;
        }


    }



    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        GetComponent<InterstitialAd>().LoadAd();
        GetComponent<RewardedAd>().LoadAd();
        //this.gameObject.GetComponent<RewardedAd>().LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}