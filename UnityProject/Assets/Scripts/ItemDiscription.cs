using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CurrentItem {None, LifeBerry, Rixile, NicirPlant, MindRemedy, PetriShroom, ReliefOintment,
                         TerraIdol, MagicIdol, key, DemonMask, MoonPearl, MarkofEtymology, AncientTablet, IDCard, LifeChime,
                         SpiritRich, PowerBand, Pendant, SpellBind, DeathBind }
public enum CurrentEquipment {None, MythrilSword, FlareSword, LightningSword, FrostSword, TerraSword, DenseArmor, HelixArmor, ApexArmor, MythicArmor, LegendaryArmor,
                               TrailBlazers, Hovers, Cinders, Storms, Medicals, RedJewel, BlueJewel, GreenJewel, YellowJewel, PurpleJewel}


[RequireComponent(typeof(Selectable))]
public class ItemDiscription : MonoBehaviour
{

    public AudioSource audioSrc;
    public AudioClip usedItemSfx;
    public AudioClip usedDemonMask;
    public float letterTime;
    string discriptMessage;
    string confirmMessage;
    public Text DiscriptMsgText;
    public Text ConfirmMsgText;
    public Image activeIcon;
    public Sprite[] itemIcons;
    public Sprite[] equipIcons;
    public CurrentItem currentItem;
    public CurrentEquipment currentEquipment;
    bool readyToClose;
    PauseGame unPause;
    int itemCounter = 0;

    bool resetConfirmText = false;
    bool resetDiscriptText = false;
    bool discriptionRunning;
    bool isPoisoned;
    bool isConfused;
    bool isParalyzed;
    bool isSilenced;
    bool isMagicIdol;
    bool isTerraIdol;
    bool isPaused;
    bool textActive;

