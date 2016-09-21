using UnityEngine;
using System.Collections;

public class Equipment : Item
{

    //The body part this equipment is for.
    public Bodypart bodypart;

    public int vigorBonus;
    public int staminaBonus;
    public int moveSpeedBonus;
    public int damageBonus;

    public int defense;

    protected float qualityModifier = 1;

    //Changing buy and sell values based off item quality.
    public override int buyValue
    {
        get
        {
            return ((int)quality * base.buyValue);
        }
    }

    public override int sellValue
    {
        get
        {
            return ((int)quality * base.sellValue);
        }
    }

    public void setQualityAndAttributes(float luck)
    {
        int qualityInt = Random.Range(1, 100);
        qualityInt = (int)(qualityInt * luck);
        //if (quality != ItemQuality.NULL)
        //{
        //    qualityInt = (int)quality;
        //}
        if (qualityInt <= (int)ItemQuality.crude)
        {
            qualityModifier = 0.5f;
            quality = ItemQuality.crude;
        }
        else if (qualityInt <= (int)ItemQuality.basic)
        {
            qualityModifier = 1f;
            quality = ItemQuality.basic;
        }
        else if (qualityInt <= (int)ItemQuality.improved)
        {
            qualityModifier = 1.5f;
            quality = ItemQuality.improved;
            this.generateBonuses(3);
        }
        else if (qualityInt <= (int)ItemQuality.legendary)
        {
            qualityModifier = 2f;
            quality = ItemQuality.legendary;
            this.generateBonuses(7);
        }
        else
        {
            qualityModifier = 100f;
            quality = ItemQuality.godly;
        }
    }

    /// <summary>
    /// This method generates the bonuses for newly created equipment.
    /// </summary>
    /// <param name="bonusPoints">The points to spend on bonuses.</param>
    private void generateBonuses(int bonusPoints)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < bonusPoints; i++)
        {
            int randomNum = random.Next(0, 99);
            if(randomNum < 25)
            {
                this.vigorBonus++;
            }
            else if(25 <= randomNum && randomNum < 50)
            {
                this.staminaBonus++;
            }
            else if (50 <= randomNum && randomNum < 75)
            {
                this.moveSpeedBonus++;
            }
            else if (randomNum > 75)
            {
                this.damageBonus++;
            }
        }
    }

    /// <summary>
    /// This method returns the items defense.
    /// </summary>
    /// <returns>The equipment's defense.</returns>
    public int getDefense()
    {
        return (int) (this.defense * qualityModifier);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}



