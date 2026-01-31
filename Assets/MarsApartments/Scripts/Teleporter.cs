using UnityEngine;
using System.Collections;

/// The Teleporter class moves the viewer between a predetermined set of waypoints whenever they touch the screen.
public class Teleporter : MonoBehaviour {
    // How tall is the player, in meters?
    public float height = 1.75f;
    // How fast to move to new location?
    public float speed = 10.0f;

    // flag that prevents teleportation
    private bool blocked = false;

    // Cached transforms for all waypoints
    private Transform[] waypoints;
    // Which waypoint is active?
    private int currentWaypointIndex = 0; 

    // Reference to the first active camera in the scene (doesn't have to be tagged as MainCamera)
    private Camera cam;

    void Start() {
        Input.gyro.enabled = true;

        // Locate the active camera
        Camera[] sceneCams = (Camera[])FindObjectsOfType(typeof(Camera));
        foreach (Camera sceneCam in sceneCams) {
            Transform parent = sceneCam.transform.parent;
            if (parent != null && parent.GetComponent<Camera>() != null) {
                continue;
            } else if (sceneCam.name == "PreRender" || sceneCam.name == "PostRender") {
                continue;
            } else {
                cam = sceneCam;
            }
        }
        if (cam == null) {
            Debug.LogError("No camera found. Please create a Unity camera in the scene.");
            return;
        }

        // Initialize the waypoints
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            waypoints[i] = transform.GetChild(i);
        }
        currentWaypointIndex = 0;
    }

    void Update() {
        // Don't do anything unless the camera can be located
        if (cam == null) {
            return;
        }

        // Gyro rotation
        if (SystemInfo.supportsGyroscope) {
             // Basic magic window implementation
             cam.transform.rotation = Quaternion.Euler(90, 0, 90) * Input.gyro.attitude * Quaternion.Euler(180, 180, 0);
        }

        // If the viewer pressed the mouse button (touch), then go to the next waypoint
        if (Input.GetMouseButtonDown(0) && !blocked) {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        // Smoothly move the viewer towards to the active waypoint
        Vector3 destPos = waypoints[currentWaypointIndex].transform.position + Vector3.up * height;
        cam.transform.position = Vector3.Lerp(cam.transform.position, destPos, Time.deltaTime * speed);
    }

    public void BlockTeleport()
    {
        blocked = true;
    }

    public void UnblockTeleport()
    {
        blocked = false;
    }
}
