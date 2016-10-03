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

}
