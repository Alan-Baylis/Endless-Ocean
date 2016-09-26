using UnityEngine;
using System.Collections;

/// <summary>
/// This class contians functionality unique to chestpieces.
/// </summary>
public class ChestPiece : Equipment {

    /// <summary>
    /// Generates bonus stats for chest pieces. Only vigor and stamina.
    /// </summary>
    /// <param name="bonusPoints">The number of points to spend on stats.</param>
    protected new void generateBonuses(int bonusPoints)
    {
        bonusPoints = (bonusPoints * GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentLevel);
        System.Random random = new System.Random();
        for (int i = 0; i < bonusPoints; i++)
        {
            int randomNum = random.Next(0, 99);
            if (randomNum < 25)
            {
                this.vigorBonus++;
            }
            else if (25 <= randomNum && randomNum < 50)
            {
                this.staminaBonus++;
            }
        }
    }

    /// <summary>
    /// Returns the chest pieces defense.
    /// </summary>
    /// <returns>The chest pieces defense.</returns>
    public new int getDefense()
    {
        return (((int)this.qualityInt / 15) * GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentLevel);
    }
}
