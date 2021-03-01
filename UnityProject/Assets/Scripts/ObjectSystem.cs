using UnityEngine;

public class ObjectSystem : MonoBehaviour
{
    public static bool[] gameVitality = new bool[2];
    public static bool[] gameEntry = new bool[11];
    public static bool[] gameSwitch = new bool[35];
    public static bool[] gameEnemySection = new bool[18];
    public static bool[] gameChest = new bool[62];
    public static bool[] gameDoor = new bool[62];
    public static bool[] gameEvent = new bool[5];
    public enum Level 
    { 
        RaftersBargain, 
        ConquestIsland, 
        RiverfallShrine, 
        AbandonedShrine, 
        GhostShip, 
        WellPathway, 
        TempleofReign, 
        DwellingTimber
    }
    //---------------ObjectMaster-----------------------//
    public void ToggleObjectsActive(Level level, bool active)
    {
        switch (level)
        {
            case Level.RaftersBargain:
                {
                    LoadActiveObject(17, gameSwitch, active);
                    LoadActiveObject(18, gameSwitch, active);
                    LoadActiveObject(19, gameSwitch, active);
                    LoadActiveObject(9, gameDoor, active);
                    LoadActiveObject(45, gameChest, active);
                    LoadActiveObject(46, gameChest, active);
                    LoadActiveObject(47, gameChest, active);
                    LoadActiveObject(48, gameChest, active);
                    break;
                }
            case Level.ConquestIsland:
                {
                    LoadActiveObject(7, gameSwitch, active);
                    LoadActiveObject(0, gameEvent, active);
                    LoadActiveObject(1, gameEvent, active);
                    LoadActiveObject(4, gameEvent, active);
                    LoadActiveObject(10, gameDoor, active);
                    LoadActiveObject(11, gameDoor, active);
                    LoadActiveObject(12, gameDoor, active);
                    LoadActiveObject(17, gameSwitch, active);
                    LoadActiveObject(18, gameSwitch, active);
                    LoadActiveObject(19, gameSwitch, active);
                    LoadActiveObject(9, gameDoor, active);
                    LoadActiveObject(8, gameChest, active);
                    LoadActiveObject(9, gameChest, active);
                    LoadActiveObject(10, gameChest, active);
                    LoadActiveObject(11, gameChest, active);
                    LoadActiveObject(50, gameChest, active);
                    LoadActiveObject(51, gameChest, active);
                    break;
                }
            case Level.RiverfallShrine:
                {
                    LoadActiveObject(0, gameSwitch, active);
                    LoadActiveObject(1, gameSwitch, active);
                    LoadActiveObject(2, gameSwitch, active);
                    LoadActiveObject(3, gameSwitch, active);
                    LoadActiveObject(4, gameSwitch, active);
                    LoadActiveObject(0, gameDoor, active);
                    LoadActiveObject(0, gameChest, active);
                    LoadActiveObject(1, gameChest, active);
                    LoadActiveObject(2, gameChest, active);
                    LoadActiveObject(3, gameChest, active);
                    LoadActiveObject(4, gameChest, active);
                    LoadActiveObject(5, gameChest, active);
                    LoadActiveObject(6, gameChest, active);
                    LoadActiveObject(7, gameChest, active);
                    break;
                }
            case Level.AbandonedShrine:
                {
                    LoadActiveObject(5, gameSwitch, active);
                    LoadActiveObject(22, gameChest, active);
                    LoadActiveObject(23, gameChest, active);
                    LoadActiveObject(24, gameChest, active);
                    LoadActiveObject(25, gameChest, active);
                    LoadActiveObject(26, gameChest, active);
                    LoadActiveObject(27, gameChest, active);
                    LoadActiveObject(28, gameChest, active);
                    LoadActiveObject(29, gameChest, active);
                    LoadActiveObject(30, gameChest, active);
                    LoadActiveObject(31, gameChest, active);
                    LoadActiveObject(32, gameChest, active);
                    LoadActiveObject(33, gameChest, active);
                    LoadActiveObject(4, gameDoor, active);
                    LoadActiveObject(5, gameDoor, active);
                    break;
                }
            case Level.GhostShip:
                {
                    LoadActiveObject(6, gameSwitch, active);
                    LoadActiveObject(16, gameSwitch, active);
                    LoadActiveObject(12, gameChest, active);
                    LoadActiveObject(13, gameChest, active);
                    LoadActiveObject(14, gameChest, active);
                    LoadActiveObject(15, gameChest, active);
                    LoadActiveObject(16, gameChest, active);
                    LoadActiveObject(17, gameChest, active);
                    LoadActiveObject(18, gameChest, active);
                    LoadActiveObject(19, gameChest, active);
                    LoadActiveObject(20, gameChest, active);
                    LoadActiveObject(21, gameChest, active);
                    LoadActiveObject(49, gameChest, active);
                    LoadActiveObject(1, gameDoor, active);
                    LoadActiveObject(2, gameDoor, active);
                    LoadActiveObject(3, gameDoor, active);
                    break;
                }
            case Level.TempleofReign:
                {
                    LoadActiveObject(8, gameSwitch, active);
                    LoadActiveObject(9, gameSwitch, active);
                    LoadActiveObject(10, gameSwitch, active);
                    LoadActiveObject(11, gameSwitch, active);
                    LoadActiveObject(12, gameSwitch, active);
                    LoadActiveObject(13, gameSwitch, active);
                    LoadActiveObject(14, gameSwitch, active);
                    LoadActiveObject(15, gameSwitch, active);
                    LoadActiveObject(34, gameChest, active);
                    LoadActiveObject(35, gameChest, active);
                    LoadActiveObject(36, gameChest, active);
                    LoadActiveObject(37, gameChest, active);
                    LoadActiveObject(38, gameChest, active);
                    LoadActiveObject(39, gameChest, active);
                    LoadActiveObject(40, gameChest, active);
                    LoadActiveObject(41, gameChest, active);
                    LoadActiveObject(42, gameChest, active);
                    LoadActiveObject(43, gameChest, active);
                    LoadActiveObject(44, gameChest, active);
                    LoadActiveObject(6, gameDoor, active);
                    LoadActiveObject(7, gameDoor, active);
                    LoadActiveObject(8, gameDoor, active);
                    LoadActiveObject(2, gameEvent, active);
                    LoadActiveObject(3, gameEvent, active);
                    break;

                }
            case Level.WellPathway:
                {
                    LoadActiveObject(31, gameSwitch, active);
                    LoadActiveObject(32, gameSwitch, active);
                    LoadActiveObject(33, gameSwitch, active);
                    LoadActiveObject(34, gameSwitch, active);
                    LoadActiveObject(57, gameChest, active);
                    LoadActiveObject(58, gameChest, active);
                    LoadActiveObject(59, gameChest, active);
                    LoadActiveObject(60, gameChest, active);
                    LoadActiveObject(61, gameChest, active);
                    LoadActiveObject(13, gameDoor, active);
                    LoadActiveObject(14, gameDoor, active);
                    break;
                }
            case Level.DwellingTimber:
                {
                    LoadActiveObject(20, gameSwitch, active);
                    LoadActiveObject(21, gameSwitch, active);
                    LoadActiveObject(22, gameSwitch, active);
                    LoadActiveObject(23, gameSwitch, active);
                    LoadActiveObject(24, gameSwitch, active);
                    LoadActiveObject(25, gameSwitch, active);
                    LoadActiveObject(26, gameSwitch, active);
                    LoadActiveObject(27, gameSwitch, active);
                    LoadActiveObject(28, gameSwitch, active);
                    LoadActiveObject(29, gameSwitch, active);
                    LoadActiveObject(30, gameSwitch, active);
                    LoadActiveObject(52, gameChest, active);
                    LoadActiveObject(53, gameChest, active);
                    LoadActiveObject(54, gameChest, active);
                    LoadActiveObject(55, gameChest, active);
                    LoadActiveObject(56, gameChest, active);
                    break;
                }
        }
    }
    public void BeatTheGame()
    {
        ToggleObjectsActive(Level.RaftersBargain, true);
        ToggleObjectsActive(Level.ConquestIsland, true);
        ToggleObjectsActive(Level.RiverfallShrine, true);
        ToggleObjectsActive(Level.AbandonedShrine, true);
        ToggleObjectsActive(Level.TempleofReign, true);
        ToggleObjectsActive(Level.GhostShip, true);
        ToggleObjectsActive(Level.WellPathway, true);
        ToggleObjectsActive(Level.DwellingTimber, true);
    }
    public void UnBeatTheGame()
    {
        ToggleObjectsActive(Level.RaftersBargain, false);
        ToggleObjectsActive(Level.ConquestIsland, false);
        ToggleObjectsActive(Level.RiverfallShrine, false);
        ToggleObjectsActive(Level.AbandonedShrine, false);
        ToggleObjectsActive(Level.TempleofReign, false);
        ToggleObjectsActive(Level.GhostShip, false);
        ToggleObjectsActive(Level.WellPathway, false);
        ToggleObjectsActive(Level.DwellingTimber, false);
    }
    public void SetActiveObject(int idNum, bool[] idObject)
    {
        for (int o = 0; o < idObject.Length; o++)
            if (o == idNum) idObject[o] = true;
    }
    public void SetDeActiveObject(int idNum, bool[] idObject)
    {
        for (int o = 0; o < idObject.Length; o++)
            if (o == idNum) idObject[o] = false;
    }
    public void LoadActiveObject(int idNum, bool[] idObject, bool idActive)
    {
        for (int o = 0; o < idObject.Length; o++)
            if (o == idNum) idObject[o] = idActive;
    }
}
