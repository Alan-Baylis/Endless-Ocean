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

    protected float qualityModifier = 1;

    protected int qualityInt;

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
        qualityInt = Random.Range(1, 100);
        qualityInt = (int)(qualityInt * luck);
        //if (quality != ItemQuality.NULL)
        //{
        //    qualityInt = (int)quality;
        //}
        if (qualityInt <= (int)ItemQuality.Crude)
        {
            qualityModifier = 0.5f;
            quality = ItemQuality.Crude;
        }
        else if (qualityInt <= (int)ItemQuality.Basic)
        {
            qualityModifier = 1f;
            quality = ItemQuality.Basic;
        }
        else if (qualityInt <= (int)ItemQuality.Improved)
        {
            qualityModifier = 1.5f;
            quality = ItemQuality.Improved;
        }
        else if (qualityInt <= (int)ItemQuality.Legendary)
        {
            qualityModifier = 2f;
            quality = ItemQuality.Legendary;
        }
        else
        {
            qualityModifier = 100f;
            quality = ItemQuality.Godly;
        }
        if((quality != ItemQuality.Crude ) && (quality != ItemQuality.Basic)){
            this.generateBonuses((qualityInt * GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentLevel) / 80);
        }
    }

    /// <summary>
    /// This method generates the bonuses for newly created equipment.
    /// </summary>
    /// <param name="bonusPoints">The points to spend on bonuses.</param>
    protected virtual void generateBonuses(int bonusPoints)
    {
        bonusPoints = (bonusPoints * GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentLevel);
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
    public virtual int getDefense()
    {
        return (qualityInt);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}



