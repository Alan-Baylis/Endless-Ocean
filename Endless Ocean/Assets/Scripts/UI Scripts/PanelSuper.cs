using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class PanelSuper : MonoBehaviour {

    public GameObject toolTip;
    /// <summary>
    /// This function shows the tooltip for an item in a panel. It shows different item in the tooltip based on the type of item the user is inspecting.
    /// </summary>
    /// <param name="item">The item to show the tooltip for.</param>
    /// <param name="tooltipPosition">The position to show the tooltip at.</param>
	public virtual void showToolTip(Item item, Vector3 tooltipPosition)
    {
        this.toolTip.transform.Find("Equipment Labels").gameObject.SetActive(false);
        this.toolTip.transform.Find("Weapon Labels").gameObject.SetActive(false);
        this.toolTip.transform.SetAsLastSibling();
        this.toolTip.transform.Find("Item Name").GetComponent<Text>().text = item.itemName;
        this.toolTip.transform.Find("Item Name").GetComponent<Text>().color = item.getQualityColour();
        this.toolTip.transform.Find("Item Description").GetComponent<Text>().text = item.description;
        this.toolTip.transform.Find("Item Image").GetComponent<Image>().sprite = item.itemIcon;
        this.toolTip.GetComponent<RectTransform>().localPosition = new Vector3(tooltipPosition.x + 20, tooltipPosition.y - 20, tooltipPosition.z + 1);
        this.toolTip.transform.Find("Cost Label").GetComponent<Text>().text = "$" + item.buyValue.ToString();
        this.toolTip.SetActive(true);
        //Checking if the item is equipment.
        if(item.GetComponent<Equipment>() != null)
        {
            GameObject equipmentLabels = this.toolTip.transform.Find("Equipment Labels").gameObject;
            equipmentLabels.SetActive(true);
            //Reseting all labels to be black.
            foreach (Transform child in equipmentLabels.transform)
            {
                if (child.gameObject.GetComponent<Text>() != null)
                {
                    child.gameObject.GetComponent<Text>().color = Color.black;
                    if (child.gameObject.GetComponent<Text>().text.All(char.IsDigit))
                    {
                        child.gameObject.GetComponent<Text>().text = 0.ToString();
                    }
                }
            }
            //Showing defense.
            Equipment tempEquipment = item.GetComponent<Equipment>();
            equipmentLabels.transform.Find("Defence Label").GetComponent<Text>().text = tempEquipment.getDefense().ToString();
            //Iterating through all bonuses and if they are > 1 show them else gray out the labels.
            if (tempEquipment.vigorBonus == 0)
            {
                equipmentLabels.transform.Find("Vigor Label").GetComponent<Text>().color = Color.gray;
                equipmentLabels.transform.Find("Vigor Text Label").GetComponent<Text>().color = Color.gray;
            }
            else
            {
                equipmentLabels.transform.Find("Vigor Label").GetComponent<Text>().text = tempEquipment.vigorBonus.ToString();
            }

            if (tempEquipment.staminaBonus == 0)
            {
                equipmentLabels.transform.Find("Stamina Label").GetComponent<Text>().color = Color.gray;
                equipmentLabels.transform.Find("Stamina Text Label").GetComponent<Text>().color = Color.gray;
            }
            else
            {
                equipmentLabels.transform.Find("Stamina Label").GetComponent<Text>().text = tempEquipment.staminaBonus.ToString();
            }

            if (tempEquipment.moveSpeedBonus == 0)
            {
                equipmentLabels.transform.Find("Move Speed Label").GetComponent<Text>().color = Color.gray;
                equipmentLabels.transform.Find("Move Speed Text Label").GetComponent<Text>().color = Color.gray;
            }
            else
            {
                equipmentLabels.transform.Find("Move Speed Label").GetComponent<Text>().text = tempEquipment.moveSpeedBonus.ToString();
            }
            if (tempEquipment.damageBonus == 0)
            {
                equipmentLabels.transform.Find("Damage Label").GetComponent<Text>().color = Color.gray;
                equipmentLabels.transform.Find("Damage Text Label").GetComponent<Text>().color = Color.gray;
            }
            else
            {
                equipmentLabels.transform.Find("Damage Label").GetComponent<Text>().text = tempEquipment.damageBonus.ToString();
            }
        }
        if(item.GetComponent<Weapon>() != null)
        {
            Weapon tempWeapon = item.GetComponent<Weapon>();
            GameObject weaponLabels = this.toolTip.transform.Find("Weapon Labels").gameObject;
            weaponLabels.SetActive(true);
            weaponLabels.transform.Find("Damage Label").GetComponent<Text>().text = tempWeapon.getDamage().ToString();
            weaponLabels.transform.Find("Energy Cost Label").GetComponent<Text>().text = tempWeapon.energyCost.ToString();
            weaponLabels.transform.Find("Knockback Label").GetComponent<Text>().text = tempWeapon.getKnockBack().ToString();

        }
        this.customizeTooltip(item);
    }

    /// <summary>
    /// This function hides the tool tip.
    /// </summary>
    public void hideToolTip()
    {
        this.toolTip.SetActive(false);
    }

    /// <summary>
    /// This function makes changes to the tooltip unique to the panel it appears in.
    /// </summary>
    public virtual void customizeTooltip(Item item)
    {

    }
}
