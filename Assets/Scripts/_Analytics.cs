using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;

public class _Analytics : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("asd", 5, 5);

    }

    void asd()
    {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        AnalyticsResult al = Analytics.CustomEvent("Level" + PlayerPrefs.GetInt("Level", 1));
        print(al);
#else
        print("No");
#endif

    }



    void Update()
    {
        
    }
}
