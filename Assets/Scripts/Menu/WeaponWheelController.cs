using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponID;
    public PlayerController playerController;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            weaponWheelSelected = !weaponWheelSelected;
        }

        if (weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
        }

        switch(weaponID)
        {
            case 0:
                selectedItem.sprite = noImage;
                break;
            case 1:
                playerController.ChooseMagic("Fireball");
                break;
            case 2:
                playerController.ChooseMagic("IceSpike");
                break;
            case 3:
                playerController.ChooseMagic("Lightning");
                break;
            case 4:
                playerController.ChooseMagic("Shield");
                break;
            case 5:
                playerController.ChooseMagic("SmallHeal");
                break;
            case 6:
                playerController.ChooseMagic("BigHeal");
                break;
            case 7:
                playerController.ChooseMagic("Paralysis");
                break;
            case 8:
                playerController.ChooseMagic("Transmutation");
                break;
        }
    }
}