    IEnumerator animateConfirm;
    IEnumerator animateDiscript;
    IEnumerator fadeImage;
    IEnumerator AnimateConfirmation()
    {
        if (itemCounter == 1)
        {
            foreach (char letter in confirmMessage.ToCharArray())
            {
                if (itemCounter == 2 || resetConfirmText)
                {
                    ConfirmMsgText.text = null;
                    break;
                }
                else if (!resetConfirmText)
                {
                    ConfirmMsgText.text += letter;
                    yield return 0;
                    yield return new WaitForSecondsRealtime(letterTime);
                }
            }
        }
        else if (itemCounter == 2)
        {
            foreach (char letter in confirmMessage.ToCharArray())
            {
                if (itemCounter == 1 || resetConfirmText)
                {

                    ConfirmMsgText.text = null;
                    break;
                }
                else if (!resetConfirmText)
                {

                    ConfirmMsgText.text += letter;
                    yield return 0;
                    yield return new WaitForSecondsRealtime(letterTime);
                }
            }
        }
    }
    IEnumerator AnimateDiscription()
    {
       if(DiscriptMsgText.text != null)
            DiscriptMsgText.text = null;
        foreach (char letter in discriptMessage.ToCharArray())
        {
            if (resetDiscriptText)
            {
                DiscriptMsgText.text = null;
                discriptionRunning = false;
                break;
            }
            else
            {
                DiscriptMsgText.text += letter;
                yield return 0;
                yield return new WaitForSecondsRealtime(letterTime);
            }

        }
    }
    IEnumerator FadeInIcon(float aTime, Image image)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, 1, t));
            image.GetComponent<Image>().color = newColor;

            if (t > 0.9f)
            {
                newColor = new Color(1, 1, 1, 1);
                image.GetComponent<Image>().color = newColor;
                break;
            }
            else
            {
                image.GetComponent<Image>().enabled = true;
            }
            yield return null;
        }
        
    }
    public void SelectDiscriptionEquipment(CurrentEquipment equip)
    {
        discriptMessage = null;
 
        switch (equip)
        {   
            //=======================================================Equipment=====================================//
            case CurrentEquipment.MythrilSword:
                {
                    activeIcon.sprite = equipIcons[0];
                    discriptMessage = "[Mythril Sword] - A hardened combat sword that never tarnishes, it was forged with Mythril making it incredibly balanced and light weight. ";
                    break;
                }
            case CurrentEquipment.FlareSword:
                {
                    activeIcon.sprite = equipIcons[1];
                    discriptMessage = "[Flare Sword] - A sword molded from rare meterorite fragments that never burn out. It can burn anything that touches.";
                    break;
                }
            case CurrentEquipment.LightningSword:
                {
                    activeIcon.sprite = equipIcons[2];
                    discriptMessage = "[Lightning Sword] - Crafted by the gods themselves, this sword emits a bolt of electricity which can paralyze a foe for a short time. ";
                    break;
                }
            case CurrentEquipment.FrostSword:
                {
                    activeIcon.sprite = equipIcons[3];
                    discriptMessage = "[Frost Sword] - A cryogenic magical sword with a blade made of pure ice that never thaws. Anything it touches can instantly freeze. ";
                    break;
                }
            case CurrentEquipment.TerraSword:
                {
                    activeIcon.sprite = equipIcons[4];
                    discriptMessage = "[Terra Sword] - An unknown powerful sword only whispered by legend, a swing of this blade creates a shockwave that causes the earth to shake. ";
                    break;
                }
            case CurrentEquipment.DenseArmor:
                {
                    activeIcon.sprite = equipIcons[5];
                    discriptMessage = "[Dense Armor] - Comfortable lightweight combat armor made with the finest steel and soft leather. ";
                    break;
                }
            case CurrentEquipment.HelixArmor:
                {
                    activeIcon.sprite = equipIcons[6];
                    discriptMessage = "[Helix Armor] - Perplexed magical armor with condensed sapphire, this type of armor resists half the damage recieved. ";
                    break;
                }
            case CurrentEquipment.ApexArmor:
                {
                    activeIcon.sprite = equipIcons[7];
                    discriptMessage = "[Apex Armor] - Crafted only for soldiers of royal blood, this elegant ruby armor withstands three times the damage. ";
                    break;
                }
            case CurrentEquipment.MythicArmor:
                {
                    activeIcon.sprite = equipIcons[8];
                    discriptMessage = "[Mythic Armor] - This Special mythical armor crafed with dragon scales, this armor resists four times the damage. ";
                    break;
                }
            case CurrentEquipment.LegendaryArmor:
                {
                    activeIcon.sprite = equipIcons[9];
                    discriptMessage = "[Legendary Armor] - Unknown... This armor is molded by a dark power and shields the wearer to only recieve five times the damage.";
                    break;
                }
            case CurrentEquipment.TrailBlazers:
                {
                    activeIcon.sprite = equipIcons[10];
                    discriptMessage = "[Trail Blazers] - Buckled leather boots comfortable for walking long distances. Wearing these are soft on the feet.";
                    break;
                }
            case CurrentEquipment.Hovers:
                {
                    activeIcon.sprite = equipIcons[11];
                    discriptMessage = "[Hovers] - Special boots crafted with powerful magic giving the ability to float. These boots are active when walking across spike floors";
                    break;
                }
            case CurrentEquipment.Cinders:
                {
                    activeIcon.sprite = equipIcons[12];
                    discriptMessage = "[Cinders] - Molded and chiseled from pure molten volcano rock. These boots are heavy on the feet but impervious when walking across lava.";
                    break;
                }
            case CurrentEquipment.Storms:
                {
                    activeIcon.sprite = equipIcons[13];
                    discriptMessage = "[Storms] - Advanced storm gear with auto adaption. These boots allow you to climb ice, walk through swamps and gives you the ability to swim.";
                    break;
                }
            case CurrentEquipment.Medicals:
                {
                    activeIcon.sprite = equipIcons[14];
                    discriptMessage = "[Medicals] - Super technological medical footware designed by a genius scientist, walking provides healing abilites.";
                    break;
                }
            case CurrentEquipment.RedJewel:
                {
                    activeIcon.sprite = equipIcons[15];
                    discriptMessage = "[Jewel of Power] - A crystal constructed of power magic. Harnessing its power, casts a shockwave that damages all surrounded enemies and objects.";
                    break;
                }
            case CurrentEquipment.BlueJewel:
                {
                    activeIcon.sprite = equipIcons[16];
                    discriptMessage = "[Jewel of Spirit] - A crystal constructed of spirituality magic. Harnessing its power, grants the ability to breath underwater and jump higher.";
                    break;
                }
            case CurrentEquipment.GreenJewel:
                {
                    activeIcon.sprite = equipIcons[17];
                    discriptMessage = "[Jewel of Time] - A crystal constructed of time magic. Harnessing its power of time, allows the user to decay time, slowing down fast moving obstacles";
                    break;
                }
            case CurrentEquipment.YellowJewel:
                {
                    activeIcon.sprite = equipIcons[18];
                    discriptMessage = "[Jewel of Light] - A crystal constructed of light magic. Harnessing its power of light, cycles ahead six  hours forward in a day or night";
                    break;
                }
            case CurrentEquipment.PurpleJewel:
                {
                    activeIcon.sprite = equipIcons[19];
                    discriptMessage = "[Jewel of Dark] - A crystal constructed of dark magic. Harnessing its power of darkness reveals the otherside, notifying of any secrets nearby and enemies are aethered with darkness.";
                    break;
                }
        }
    }
    public void SelectConfirmEquipment(CurrentEquipment equip)
    {
        ConfirmMsgText.GetComponent<Text>();
        ConfirmMsgText.text = "";
        confirmMessage = ConfirmMsgText.text;
        itemCounter += 1;
        if (itemCounter > 2)
        {
            itemCounter = 1;
        }
        switch (equip)
        {
            //============================================Equipment==================================================//
            case CurrentEquipment.MythrilSword:
                {
                    if (!SwordSystem.mythrilSwordEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Mythril Sword?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Mythril Sword...";
                            SwordSystem swordSys = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
                            swordSys.SelectSword(SwordActive.mythril);
                        }
                    }
                    else if (SwordSystem.mythrilSwordEnabled)
                    {
                        confirmMessage = "Mythril Sword already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.FlareSword:
                {
           
                    if (!SwordSystem.flareSwordEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Flare Sword?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Flare Sword...";
                            SwordSystem swordSys = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
                            swordSys.SelectSword(SwordActive.flare);
                        }
                    }
                    else if (SwordSystem.flareSwordEnabled)
                    {
                        confirmMessage = "Flare Sword already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.LightningSword:
                {
                   
                    if (!SwordSystem.lightningSwordEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Lightning Sword?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Lightning Sword...";
                            SwordSystem swordSys = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
                            swordSys.SelectSword(SwordActive.lightning);
                        }
                    }
                    else if (SwordSystem.lightningSwordEnabled)
                    {
                        confirmMessage = "Lightning Sword already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.FrostSword:
                {
                    
                    if (!SwordSystem.frostSwordEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Frost Sword?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Frost Sword...";
                            SwordSystem swordSys = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
                            swordSys.SelectSword(SwordActive.frost);
                        }
                    }
                    else if (SwordSystem.frostSwordEnabled)
                    {
                        confirmMessage = "Frost Sword already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.TerraSword:
                {
                    
                    if (!SwordSystem.terraSwordEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Terra Sword?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Terra Sword...";
                            SwordSystem swordSys = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
                            swordSys.SelectSword(SwordActive.terra);
                        }
                    }
                    else if (SwordSystem.terraSwordEnabled)
                    {
                        confirmMessage = "Terra Sword already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.DenseArmor:
                {
                   
                    if (!PlayerSystem.denseArmorEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Dense Armor?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Dense Armor...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectArmor(ArmorActive.dense);
                        }
                    }
                    else if (PlayerSystem.denseArmorEnabled)
                    {
                        confirmMessage = "Dense Armor already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.HelixArmor:
                {
                   
                    if (!PlayerSystem.helixArmorEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Helix Armor?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Helix Armor...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectArmor(ArmorActive.helix);
                        }
                    }
                    else if (PlayerSystem.helixArmorEnabled)
                    {
                        confirmMessage = "Helix Armor already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.ApexArmor:
                {
                    
                    if (!PlayerSystem.apexArmorEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Apex Armor?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Apex Armor...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectArmor(ArmorActive.apex);
                        }
                    }
                    else if (PlayerSystem.apexArmorEnabled)
                    {
                        confirmMessage = "Apex Armor already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.MythicArmor:
                {
                
                    if (!PlayerSystem.mythicArmorEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Mythic Armor?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Mythic Armor...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectArmor(ArmorActive.mythic);
                        }
                    }
                    else if (PlayerSystem.mythicArmorEnabled)
                    {
                        confirmMessage = "Mythic Armor already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.LegendaryArmor:
                {
                   
                    if (!PlayerSystem.legendaryArmorEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Legendary Armor?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Legendary Armor...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectArmor(ArmorActive.legendary);
                        }
                    }
                    else if (PlayerSystem.legendaryArmorEnabled)
                    {
                        confirmMessage = "Legendary Armor already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.TrailBlazers:
                {
                   
                    if (!PlayerSystem.trailBootEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Trail Blazers?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Trail Blazers...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectBoots(BootActive.trail);
                        }
                    }
                    else if (PlayerSystem.trailBootEnabled)
                    {
                        confirmMessage = "Trail Blazers already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.Hovers:
                {
                   
                    if (!PlayerSystem.hoverBootEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Hovers?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Hovers...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectBoots(BootActive.hover);
                        }
                    }
                    else if (PlayerSystem.hoverBootEnabled)
                    {
                        confirmMessage = "Hovers already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.Cinders:
                {
                   
                    if (!PlayerSystem.cinderBootEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Cinders?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Cinders...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectBoots(BootActive.cinder);
                        }
                    }
                    else if (PlayerSystem.cinderBootEnabled)
                    {
                        confirmMessage = "Cinders already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.Storms:
                {
                    
                    if (!PlayerSystem.stormBootEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Storms?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Storms...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectBoots(BootActive.storm);
                        }
                    }
                    else if (PlayerSystem.stormBootEnabled)
                    {
                        confirmMessage = "Storms already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.Medicals:
                {
                    
                    if (!PlayerSystem.medicalBootEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Medicals?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = "Equipped Medicals...";
                            PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                            playSys.SelectBoots(BootActive.medical);
                        }
                    }
                    else if (PlayerSystem.medicalBootEnabled)
                    {
                        confirmMessage = "Medicals already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.RedJewel:
                {
                    if (!JewelSystem.redJewelEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Jewel of Power?";
                        }
                        else if (itemCounter == 2)
                        {
                            if (!JewelSystem.isJewelActive)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = "Equipped Jewel of Power...";
                                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                                jewelSys.SelectJewel(JewelActive.red);
                            }
                            else
                                confirmMessage = "Jewel Power already in Use...";
                        }
                    }
                    else if (JewelSystem.redJewelEnabled)
                    {
                        confirmMessage = "Jewel already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.BlueJewel:
                {
                    if (!JewelSystem.blueJewelEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Jewel of Spirit?";
                        }
                        else if (itemCounter == 2)
                        {
                            if (!JewelSystem.isJewelActive)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = "Equipped Jewel of Spirit...";
                                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                                jewelSys.SelectJewel(JewelActive.blue);
                            }
                            else
                                confirmMessage = "Jewel Power already in Use...";
                        }
                    }
                    else if (JewelSystem.blueJewelEnabled)
                    {
                        confirmMessage = "Jewel already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.GreenJewel:
                {
                    if (!JewelSystem.greenJewelEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Jewel of Time?";
                        }
                        else if (itemCounter == 2)
                        {
                            if (!JewelSystem.isJewelActive)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = "Equipped Jewel of Time...";
                                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                                jewelSys.SelectJewel(JewelActive.green);
                            }
                            else
                                confirmMessage = "Jewel Power already in Use...";
                        }
                    }
                    else if (JewelSystem.greenJewelEnabled)
                    {
                        confirmMessage = "Jewel already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.YellowJewel:
                {
                    if (!JewelSystem.yellowJewelEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Jewel of Light?";
                        }
                        else if (itemCounter == 2)
                        {
                            if (!JewelSystem.isJewelActive)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = "Equipped Jewel of Light...";
                                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                                jewelSys.SelectJewel(JewelActive.yellow);
                            }
                            else
                                confirmMessage = "Jewel Power already in Use...";
                        }
                    }
                    else if (JewelSystem.yellowJewelEnabled)
                    {
                        confirmMessage = "Jewel already Equipped...";
                    }
                    break;
                }
            case CurrentEquipment.PurpleJewel:
                {
                    if (!JewelSystem.purpleJewelEnabled)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to Equip Jewel of Dark?";
                        }
                        else if (itemCounter == 2)
                        {
                            if (!JewelSystem.isJewelActive)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = "Equipped Jewel of Dark...";
                                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                                jewelSys.SelectJewel(JewelActive.purple);
                            }
                            else
                                confirmMessage = "Jewel Power already in Use...";
                        }
                    }
                    else if (JewelSystem.purpleJewelEnabled)
                    {
                        confirmMessage = "Jewel already Equipped...";
                    }
                    break;
                }
        }
        if (animateConfirm != null)
            StopCoroutine(animateConfirm);
        animateConfirm = AnimateConfirmation();
        StartCoroutine(animateConfirm);
    }
    public void SelectDiscriptionItem(CurrentItem item)
    {
        discriptMessage = null;
        switch (item)
        {
           
            case CurrentItem.LifeBerry:
                {
                  
                    if (!ItemSystem.lifeBerryEmpty)
                    {
                        activeIcon.sprite = itemIcons[0];
                        discriptMessage = "[Life Berry] - Magical berries that can be mixed together to make a life reviving potion, revives and heals half of the players health. ";
                    }
                    else if (ItemSystem.lifeBerryEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }
                    break;
                }
            case CurrentItem.Rixile:
                {
                   
                    if (!ItemSystem.rixileEmpty)
                    {
                        activeIcon.sprite = itemIcons[1];
                        discriptMessage = "[Rixile] - Brewed with the finest life berries and medicine. This strong potion fills players health. ";
                    }
                    else if (ItemSystem.rixileEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }

                    break;
                }
            case CurrentItem.NicirPlant:
                {
                    if (!ItemSystem.nicirPlantEmpty)
                    {
                        activeIcon.sprite = itemIcons[2];
                        discriptMessage = "[Nicir Plant] - One of the Most Poisonous plants found in the wild. It can be brewed to make a powerful antidote. ";
                    }
                    else if (ItemSystem.nicirPlantEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }
                    break;
                }
            case CurrentItem.MindRemedy:
                {
                    if (!ItemSystem.mindRemedyEmpty)
                    {
                        activeIcon.sprite = itemIcons[3];
                        discriptMessage = "[Mind Remedy] - A Mysterious Potion that repairs a twixed mind. This item removes confusion.";
                    }
                    else if (ItemSystem.mindRemedyEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }
                    break;
                }
            case CurrentItem.PetriShroom:
                {
                    if (!ItemSystem.petriShroomEmpty)
                    {
                        activeIcon.sprite = itemIcons[4];
                        discriptMessage = "[Petri-Shroom] - A rare mushroom picked in a damp withered forest and mashed into a medicine. It has the power to cure paralysis.";
                    }
                    else if (ItemSystem.petriShroomEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }
                    break;
                }
            case CurrentItem.ReliefOintment:
                {
                    if (!ItemSystem.reliefOintmentEmpty)
                    {
                        activeIcon.sprite = itemIcons[5];
                        discriptMessage = "[Relief Ointment] - A special medicinal ointment that relieves muscle stigma. Using this item removes silence.";
                    }
                    else if (ItemSystem.reliefOintmentEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }

                    break;
                }
            case CurrentItem.TerraIdol:
                {
                    if (!ItemSystem.terraIdolEmpty)
                    {
                        activeIcon.sprite = itemIcons[6];
                        discriptMessage = "[Terra Idol] - A spiritual tablet said to have been crafted by a terraformed goddess, believe to have caused catastrophic earth quakes.";
                    }
                    else if (ItemSystem.terraIdolEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }

                    break;
                }
            case CurrentItem.MagicIdol:
                {
                    if (!ItemSystem.magicIdolEmpty)
                    {
                        activeIcon.sprite = itemIcons[7];
                        discriptMessage = "[Magic Idol] - A magical tablet crafted by a mystical black smith, believed to give its user outstanding sword abilites.";
                    }
                    else if (ItemSystem.magicIdolEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }

                    break;
                }
            case CurrentItem.key:
                {
                    if (!ItemSystem.keyEmpty)
                    {
                        activeIcon.sprite = itemIcons[8];
                        discriptMessage = "[Key] - A rusty withered old age key, used to open a majority of locked doors in dungeons, temples and caves";
                    }
                    else if (ItemSystem.keyEmpty)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }

                    break;
                }
            case CurrentItem.DemonMask:
                {
                    if (ItemSystem.demonMaskPickedUp)
                    {
                        activeIcon.sprite = itemIcons[9];
                        discriptMessage = "[Demon Mask] - Undead spirits underlie deep inside this mask, it taints the soul of the wearer to see things that arent really there.";
                    }
                    else if (!ItemSystem.demonMaskPickedUp)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }

                    break;
                }
            case CurrentItem.MoonPearl:
                {
                    if (ItemSystem.moonPearlPickedUp)
                    {
                        activeIcon.sprite = itemIcons[10];
                        discriptMessage = "[Moon Pearl] - A tiny mystical pearl wrapped in a magical blue velvet. During night, the stone shines bright.";
                    }
                    else if (!ItemSystem.moonPearlPickedUp)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }

                    break;
                }
            case CurrentItem.MarkofEtymology:
                {
                    if (ItemSystem.markOfEtymologyPickedUp)
                    {
                        activeIcon.sprite = itemIcons[11];
                        discriptMessage = "[Mark of Etymology] - A green emerald amulet with concealed magic contained in the crystalized gem. Used to decypher encripted text books.";
                    }
                    else if (!ItemSystem.markOfEtymologyPickedUp)
                    {
                        Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                        activeIcon.color = blank;
                        discriptMessage = "";
                    }

                    break;
                }
        }


    }
    public void SelectConfirmItem(CurrentItem item)
    {
        ConfirmMsgText.GetComponent<Text>();
        ConfirmMsgText.text = "";
        confirmMessage = ConfirmMsgText.text;
        itemCounter += 1;
        if (itemCounter > 2)
        {
            itemCounter = 1;
        }
        switch (item)
        {
     
            //================================================Items==================================================//
            case CurrentItem.LifeBerry:
                {
                   
                    if (!ItemSystem.lifeBerryEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Life Berry?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = " Used Life Berry...";
                            ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                            itemSys.UseItem(ItemSystem.Item.LifeBerry, 1);
                        }
                    }
                    break;
                }
            case CurrentItem.Rixile:
                {
                   
                    if (!ItemSystem.rixileEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Rixile?";
                        }
                        else if (itemCounter == 2)
                        {
                            audioSrc.pitch = 1;
                            audioSrc.volume = 1;
                            audioSrc.PlayOneShot(usedItemSfx);
                            confirmMessage = " Used Rixile...";
                            ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                            itemSys.UseItem(ItemSystem.Item.Rixile, 1);

                        }
                    }
                    break;
                }
            case CurrentItem.NicirPlant:
                {
                
                    if (!ItemSystem.nicirPlantEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Nicir Plant?";
                        }
                        else if (itemCounter == 2)
                        {
                            isPoisoned = PlayerSystem.isPoisoned;
                            if (isPoisoned)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = " Used Nicir Plant...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.NicirPlant, 1);

                            }
                            else
                                confirmMessage = " You are not poisoned...";
                        }
                    }
                    break;
                }
            case CurrentItem.MindRemedy:
                {
                   
                    if (!ItemSystem.mindRemedyEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Mind Remedy?";
                        }
                        else if (itemCounter == 2)
                        {
                            isConfused = PlayerSystem.isConfused;
                            if (isConfused)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = " Used Mind Potion...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.MindRemedy, 1);

                            }
                            else
                                confirmMessage = " You are not confused...";
                        }
                    }
                    break;
                }
            case CurrentItem.PetriShroom:
                {
                   
                    if (!ItemSystem.petriShroomEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Petri-Shroom?";
                        }
                        else if (itemCounter == 2)
                        {
                            isParalyzed = PlayerSystem.isParalyzed;
                            if (isParalyzed)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = " Used Petri-Shroom...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.PetriShroom, 1);
                            }
                            else
                                confirmMessage = " You are not paralyzed...";
                        }
                    }
                    break;
                }
            case CurrentItem.ReliefOintment:
                {
                    
                    if (!ItemSystem.reliefOintmentEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Relief Ointment?";
                        }
                        else if (itemCounter == 2)
                        {
                            isSilenced = PlayerSystem.isSilenced;
                            if (isSilenced)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = " Used Relief Ointment...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.ReliefOintment, 1);
                            }
                            else
                                confirmMessage = " You are not silenced...";
                        }
                    }
                    break;
                }
            case CurrentItem.TerraIdol:
                {
                    
                    if (!ItemSystem.terraIdolEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Terra Idol?";
                        }
                        else if (itemCounter == 2)
                        {
                            isTerraIdol = SwordSystem.isTerraIdolActive;
                            if (!isTerraIdol)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = " Used Terra Idol...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.TerraIdol, 1);
                            }
                            else
                                confirmMessage = " Item already in use...";
                        }
                       
                    }
                    break;
                }
            case CurrentItem.MagicIdol:
                {
                   
                    if (!ItemSystem.magicIdolEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Magic Idol?";
                        }
                        else if (itemCounter == 2)
                        {
                            isMagicIdol = SwordSystem.isMagicIdolActive;
                            if (!isMagicIdol)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = " Used Magic Idol...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.MagicIdol, 1);
                            }
                            else
                                confirmMessage = " Item already in use...";
                        }
                       

                    }
                    break;
                }
            case CurrentItem.key:
                {
                    
                    if (!ItemSystem.keyEmpty)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = "Would you like to use Key?";
                        }
                        else if (itemCounter == 2)
                        {
                         
                            if (ItemSystem.inDoorTerritory && ItemSystem.isDoorLocked)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = " Used Key...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.key, 1);
                            }
                            else
                                confirmMessage = " Item cannot be used...";
                        }
                    }
                    break;
                }
            case CurrentItem.DemonMask:
                {
                 
                    if (ItemSystem.demonMaskPickedUp)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = " Would you like to use Demon Mask?";
                        }
                        else if (itemCounter == 2)
                        {
                          
                            if (!ItemSystem.demonMaskEnabled)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedDemonMask);
                                confirmMessage = " Put on Demon Mask...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.DemonMask, 0);
                            }
                            else
                            {
                                confirmMessage = " Took off Demon Mask...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.DemonMask, 0);
                            }
                               
                        }
                    }
                    break;
                }
            case CurrentItem.MoonPearl:
                {

                    if (ItemSystem.moonPearlPickedUp)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = "Would you like to use Moon Pearl?";
                        }
                        else if (itemCounter == 2)
                        {

                            if (ItemSystem.inDoorTerritory && ItemSystem.isDoorLocked && TimeSystem.isNight)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedItemSfx);
                                confirmMessage = " The stone shined brightly...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.MoonPearl, 0);
                            }
                            else
                                confirmMessage = "Nothing Happened...";
                        }
                    }
                    break;
                }
            case CurrentItem.MarkofEtymology:
                {

                    if (ItemSystem.markOfEtymologyPickedUp)
                    {
                        if (itemCounter == 1)
                        {
                            confirmMessage = "Would you like to use Mark of Etymology?";
                        }
                        else if (itemCounter == 2)
                        {

                            if (!ItemSystem.markOfEtymologyEnabled)
                            {
                                audioSrc.pitch = 1;
                                audioSrc.volume = 1;
                                audioSrc.PlayOneShot(usedDemonMask);
                                confirmMessage = " Mark of Etymology glowed heavily...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.MarkofEtymology, 0);
                            }
                            else
                            {
                                confirmMessage = " Mark of Etymology became dull...";
                                ItemSystem itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
                                itemSys.UseItem(ItemSystem.Item.MarkofEtymology, 0);
                            }
                        }
                    }
                    break;
                }
        }
        if (animateConfirm != null)
            StopCoroutine(animateConfirm);
        animateConfirm = AnimateConfirmation();
        StartCoroutine(animateConfirm);
    }
    public void ActivateDiscription(int num)
    {
        Button thisButton = GetComponent<Button>();
        if (thisButton.interactable) {
            //Item//
            if (num == 1 && !discriptionRunning)
            {

                if (animateDiscript != null)
                    StopCoroutine(animateDiscript);
                if (fadeImage != null)
                    StopCoroutine(fadeImage);

                DiscriptMsgText.text = null;
                resetDiscriptText = false;

                SelectDiscriptionItem(currentItem);
                
                animateDiscript = AnimateDiscription();
                StartCoroutine(animateDiscript);
              
                fadeImage = FadeInIcon(0.5f, activeIcon);
                StartCoroutine(fadeImage);

                discriptionRunning = true;

            }
            //Equipment//
            else if (num == 2 && !discriptionRunning)
            {
                if (animateDiscript != null)
                    StopCoroutine(animateDiscript);
                if (fadeImage != null)
                    StopCoroutine(fadeImage);

                DiscriptMsgText.text = null;
                resetDiscriptText = false;

                SelectDiscriptionEquipment(currentEquipment);
               
                animateDiscript = AnimateDiscription();
                StartCoroutine(animateDiscript);

                fadeImage = FadeInIcon(0.5f, activeIcon);
                StartCoroutine(fadeImage);

                discriptionRunning = true;
            }

            //None//
            else
            {
                if (fadeImage != null)
                    StopCoroutine(fadeImage);
                if (animateDiscript != null)
                    StopCoroutine(animateDiscript);
                if (animateConfirm != null)
                    StopCoroutine(animateConfirm);
                discriptionRunning = false;
                itemCounter = 0;
                resetDiscriptText = true;
                ConfirmMsgText.text = null;
                confirmMessage = null;
                discriptMessage = null;
                DiscriptMsgText.text = null;
                Color blank = new Color(activeIcon.color.r, activeIcon.color.g, activeIcon.color.b, 0);
                activeIcon.color = blank;
            }
        }
    }
    public void ActivateItemConfirmationUse()
    {
        SelectConfirmItem(currentItem);
    }
    public void ActivateEquipmentConfirmationUse()
    {
        SelectConfirmEquipment(currentEquipment);
    }
}

