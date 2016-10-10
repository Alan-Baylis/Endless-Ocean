using UnityEngine;
using System.Collections;

public class zoomCameraCollider : MonoBehaviour
{
    bool zoomIn;
    public Camera camera;

    float fieldOfView;
    float maxFieldOfView;
    float targetZoom;

    // Use this for initialization
    void Start()
    {
        maxFieldOfView = camera.fieldOfView;
        fieldOfView = maxFieldOfView;
        targetZoom = 25.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (zoomIn)
        {
            StartCoroutine(performZoomIn());
        }
        else if (!zoomIn)
        {
            StartCoroutine(performZoomOut());
        }
    }

    IEnumerator performZoomIn()
    {
        while (fieldOfView > targetZoom)
        {
            this.fieldOfView -= Time.deltaTime / 8;
            this.camera.fieldOfView = this.fieldOfView;
            yield return null;
        }
        yield return new WaitForSeconds(2);

        yield return null;
    }

    IEnumerator performZoomOut()
    {
        while (fieldOfView < maxFieldOfView)
        {
            this.fieldOfView += Time.deltaTime / 8;
            this.camera.fieldOfView = this.fieldOfView;
            yield return null;
        }
        yield return new WaitForSeconds(2);

        yield return null;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            zoomIn = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            zoomIn = false;
        }
    }
}
