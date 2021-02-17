using UnityEngine.UI;
using UnityEngine;
using System.Collections;
public class SelectionalSystem : MonoBehaviour
{
    PlayerSystem playSys;
    SwordSystem swordSys;
    JewelSystem jewelSys;
    CharacterSystem characterCtrl;
    public AudioSource audioSrc;
    public AudioClip selectSfx;
    public AudioClip selectSfx2;
    public AudioClip selectSfx3;
    public float fadeSpeed;
    public GameObject selection;
    public Image[] icons;
    IEnumerator routine;
    bool active = false;
    public static bool isSelecting;

    [Header("SwordIcons")]
    public Image selectionMythrilIU;
    public Image selectionFlareIU;
    public Image selectionLightningIU;
    public Image selectionFrostIU;
    public Image selectionTerraIU;
    [Space]
    [Header("ArmorIcons")]
    public Image selectionDenseIU;
    public Image selectionHelixIU;
    public Image selectionApexIU;
    public Image selectionMythicIU;
    public Image selectionLegendaryIU;
    [Space]
    [Header("BootIcons")]
    public Image selectionTrailIU;
    public Image selectionHoverIU;
    public Image selectionCinderIU;
    public Image selectionStormIU;
    public Image selectionMedicalIU;

    [Space]
    [Header("JewelIcons")]
    public Image selectionRedJewelIU;
    public Image selectionBlueJewelIU;
    public Image selectionGreenJewelIU;
    public Image selectionYellowJewelIU;
    public Image selectionPurpleJewelIU;

    public bool isUpK;
    public bool isDownK;
    public bool isLeftK;
    public bool isRightK;

    public bool isUpC;
    public bool isDownC;
    public bool isLeftC;
    public bool isRightC;



