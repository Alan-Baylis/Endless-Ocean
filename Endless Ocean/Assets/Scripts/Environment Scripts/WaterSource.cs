using UnityEngine;
using System.Collections;

/// <summary>
/// Class for water source particle systems.
/// 
/// All this class does is start the water rising when its particle collide with a body of water.
/// </summary>
public class WaterSource : MonoBehaviour {

    public bool addingWater;

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
            }
        }
    }

}
