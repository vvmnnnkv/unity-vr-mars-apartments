using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class CardboardStartup : MonoBehaviour
{
    void Awake()
    {
        // Keep this object alive
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(StartXR());
    }

    public IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");

        // Ensure XR settings exist, or create temporary instance if possible.
        // Note: Runtime creation of complete XRGeneralSettings from scratch is complex due to internal protections.
        // However, if the project builds, we rely on the XR Management system to try and load the default loader if configured.
        // If not configured (because we didn't commit the .asset files), we try to force it.

        if (XRGeneralSettings.Instance == null)
        {
            Debug.LogError("XRGeneralSettings instance is null. Please configure XR Plug-in Management in Project Settings.");
            yield break;
        }

        if (XRGeneralSettings.Instance.Manager == null)
        {
             Debug.LogError("XRManagerSettings instance is null.");
             yield break;
        }

        // Check if already initialized
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            Debug.Log("XR already initialized.");
            yield break;
        }

        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player logs for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }
}