    private void Start()
    {
        selection.SetActive(false);
        FadeIcons(false);
        active = false;
        icons[0].enabled = false;
    }
    void Update()
    {

        if (Input.GetButtonDown("Select") && !CharacterSystem.isClimbing && !CharacterSystem.isCarrying && !active && !SceneSystem.isDisabled && !PauseGame.isPaused  && !DialogueSystem.isDialogueActive && !PlayerSystem.isDead && !ShopSystem.isShop && !SwordSystem.isAttacking)
        {
            characterCtrl = GetComponent<CharacterSystem>();
            
            characterCtrl.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
            audioSrc.PlayOneShot(selectSfx);
            SetIcons();
            selection.SetActive(true);

            FadeIcons(true);
            active = true;
            isSelecting = true;
        }
        else if(Input.GetButtonUp("Select"))
        {
            if(!CharacterSystem.isClimbing && !CharacterSystem.isCarrying && !SceneSystem.isDisabled && !PauseGame.isPaused && !DialogueSystem.isDialogueActive && !PlayerSystem.isDead && !ShopSystem.isShop && !SwordSystem.isAttacking)
                audioSrc.PlayOneShot(selectSfx2);
            FadeIcons(false);
            active = false;
            isSelecting = false;
        }
        if (isSelecting)
        {
            SetIcons();
            if (Input.GetAxis("DVertical") == 1 && !isUpC)
            {
                audioSrc.PlayOneShot(selectSfx3);
                SelectEquipment(Equipment.Sword);
                isUpC = true;
            }
            else if (Input.GetAxis("DVertical") == 0)
                isUpC = false;

            if (Input.GetAxis("DVertical") == -1 && !isDownC && !JewelSystem.isJewelActive)
            {
                audioSrc.PlayOneShot(selectSfx3);
                SelectEquipment(Equipment.jewels);
                isDownC = true;
            }
            else if (Input.GetAxis("DVertical") == 0)
                isDownC = false;

            if (Input.GetAxis("DHorizontal") == 1 && !isRightC)
            {
                audioSrc.PlayOneShot(selectSfx3);
                SelectEquipment(Equipment.boots);
                isRightC = true;
            }
            else if (Input.GetAxis("DHorizontal") == 0)
                isRightC = false;

            if (Input.GetAxis("DHorizontal") == -1 && !isLeftC)
            {
                audioSrc.PlayOneShot(selectSfx3);
                SelectEquipment(Equipment.armor);
                isLeftC = true;
            }
            else if (Input.GetAxis("DHorizontal") == 0)
                isLeftC = false;

            ///////////////////////////////////Keyboard///////////////////////////////////////////////////////////

            //=====================================SelectUp=================================================//
            if (Input.GetKeyDown(KeyCode.W) && !isUpK || Input.GetKeyDown(KeyCode.UpArrow) && !isUpK)
            {
                audioSrc.PlayOneShot(selectSfx3);
                SelectEquipment(Equipment.Sword);
                isUpK = true;
            }
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                isUpK = false;
            //=====================================SelectDown=================================================//

            if (Input.GetKeyDown(KeyCode.S) && !isDownK && !JewelSystem.isJewelActive || Input.GetKeyDown(KeyCode.DownArrow) && !isDownK && !JewelSystem.isJewelActive)
            {
                audioSrc.PlayOneShot(selectSfx3);
                SelectEquipment(Equipment.jewels);
                isDownK = true;
            }
            else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                isDownK = false;
            //=====================================SelectLeft=================================================//
            if (Input.GetKeyDown(KeyCode.A) && !isLeftK || Input.GetKeyDown(KeyCode.LeftArrow) && !isLeftK)
            {
                audioSrc.PlayOneShot(selectSfx3);
                SelectEquipment(Equipment.armor);
                isLeftK = true;
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
                isLeftK = false;
            //=====================================SelectRight=================================================//
            if (Input.GetKeyDown(KeyCode.D) && !isRightK || Input.GetKeyDown(KeyCode.RightArrow) && !isRightK)
            {
                audioSrc.PlayOneShot(selectSfx3);
                SelectEquipment(Equipment.boots);
                isRightK = true;
            }
            else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                isRightK = false;
        }
    }
    public void FadeIcons(bool In)
    {
            if (In)
            {
                if (routine != null)
                    StopCoroutine(routine);
                routine = AnimateImageFade(fadeSpeed, true);
                StartCoroutine(routine);
            }
            else
            {
                if (routine != null)
                    StopCoroutine(routine);
                routine = AnimateImageFade(fadeSpeed, false);
                StartCoroutine(routine);
            }
    }
    IEnumerator AnimateImageFade(float aTime, bool fadeIn)
    {
        for (int f = 0; f < 9; f++)
        {

            float alpha = icons[f].color.a;
            float aValue;
            if (fadeIn)
                aValue = 1.0f;
            else
                aValue = 0.0f;
            for (float t = 0.0f; t < 1.0f; t += Time.unscaledDeltaTime / aTime)
            {
                Color newColor = new Color(icons[f].color.r, icons[f].color.g, icons[f].color.b, Mathf.Lerp(alpha, aValue, t));
                icons[f].color = newColor;
                if (t >= 0.8)
                {
                    if (fadeIn)
                        newColor = new Color(icons[f].color.r, icons[f].color.g, icons[f].color.b, 1);
                    else
                    {
                        newColor = new Color(icons[f].color.r, icons[f].color.g, icons[f].color.b, 0);
               
                        if (f == 1)
                            selection.SetActive(false);
                        else if (f == 2)
                            selection.SetActive(false);
                        else if (f == 3)
                            selection.SetActive(false);
                        else if (f == 4)
                            selection.SetActive(false);
                        else if (f == 5)
                            selection.SetActive(false);
                        else if (f == 6)
                            selection.SetActive(false);
                        else if (f == 7)
                            selection.SetActive(false);
                        else if (f == 8)
                            selection.SetActive(false);
                    }
                    icons[f].color = newColor;

                }
                yield return null;
            }
        }
    }
    public enum Equipment { Sword, armor, boots, jewels}
    public void SelectEquipment(Equipment type)
    {
        switch (type)
        {
            case Equipment.Sword:
                {
                    icons[0].enabled = true;
                    swordSys = GetComponent<SwordSystem>();
                    if (SwordSystem.mythrilSwordEnabled)
                    {
                        if (SwordSystem.flareSwordPickedUp)
                            swordSys.SelectSword(SwordActive.flare);
                        else if (SwordSystem.lightningSwordPickedUp)
                            swordSys.SelectSword(SwordActive.lightning);
                        else if (SwordSystem.frostSwordPickedUp)
                            swordSys.SelectSword(SwordActive.frost);
                        else if (SwordSystem.terraSwordPickedUp)
                            swordSys.SelectSword(SwordActive.terra);
                        else
                            swordSys.SelectSword(SwordActive.mythril);
                    }

                    else if (SwordSystem.flareSwordEnabled)
                    {
                        if (SwordSystem.lightningSwordPickedUp)
                            swordSys.SelectSword(SwordActive.lightning);
                        else if (SwordSystem.frostSwordPickedUp)
                            swordSys.SelectSword(SwordActive.frost);
                        else if (SwordSystem.terraSwordPickedUp)
                            swordSys.SelectSword(SwordActive.terra);
                        else
                            swordSys.SelectSword(SwordActive.mythril);
                    }
                    else if (SwordSystem.lightningSwordEnabled)
                    {
                        if (SwordSystem.frostSwordPickedUp)
                            swordSys.SelectSword(SwordActive.frost);
                        else if (SwordSystem.terraSwordPickedUp)
                            swordSys.SelectSword(SwordActive.terra);
                        else
                            swordSys.SelectSword(SwordActive.mythril);
                    }
                    else if (SwordSystem.frostSwordEnabled)
                    {
                        if (SwordSystem.terraSwordPickedUp)
                            swordSys.SelectSword(SwordActive.terra);
                        else
                            swordSys.SelectSword(SwordActive.mythril);
                    }
                    else if (SwordSystem.terraSwordEnabled)
                        swordSys.SelectSword(SwordActive.mythril);
                    break;
                }
            case Equipment.armor:
                {
                    icons[3].enabled = true;
                    playSys = GetComponent<PlayerSystem>();
                    if (PlayerSystem.denseArmorEnabled)
                    {
                        if (PlayerSystem.helixArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.helix);
                        else if (PlayerSystem.apexArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.apex);
                        else if (PlayerSystem.mythicArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.mythic);
                        else if (PlayerSystem.legendaryArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.legendary);
                        else
                            playSys.SelectArmor(ArmorActive.dense);
                    }
                    else if (PlayerSystem.helixArmorEnabled)
                    {
                        if (PlayerSystem.apexArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.apex);
                        else if (PlayerSystem.mythicArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.mythic);
                        else if (PlayerSystem.legendaryArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.legendary);
                        else
                            playSys.SelectArmor(ArmorActive.dense);
                    }
                    else if (PlayerSystem.apexArmorEnabled)
                    {
                        if (PlayerSystem.mythicArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.mythic);
                        else if (PlayerSystem.legendaryArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.legendary);
                        else
                            playSys.SelectArmor(ArmorActive.dense);
                    }
                    else if (PlayerSystem.mythicArmorEnabled)
                    {
                        if (PlayerSystem.legendaryArmorPickedUp)
                            playSys.SelectArmor(ArmorActive.legendary);
                        else
                            playSys.SelectArmor(ArmorActive.dense);
                    }
                    else if (PlayerSystem.legendaryArmorEnabled)
                        playSys.SelectArmor(ArmorActive.dense);
                    break;
                }
            case Equipment.boots:
                {
                    icons[1].enabled = true;
                    playSys = GetComponent<PlayerSystem>();
                    if (PlayerSystem.trailBootEnabled)
                    {
                        if (PlayerSystem.hoverBootPickedUp)
                            playSys.SelectBoots(BootActive.hover);
                        else if (PlayerSystem.cinderBootPickedUp)
                            playSys.SelectBoots(BootActive.cinder);
                        else if (PlayerSystem.stormBootPickedUp)
                            playSys.SelectBoots(BootActive.storm);
                        else if (PlayerSystem.medicalBootPickedUp)
                            playSys.SelectBoots(BootActive.medical);
                        else
                            playSys.SelectBoots(BootActive.trail);
                    }
                    else if (PlayerSystem.hoverBootEnabled)
                    {
                        if (PlayerSystem.cinderBootPickedUp)
                            playSys.SelectBoots(BootActive.cinder);
                        else if (PlayerSystem.stormBootPickedUp)
                            playSys.SelectBoots(BootActive.storm);
                        else if (PlayerSystem.medicalBootPickedUp)
                            playSys.SelectBoots(BootActive.medical);
                        else
                            playSys.SelectBoots(BootActive.trail);
                    }
                    else if (PlayerSystem.cinderBootEnabled)
                    {
                        if (PlayerSystem.stormBootPickedUp)
                            playSys.SelectBoots(BootActive.storm);
                        else if (PlayerSystem.medicalBootPickedUp)
                            playSys.SelectBoots(BootActive.medical);
                        else
                            playSys.SelectBoots(BootActive.trail);
                    }
                    else if (PlayerSystem.stormBootEnabled)
                    {
                        if (PlayerSystem.medicalBootPickedUp)
                            playSys.SelectBoots(BootActive.medical);
                        else
                            playSys.SelectBoots(BootActive.trail);
                    }
                    else if (PlayerSystem.medicalBootEnabled)
                        playSys.SelectBoots(BootActive.trail);
                    break;
                }
            case Equipment.jewels:
                {
                    jewelSys = GetComponent<JewelSystem>();
                    if (JewelSystem.redJewelEnabled)
                    {
                        icons[2].enabled = true;
                        if (JewelSystem.blueJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.blue);
                        else if (JewelSystem.greenJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.green);
                        else if (JewelSystem.yellowJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.yellow);
                        else if (JewelSystem.purpleJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.purple);
                        else
                            jewelSys.SelectJewel(JewelActive.red);
                    }
                    else if (JewelSystem.blueJewelEnabled)
                    {
                        icons[2].enabled = true;
                        if (JewelSystem.greenJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.green);
                        else if (JewelSystem.yellowJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.yellow);
                        else if (JewelSystem.purpleJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.purple);
                        else if (JewelSystem.redJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.red);
                        else
                            jewelSys.SelectJewel(JewelActive.blue);
                    }
                    else if (JewelSystem.greenJewelEnabled)
                    {
                        icons[2].enabled = true;
                        if (JewelSystem.yellowJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.yellow);
                        else if (JewelSystem.purpleJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.purple);
                        else if (JewelSystem.redJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.red);
                        else if (JewelSystem.blueJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.blue);
                        else
                            jewelSys.SelectJewel(JewelActive.green);
                    }
                    else if (JewelSystem.yellowJewelEnabled)
                    {
                        icons[2].enabled = true;
                        if (JewelSystem.purpleJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.purple);
                        else if (JewelSystem.redJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.red);
                        else if (JewelSystem.blueJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.blue);
                        else if (JewelSystem.greenJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.green);
                        else
                            jewelSys.SelectJewel(JewelActive.yellow);
                    }
                    else if (JewelSystem.purpleJewelEnabled)
                    {
                        icons[2].enabled = true;
                        if (JewelSystem.redJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.red);
                        else if (JewelSystem.blueJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.blue);
                        else if (JewelSystem.greenJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.green);
                        else if (JewelSystem.yellowJewelPickedUp)
                            jewelSys.SelectJewel(JewelActive.yellow);
                        else
                            jewelSys.SelectJewel(JewelActive.purple);
                    }
                    else
                        icons[2].enabled = false;
                    break;
                }
        }
    }
    public void SetIcons()
    {
        //=========================================================Swords==============================================//
        if (SwordSystem.mythrilSwordEnabled)
        {
            selectionTerraIU.enabled = false;
            selectionFrostIU.enabled = false;
            selectionLightningIU.enabled = false;
            selectionFlareIU.enabled = false;
            icons[0].sprite = selectionMythrilIU.sprite;
            icons[0].enabled = true;
        }
        else if (SwordSystem.flareSwordEnabled)
        {
            selectionMythrilIU.enabled = false;
            selectionTerraIU.enabled = false;
            selectionFrostIU.enabled = false;
            selectionLightningIU.enabled = false;
            icons[0].sprite = selectionFlareIU.sprite;
            icons[0].enabled = true;
        }
        else if (SwordSystem.lightningSwordEnabled)
        {
            selectionMythrilIU.enabled = false;
            selectionTerraIU.enabled = false;
            selectionFrostIU.enabled = false;
            selectionFlareIU.enabled = false;
            icons[0].sprite = selectionLightningIU.sprite;
            icons[0].enabled = true;
        }
        else if (SwordSystem.frostSwordEnabled)
        {
            selectionMythrilIU.enabled = false;
            selectionTerraIU.enabled = false;
            selectionLightningIU.enabled = false;
            selectionFlareIU.enabled = false;
            icons[0].sprite = selectionFrostIU.sprite;
            icons[0].enabled = true;
        }
        else if (SwordSystem.terraSwordEnabled)
        {
            selectionMythrilIU.enabled = false;
            selectionFrostIU.enabled = false;
            selectionLightningIU.enabled = false;
            selectionFlareIU.enabled = false;
            icons[0].sprite = selectionTerraIU.sprite;
            icons[0].enabled = true;
        }
        else
        {
            icons[0].enabled = false;
        }
        //=========================================================Armor==============================================//
        if (PlayerSystem.denseArmorEnabled)
        {
            selectionHelixIU.enabled = false;
            selectionApexIU.enabled = false;
            selectionMythicIU.enabled = false;
            selectionLegendaryIU.enabled = false;
            icons[3].sprite = selectionDenseIU.sprite;
            icons[3].enabled = true;
        }
        else if (PlayerSystem.helixArmorEnabled)
        {
            selectionDenseIU.enabled = false;
            selectionApexIU.enabled = false;
            selectionMythicIU.enabled = false;
            selectionLegendaryIU.enabled = false;
            icons[3].sprite = selectionHelixIU.sprite;
            icons[3].enabled = true;
        }
        else if (PlayerSystem.apexArmorEnabled)
        {
            selectionDenseIU.enabled = false;
            selectionHelixIU.enabled = false;
            selectionMythicIU.enabled = false;
            selectionLegendaryIU.enabled = false;
            icons[3].sprite = selectionApexIU.sprite;
            icons[3].enabled = true;
        }
        else if (PlayerSystem.mythicArmorEnabled)
        {
            selectionDenseIU.enabled = false;
            selectionHelixIU.enabled = false;
            selectionApexIU.enabled = false;
            selectionLegendaryIU.enabled = false;
            icons[3].sprite = selectionMythicIU.sprite;
            icons[3].enabled = true;
        }
        else if (PlayerSystem.legendaryArmorEnabled)
        {
            selectionDenseIU.enabled = false;
            selectionHelixIU.enabled = false;
            selectionApexIU.enabled = false;
            selectionMythicIU.enabled = false;
            icons[3].sprite = selectionLegendaryIU.sprite;
            icons[3].enabled = true;
        }
        else
        {
            icons[3].enabled = false;
        }
        //=========================================================Boots==============================================//
        if (PlayerSystem.trailBootEnabled)
        {
            selectionHoverIU.enabled = false;
            selectionCinderIU.enabled = false;
            selectionStormIU.enabled = false;
            selectionMedicalIU.enabled = false;
            icons[1].sprite = selectionTrailIU.sprite;
            icons[1].enabled = true;
        }
        else if (PlayerSystem.hoverBootEnabled)
        {
            selectionTrailIU.enabled = false;
            selectionCinderIU.enabled = false;
            selectionStormIU.enabled = false;
            selectionMedicalIU.enabled = false;
            icons[1].sprite = selectionHoverIU.sprite;
            icons[1].enabled = true;
        }
        else if (PlayerSystem.cinderBootEnabled)
        {
            selectionTrailIU.enabled = false;
            selectionHoverIU.enabled = false;
            selectionStormIU.enabled = false;
            selectionMedicalIU.enabled = false;
            icons[1].sprite = selectionCinderIU.sprite;
            icons[1].enabled = true;
        }
        else if (PlayerSystem.stormBootEnabled)
        {
            selectionTrailIU.enabled = false;
            selectionHoverIU.enabled = false;
            selectionCinderIU.enabled = false;
            selectionMedicalIU.enabled = false;
            icons[1].sprite = selectionStormIU.sprite;
            icons[1].enabled = true;
        }
        else if (PlayerSystem.medicalBootEnabled)
        {
            selectionTrailIU.enabled = false;
            selectionHoverIU.enabled = false;
            selectionCinderIU.enabled = false;
            selectionStormIU.enabled = false;
            icons[1].sprite = selectionMedicalIU.sprite;
            icons[1].enabled = true;
        }
        else
        {
            icons[1].enabled = false;
        }
        //=========================================================Jewels==============================================//
        if (JewelSystem.redJewelEnabled)
        {
            selectionBlueJewelIU.enabled = false;
            selectionGreenJewelIU.enabled = false;
            selectionYellowJewelIU.enabled = false;
            selectionPurpleJewelIU.enabled = false;
            icons[2].sprite = selectionRedJewelIU.sprite;
            icons[2].enabled = true;
        }
        else if (JewelSystem.blueJewelEnabled)
        {
            selectionRedJewelIU.enabled = false;
            selectionGreenJewelIU.enabled = false;
            selectionYellowJewelIU.enabled = false;
            selectionPurpleJewelIU.enabled = false;
            icons[2].sprite = selectionBlueJewelIU.sprite;
            icons[2].enabled = true;
        }
        else if (JewelSystem.greenJewelEnabled)
        {
            selectionRedJewelIU.enabled = false;
            selectionBlueJewelIU.enabled = false;
            selectionYellowJewelIU.enabled = false;
            selectionPurpleJewelIU.enabled = false;
            icons[2].sprite = selectionGreenJewelIU.sprite;
            icons[2].enabled = true;
        }
        else if (JewelSystem.yellowJewelEnabled)
        {
            selectionRedJewelIU.enabled = false;
            selectionBlueJewelIU.enabled = false;
            selectionGreenJewelIU.enabled = false;
            selectionPurpleJewelIU.enabled = false;
            icons[2].sprite = selectionYellowJewelIU.sprite;
            icons[2].enabled = true;
        }
        else if (JewelSystem.purpleJewelEnabled)
        {
            selectionRedJewelIU.enabled = false;
            selectionBlueJewelIU.enabled = false;
            selectionGreenJewelIU.enabled = false;
            selectionYellowJewelIU.enabled = false;
            icons[2].sprite = selectionPurpleJewelIU.sprite;
            icons[2].enabled = true;
        }
        else
        {
            icons[2].enabled = false;
        }
    }
}
