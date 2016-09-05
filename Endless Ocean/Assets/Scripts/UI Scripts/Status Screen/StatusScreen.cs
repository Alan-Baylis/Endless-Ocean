using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This is the class for displaying the status screen.
/// </summary>
public class StatusScreen : MonoBehaviour {

    private PlayerController player;

    public Text totalExperienceLabel;
    public Text nextExperienceLabel;
    public Text healthLabel;
    public Text damageLabel;
    public Text movementSpeedLabel;
    public Text energyLabel;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();


	}
	
	// Update is called once per frame
	void Update () {
        this.totalExperienceLabel.text = player.totalExperience.ToString();
        this.damageLabel.text = player.attack.ToString();
        this.healthLabel.text = player.maxHealth.ToString();
        this.energyLabel.text = player.maxEnergy.ToString();
        this.movementSpeedLabel.text = player.movementSpeed.ToString();
    }
}
