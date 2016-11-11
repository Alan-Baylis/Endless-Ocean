using UnityEngine;
using System.Collections;

/// <summary>
/// Class for water source particle systems.
/// 
/// All this class does is start the water rising when its particles collide with a body of water.
/// </summary>
public class WaterSource : MonoBehaviour {

    public bool addingWater;
    public Camera cam;
    float smoothing = 1.0f;
    bool performMove;
    private float startTime;
    private float DistanctToTravel = 0.0f;

    //locations to travel between
    public Vector3 posEnd;
    public Vector3 posStart;

    // Use this for initialization
    void Start()
    {
        performMove = false;
    }

    /// <summary>
    /// Shows that this water source is adding water when its particles collide with a body of water.
    /// </summary>
    void OnParticleTrigger()
    {
        if(this.gameObject.GetComponent<ParticleSystem>().GetTriggerParticles(ParticleSystemTriggerEventType.Enter, new System.Collections.Generic.List<ParticleSystem.Particle>()) > 0)
        {
            this.addingWater = true;
            this.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (performMove)
        {
            float distCovered = (Time.time - startTime) * 25.0f;
            float smoothing = distCovered / DistanctToTravel;
            cam.transform.position = Vector3.Lerp(posStart, posEnd, smoothing);
            StartCoroutine(wait());
        }
    }

    IEnumerator wait()
    {
        while (posEnd != cam.transform.position)
        {
            yield return null;
        }
        yield return new WaitForSeconds(5);
        cam.GetComponent<CameraController>().followPlayer = true;
        GameObject.Find("Player").GetComponent<PlayerController>().enableMove = true;
        this.gameObject.SetActive(false);
        yield return null;
    }

    /// <summary>
    /// Allows the user to start water source pouring water.
    /// </summary>
    /// <param name="other">The other collider inside the water sources collider.</param>
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                this.GetComponent<ParticleSystem>().enableEmission = true;
                cam.GetComponent<CameraController>().followPlayer = false;
                GameObject.Find("Player").GetComponent<PlayerController>().enableMove = false;

                startTime = Time.time;
                posStart = cam.transform.position;
                posEnd = new Vector3(200f, 3f, cam.transform.position.z);
                DistanctToTravel = Vector3.Distance(posEnd, posStart);
                performMove = true;
                
            }
        }
    }

}
