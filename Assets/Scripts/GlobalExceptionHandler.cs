using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalExceptionHandler : MonoBehaviour
{
    private ISceneNavigator _sceneNavigator;
    private IApplicationModel _applicationModel;
    public LandingSceneGod sceneNavigator;
    public int defaultLevel;

    void Awake()
    {
#if GLOBAL_EXCEPTION_HANDLER

        Application.logMessageReceived += HandleException;
        DontDestroyOnLoad(gameObject);

      /*  _sceneNavigator = sceneNavigator;
        _applicationModel = ApplicationModelProvider.ApplicationModel;

        if (_sceneNavigator == null)
        {
            // TEMP  Force level I'm currently testing :)
            _applicationModel.SelectedLevel = defaultLevel;
            //Debug.Log(applicationModel.SelectedLevel);
            _sceneNavigator = Substitute.For<ISceneNavigator>();
        }*/
#endif
        //if not implemented then do nothing
    }

    void HandleException(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)//on exception 
        {
            Debug.Log("Exception caught and logged for analytics - remove GLOBAL_EXCEPTION_HANDLER - to disable");
            Dictionary<string, object> logErrorDetail = new Dictionary<string, object>() { { "logString", logString },{ "stackTrace", stackTrace }};
            AnalyticsService.Instance.CustomData("AppException", logErrorDetail);
            AnalyticsService.Instance.Flush();
        }  
    }

}
