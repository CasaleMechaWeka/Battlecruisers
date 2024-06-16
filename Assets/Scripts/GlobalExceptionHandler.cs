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

        /* _sceneNavigator = sceneNavigator;
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
        if (type == LogType.Exception) // on exception
        {
            Debug.Log("Exception caught and logged for analytics - remove GLOBAL_EXCEPTION_HANDLER - to disable");
            Dictionary<string, object> logErrorDetail = new Dictionary<string, object>() { { "logString", logString }, { "stackTrace", stackTrace } };

            // Detailed logging for NullReferenceException
            if (logString.Contains("NullReferenceException"))
            {
                Debug.LogError("NullReferenceException occurred. Detailed investigation needed.");

                // Add any additional logging here to capture the state of variables or context information
                LogDetailedContextInformation();
            }

            try
            {
                AnalyticsService.Instance.CustomData("AppException", logErrorDetail);
                AnalyticsService.Instance.Flush();
            }
            catch (ConsentCheckException ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }

    void LogDetailedContextInformation()
    {
        // Add detailed logging here for the context in which the exception occurred
        // For example, log the state of key variables or objects
        if (_sceneNavigator != null)
        {
            Debug.Log($"Scene Navigator is set: {_sceneNavigator}");
        }
        else
        {
            Debug.LogError("Scene Navigator is null");
        }

        if (_applicationModel != null)
        {
            Debug.Log($"Application Model Selected Level: {_applicationModel.SelectedLevel}");
        }
        else
        {
            Debug.LogError("Application Model is null");
        }
    }
}
