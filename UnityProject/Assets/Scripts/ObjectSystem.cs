using UnityEngine;

public class ObjectSystem : MonoBehaviour
{
    [Header("Vitality Section")]
    public static bool vitality1 = false;
    public static bool vitality2 = false;
    [Header("Journal Section")]
    public static bool entry1 = false;
    public static bool entry2 = false;
    public static bool entry3 = false;
    public static bool entry4 = false;
    public static bool entry5 = false;
    public static bool entry6 = false;
    public static bool entry7 = false;
    public static bool entry8 = false;
    public static bool entry9 = false;
    public static bool entry10 = false;
    public static bool entry11 = false;

    [Header("Switch Section")]
    //====================================DungeonOne=====================================//
    public static bool switch1 = false;
    public static bool switch2 = false;
    public static bool switch3 = false;
    public static bool switch4 = false;
    public static bool switch5 = false;
    //====================================DungeonTwo=====================================//
    public static bool switch6 = false;
    //====================================GhostShip=====================================//
    public static bool switch7 = false;
    public static bool switch17 = false;
    //====================================OverWorld=====================================//
    public static bool switch8 = false;
    //====================================TempleOne=====================================//
    public static bool switch9 = false;
    public static bool switch10 = false;
    public static bool switch11 = false;
    public static bool switch12 = false;
    public static bool switch13 = false;
    public static bool switch14 = false;
    public static bool switch15 = false;
    public static bool switch16 = false;
    //==================================IntroScene=====================================//
    public static bool switch18 = false;
    public static bool switch19 = false;
    public static bool switch20 = false;
    //==================================DwellingTimber=====================================//
    public static bool switch21 = false;
    public static bool switch22 = false;
    public static bool switch23 = false;
    public static bool switch24 = false;
    public static bool switch25 = false;
    public static bool switch26 = false;
    public static bool switch27 = false;
    public static bool switch28 = false;
    public static bool switch29 = false;
    public static bool switch30 = false;
    public static bool switch31 = false;
    //====================================DungeonThree===================================//
    public static bool switch32 = false;
    public static bool switch33 = false;
    public static bool switch34 = false;
    public static bool switch35 = false;

    [Header("Enemy Section")]
    //====================================DungeonOne=====================================//
    public static bool enemySection1 = false;
    public static bool enemySection2 = false;
    public static bool enemySection3 = false;
    //====================================OverWorld=====================================//
    public static bool enemySection4 = false;
    public static bool enemySection5 = false;
    public static bool enemySection6 = false;
    //====================================DungeonTwo=====================================//
    public static bool enemySection7 = false;
    public static bool enemySection8 = false;
    //====================================TempleOne=====================================//
    public static bool enemySection9 = false;
    public static bool enemySection10 = false;
    public static bool enemySection11 = false;
    public static bool enemySection12 = false;
    //====================================DwellingTimber=====================================//
    public static bool enemySection13 = false;
    public static bool enemySection14 = false;
    public static bool enemySection15 = false;
    public static bool enemySection16 = false;
    //====================================DungeonThree===================================//
    public static bool enemySection17 = false;
    public static bool enemySection18 = false;
    [Header("Chest Section")]
    //====================================DungeonOne=====================================//
    public static bool chest1 = false;
    public static bool chest2 = false;
    public static bool chest3 = false;
    public static bool chest4 = false;
    public static bool chest5 = false;
    public static bool chest6 = false;
    public static bool chest7 = false;
    public static bool chest8 = false;
    //====================================OverWorld=====================================//
    public static bool chest9 = false;
    public static bool chest10 = false;
    public static bool chest11 = false;
    public static bool chest12 = false;
    public static bool chest51 = false;
    public static bool chest52 = false;
    //====================================GhostShip=====================================//
    public static bool chest13 = false;
    public static bool chest14 = false;
    public static bool chest15 = false;
    public static bool chest16 = false;
    public static bool chest17 = false;
    public static bool chest18 = false;
    public static bool chest19 = false;
    public static bool chest20 = false;
    public static bool chest21 = false;
    public static bool chest22 = false;
    public static bool chest50 = false;
    //====================================DungeonTwo=====================================//
    public static bool chest23 = false;
    public static bool chest24 = false;
    public static bool chest25 = false;
    public static bool chest26 = false;
    public static bool chest27 = false;
    public static bool chest28 = false;
    public static bool chest29 = false;
    public static bool chest30 = false;
    public static bool chest31 = false;
    public static bool chest32 = false;
    public static bool chest33 = false;
    public static bool chest34 = false;
    //====================================TempleOne=====================================//
    public static bool chest35 = false;
    public static bool chest36 = false;
    public static bool chest37 = false;
    public static bool chest38 = false;
    public static bool chest39 = false;
    public static bool chest40 = false;
    public static bool chest41 = false;
    public static bool chest42 = false;
    public static bool chest43 = false;
    public static bool chest44 = false;
    public static bool chest45 = false;
    //==================================IntroScene=====================================//
    public static bool chest46 = false;
    public static bool chest47 = false;
    public static bool chest48 = false;
    public static bool chest49 = false;
    //==================================DwellingTimber=====================================//
    public static bool chest53 = false;
    public static bool chest54 = false;
    public static bool chest55 = false;
    public static bool chest56 = false;
    public static bool chest57 = false;
    //====================================DungeonThree===================================//
    public static bool chest58 = false;
    public static bool chest59 = false;
    public static bool chest60 = false;
    public static bool chest61 = false;
    public static bool chest62 = false;

    [Header("Door Section")]
    //====================================DungeonOne=====================================//
    public static bool door1 = false;
    //====================================GhostShip=====================================//
    public static bool door2 = false;
    public static bool door3 = false;
    public static bool door4 = false;
    //====================================DungeonTwo=====================================//
    public static bool door5 = false;
    public static bool door6 = false;
    //====================================Temple1=====================================//
    public static bool door7 = false;
    public static bool door8 = false;
    public static bool door9 = false;
    //==================================IntroScene=====================================//
    public static bool door10 = false;
    //====================================OverWorld=====================================//
    public static bool door11 = false;
    public static bool door12 = false;
    public static bool door13 = false;
    //====================================DungeonThree===================================//
    public static bool door14 = false;
    public static bool door15 = false;
    [Header("EventSection")]
    //====================================OverWorld=====================================//
    public static bool event1 = false;
    public static bool event2 = false;
    public static bool event5 = false;
    //====================================Temple1=====================================//
    public static bool event3 = false;
    public static bool event4 = false;

    public enum Level { IntroScene, OverWorld, Dungeon1, Dungeon2, GhostShip, Dungeon3, Temple1, Tree }
    //---------------ObjectMaster-----------------------//
    public void ToggleObjectsActive(Level level, bool active)
    {
        switch (level)
        {
            case Level.IntroScene:
                {
                    switch18 = active;
                    switch19 = active;
                    switch20 = active;
                    door10 = active;
                    chest46 = active;
                    chest47 = active;
                    chest48 = active;
                    chest49 = active;
                    break;
                }
            case Level.OverWorld:
                {
                    switch8 = active;
                    event1 = active;
                    event2 = active;
                    event5 = active;
                    door11 = active;
                    door12 = active;
                    door13 = active;
                    chest9 = active;
                    chest10 = active;
                    chest11 = active;
                    chest12 = active;
                    chest51 = active;
                    chest52 = active;
                    break;
                }
            case Level.Dungeon1:
                { 
                    switch1 = active;
                    switch2 = active;
                    switch3 = active;
                    switch4 = active;
                    switch5 = active;
                    door1 = active;
                    chest1 = active;
                    chest2 = active;
                    chest3 = active;
                    chest4 = active;
                    chest5 = active;
                    chest6 = active;
                    chest7 = active;
                    chest8 = active;
                    break;
                }
            case Level.Dungeon2:
                {
                    switch6 = active;
                    chest23 = active;
                    chest24 = active;
                    chest25 = active;
                    chest26 = active;
                    chest27 = active;
                    chest28 = active;
                    chest29 = active;
                    chest30 = active;
                    chest31 = active;
                    chest32 = active;
                    chest33 = active;
                    chest34 = active;
                    door5 = active;
                    door6 = active;
                    break;
                }
            case Level.GhostShip:
                {
                    switch7 = active;
                    switch17 = active;
                    chest13 = active;
                    chest14 = active;
                    chest15 = active;
                    chest16 = active;
                    chest17 = active;
                    chest18 = active;
                    chest19 = active;
                    chest20 = active;
                    chest21 = active;
                    chest22 = active;
                    chest50 = active;
                    door2 = active;
                    door3 = active;
                    door4 = active;
                    break;
                }
            case Level.Temple1:
                {
                    switch9 = active;
                    switch10 = active;
                    switch11 = active;
                    switch12 = active;
                    switch13 = active;
                    switch14 = active;
                    switch15 = active;
                    switch16 = active;
                    chest35 = active;
                    chest36 = active;
                    chest37 = active;
                    chest38 = active;
                    chest39 = active;
                    chest40 = active;
                    chest41 = active;
                    chest42 = active;
                    chest43 = active;
                    chest44 = active;
                    chest45 = active;
                    door7 = active;
                    door8 = active;
                    door9 = active;
                    event3 = active;
                    event4 = active;
                    break;

                }
            case Level.Dungeon3:
                {
                    switch32 = active;
                    switch33 = active;
                    switch34 = active;
                    switch35 = active;
                    chest58 = active;
                    chest59 = active;
                    chest60 = active;
                    chest61 = active;
                    chest62 = active;
                    door14 = active;
                    door15 = active;
                    break;
                }
            case Level.Tree:
                {
                    switch21 = active;
                    switch22 = active;
                    switch23 = active;
                    switch24 = active;
                    switch25 = active;
                    switch26 = active;
                    switch27 = active;
                    switch28 = active;
                    switch29 = active;
                    switch30 = active;
                    switch31 = active;
                    chest53 = active;
                    chest54 = active;
                    chest55 = active;
                    chest56 = active;
                    chest57 = active;
                    break;
                }
        }
    }
    public void BeatTheGame()
    {
        ToggleObjectsActive(Level.IntroScene, true);
        ToggleObjectsActive(Level.OverWorld, true);
        ToggleObjectsActive(Level.Dungeon1, true);
        ToggleObjectsActive(Level.Dungeon2, true);
        ToggleObjectsActive(Level.Temple1, true);
        ToggleObjectsActive(Level.GhostShip, true);
        ToggleObjectsActive(Level.Dungeon3, true);
        ToggleObjectsActive(Level.Tree, true);
    }
    public void UnBeatTheGame()
    {
        ToggleObjectsActive(Level.IntroScene, false);
        ToggleObjectsActive(Level.OverWorld, false);
        ToggleObjectsActive(Level.Dungeon1, false);
        ToggleObjectsActive(Level.Dungeon2, false);
        ToggleObjectsActive(Level.Temple1, false);
        ToggleObjectsActive(Level.GhostShip, false);
        ToggleObjectsActive(Level.Dungeon3, false);
        ToggleObjectsActive(Level.Tree, false);
    }
    //---------------Switches-----------------------//
    public void SetSwitchActive(int num)
    {
        if (num == 1)
            switch1 = true;
        else if (num == 2)
            switch2 = true;
        else if (num == 3)
            switch3 = true;
        else if (num == 4)
            switch4 = true;
        else if (num == 5)
            switch5 = true;
        else if (num == 6)
            switch6 = true;
        else if (num == 7)
            switch7 = true;
        else if (num == 8)
            switch8 = true;
        else if (num == 9)
            switch9 = true;
        else if (num == 10)
            switch10 = true;
        else if (num == 11)
            switch11 = true;
        else if (num == 12)
            switch12 = true;
        else if (num == 13)
            switch13 = true;
        else if (num == 14)
            switch14 = true;
        else if (num == 15)
            switch15 = true;
        else if (num == 16)
            switch16 = true;
        else if (num == 17)
            switch17 = true;
        else if (num == 18)
            switch18 = true;
        else if (num == 19)
            switch19 = true;
        else if (num == 20)
            switch20 = true;
        else if (num == 20)
            switch20 = true;
        else if (num == 21)
            switch21 = true;
        else if (num == 22)
            switch22 = true;
        else if (num == 23)
            switch23 = true;
        else if (num == 24)
            switch24 = true;
        else if (num == 25)
            switch25 = true;
        else if (num == 26)
            switch26 = true;
        else if (num == 27)
            switch27 = true;
        else if (num == 28)
            switch28 = true;
        else if (num == 29)
            switch29 = true;
        else if (num == 30)
            switch30 = true;
        else if (num == 31)
            switch31 = true;
        else if (num == 32)
            switch32 = true;
        else if (num == 33)
            switch33 = true;
        else if (num == 34)
            switch34 = true;
        else if (num == 35)
            switch35 = true;



    }
    public void LoadSwitches(int num, bool open)
    {
        if (num == 1)
            switch1 = open;
        else if (num == 2)
            switch2 = open;
        else if (num == 3)
            switch3 = open;
        else if (num == 4)
            switch4 = open;
        else if (num == 5)
            switch5 = open;

        else if (num == 6)
            switch6 = open;
        else if (num == 7)
            switch7 = open;
        else if (num == 8)
            switch8 = open;

        else if (num == 9)
            switch9 = open;
        else if (num == 10)
            switch10 = open;
        else if (num == 11)
            switch11 = open;
        else if (num == 12)
            switch12 = open;
        else if (num == 13)
            switch13 = open;
        else if (num == 14)
            switch14 = open;
        else if (num == 15)
            switch15 = open;
        else if (num == 16)
            switch16 = open;

        else if (num == 17)
            switch17 = open;
        else if (num == 18)
            switch18 = open;
        else if (num == 19)
            switch19 = open;
        else if (num == 20)
            switch20 = open;

        else if (num == 21)
            switch21 = open;
        else if (num == 22)
            switch22 = open;
        else if (num == 23)
            switch23 = open;
        else if (num == 24)
            switch24 = open;
        else if (num == 25)
            switch25 = open;
        else if (num == 26)
            switch26 = open;
        else if (num == 27)
            switch27 = open;
        else if (num == 28)
            switch28 = open;
        else if (num == 29)
            switch29 = open;
        else if (num == 30)
            switch30 = open;
        else if (num == 31)
            switch31 = open;

        else if (num == 32)
            switch32 = open;
        else if (num == 33)
            switch33 = open;
        else if (num == 34)
            switch34 = open;
        else if (num == 35)
            switch35 = open;

    }
    //---------------Enemies-----------------------//
    public void SetEnemySectionActive(int num)
    {
        if (num == 1)
            enemySection1 = true;
        else if (num == 2)
            enemySection2 = true;
        else if (num == 3)
            enemySection3 = true;
        else if (num == 4)
            enemySection4 = true;
        else if (num == 5)
            enemySection5 = true;
        else if (num == 6)
            enemySection6 = true;
        else if (num == 7)
            enemySection7 = true;
        else if (num == 8)
            enemySection8 = true;
        else if (num == 9)
            enemySection9 = true;
        else if (num == 10)
            enemySection10 = true;
        else if (num == 11)
            enemySection11 = true;
        else if (num == 12)
            enemySection12 = true;
        else if (num == 13)
            enemySection13 = true;
        else if (num == 14)
            enemySection14 = true;
        else if (num == 15)
            enemySection15 = true;
        else if (num == 16)
            enemySection16 = true;
        else if (num == 17)
            enemySection17 = true;
        else if (num == 18)
            enemySection18 = true;
    }
    public void LoadEnemies(int num, bool open)
    {
        if (num == 1)
            enemySection1 = open;
        else if (num == 2)
            enemySection2 = open;
        else if (num == 3)
            enemySection3 = open;
        else if (num == 4)
            enemySection4 = open;
        else if (num == 5)
            enemySection5 = open;
        else if (num == 6)
            enemySection6 = open;
        else if (num == 7)
            enemySection7 = open;
        else if (num == 8)
            enemySection8 = open;
        else if (num == 9)
            enemySection9 = open;
        else if (num == 10)
            enemySection10 = open;
        else if (num == 11)
            enemySection11 = open;
        else if (num == 12)
            enemySection12 = open;
        else if (num == 13)
            enemySection13 = open;
        else if (num == 14)
            enemySection14 = open;
        else if (num == 15)
            enemySection15 = open;
        else if (num == 16)
            enemySection16 = open;
        else if (num == 17)
            enemySection17 = open;
        else if (num == 18)
            enemySection18 = open;
    }
    //---------------Chests-----------------------//
    public void SetChestActive(int num)
    {
        if (num == 1)
            chest1 = true;
        else if (num == 2)
            chest2 = true;
        else if (num == 3)
            chest3 = true;
        else if (num == 4)
            chest4 = true;
        else if (num == 5)
            chest5 = true;
        else if (num == 6)
            chest6 = true;
        else if (num == 7)
            chest7 = true;
        else if (num == 8)
            chest8 = true;

        else if (num == 9)
            chest9 = true;
        else if (num == 10)
            chest10 = true;
        else if (num == 11)
            chest11 = true;
        else if (num == 12)
            chest12 = true;

        else if (num == 13)
            chest13 = true;
        else if (num == 14)
            chest14 = true;
        else if (num == 15)
            chest15 = true;
        else if (num == 16)
            chest16 = true;
        else if (num == 17)
            chest17 = true;
        else if (num == 18)
            chest18 = true;
        else if (num == 19)
            chest19 = true;
        else if (num == 20)
            chest20 = true;
        else if (num == 21)
            chest21 = true;
        else if (num == 22)
            chest22 = true;

        else if (num == 23)
            chest23 = true;
        else if (num == 24)
            chest24 = true;
        else if (num == 25)
            chest25 = true;
        else if (num == 26)
            chest26 = true;
        else if (num == 27)
            chest27 = true;
        else if (num == 28)
            chest28 = true;
        else if (num == 29)
            chest29 = true;
        else if (num == 30)
            chest30 = true;
        else if (num == 31)
            chest31 = true;
        else if (num == 32)
            chest32 = true;
        else if (num == 33)
            chest33 = true;
        else if (num == 34)
            chest34 = true;

        else if (num == 35)
            chest35 = true;
        else if (num == 36)
            chest36 = true;
        else if (num == 37)
            chest37 = true;
        else if (num == 38)
            chest38 = true;
        else if (num == 39)
            chest39 = true;
        else if (num == 40)
            chest40 = true;
        else if (num == 41)
            chest41 = true;
        else if (num == 42)
            chest42 = true;
        else if (num == 43)
            chest43 = true;
        else if (num == 44)
            chest44 = true;
        else if (num == 45)
            chest45 = true;
        else if (num == 46)
            chest46 = true;
        else if (num == 47)
            chest47 = true;
        else if (num == 48)
            chest48 = true;
        else if (num == 49)
            chest49 = true;
        else if (num == 50)
            chest50 = true;
        else if (num == 51)
            chest51 = true;
        else if (num == 52)
            chest52 = true;

        else if (num == 53)
            chest53 = true;
        else if (num == 54)
            chest54 = true;
        else if (num == 55)
            chest55 = true;
        else if (num == 56)
            chest56 = true;
        else if (num == 57)
            chest57 = true;

        else if (num == 58)
            chest58 = true;
        else if (num == 59)
            chest59 = true;
        else if (num == 60)
            chest60 = true;
        else if (num == 61)
            chest61 = true;
        else if (num == 62)
            chest62 = true;
    }
    public void LoadChests(int num, bool open)
    {
        if (num == 1)
            chest1 = open;
        else if (num == 2)
            chest2 = open;
        else if (num == 3)
            chest3 = open;
        else if (num == 4)
            chest4 = open;
        else if (num == 5)
            chest5 = open;
        else if (num == 6)
            chest6 = open;
        else if (num == 7)
            chest7 = open;
        else if (num == 8)
            chest8 = open;

        else if (num == 9)
            chest9 = open;
        else if (num == 10)
            chest10 = open;
        else if (num == 11)
            chest11 = open;
        else if (num == 12)
            chest12 = open;

        else if (num == 13)
            chest13 = open;
        else if (num == 14)
            chest14 = open;
        else if (num == 15)
            chest15 = open;
        else if (num == 16)
            chest16 = open;
        else if (num == 17)
            chest17 = open;
        else if (num == 18)
            chest18 = open;
        else if (num == 19)
            chest19= open;
        else if (num == 20)
            chest20 = open;
        else if (num == 21)
            chest21 = open;
        else if (num == 22)
            chest22 = open;

        else if (num == 23)
            chest23 = open;
        else if (num == 24)
            chest24 = open;
        else if (num == 25)
            chest25 = open;
        else if (num == 26)
            chest26 = open;
        else if (num == 27)
            chest27 = open;
        else if (num == 28)
            chest28 = open;
        else if (num == 29)
            chest29 = open;
        else if (num == 30)
            chest30 = open;
        else if (num == 31)
            chest31 = open;
        else if (num == 32)
            chest32 = open;
        else if (num == 33)
            chest33 = open;
        else if (num == 34)
            chest34 = open;

        else if (num == 35)
            chest35 = open;
        else if (num == 36)
            chest36 = open;
        else if (num == 37)
            chest37 = open;
        else if (num == 38)
            chest38 = open;
        else if (num == 39)
            chest39 = open;
        else if (num == 40)
            chest40 = open;
        else if (num == 41)
            chest41 = open;
        else if (num == 42)
            chest42 = open;
        else if (num == 43)
            chest43 = open;
        else if (num == 44)
            chest44 = open;
        else if (num == 45)
            chest45 = open;
        else if (num == 46)
            chest46 = open;
        else if (num == 47)
            chest47 = open;
        else if (num == 48)
            chest48 = open;
        else if (num == 49)
            chest49 = open;
        else if (num == 50)
            chest50 = open;
        else if (num == 51)
            chest51 = open;
        else if (num == 52)
            chest52 = open;

        else if (num == 53)
            chest53 = open;
        else if (num == 54)
            chest54 = open;
        else if (num == 55)
            chest55 = open;
        else if (num == 56)
            chest56 = open;
        else if (num == 57)
            chest57 = open;

        else if (num == 58)
            chest58 = open;
        else if (num == 59)
            chest59 = open;
        else if (num == 60)
            chest60 = open;
        else if (num == 61)
            chest61 = open;
        else if (num == 62)
            chest62 = open;
    }
    //---------------Doors-----------------------//
    public void SetDoorActive(int num)
    {
        if (num == 1)
            door1 = true;
        else if (num == 2)
            door2 = true;
        else if (num == 3)
            door3 = true;
        else if (num == 4)
            door4 = true;

        else if (num == 5)
            door5 = true;
        else if (num == 6)
            door6 = true;

        else if (num == 7)
            door7 = true;
        else if (num == 8)
            door8 = true;
        else if (num == 9)
            door9 = true;

        else if (num == 10)
            door10 = true;
        else if (num == 11)
            door11 = true;
        else if (num == 12)
            door12 = true;
        else if (num == 13)
            door13 = true;

        else if (num == 14)
            door14 = true;
        else if (num == 15)
            door15 = true;
    }
    public void LoadDoors(int num, bool open)
    {
        if (num == 1)
            door1 = open;
        else if (num == 2)
            door2 = open;
        else if (num == 3)
            door3 = open;
        else if (num == 4)
            door4 = open;

        else if (num == 5)
            door5 = open;
        else if (num == 6)
            door6 = open;

        else if (num == 7)
            door7 = open;
        else if (num == 8)
            door8 = open;
        else if (num == 9)
            door9 = open;

        else if (num == 10)
            door10 = open;
        else if (num == 11)
            door11 = open;
        else if (num == 12)
            door12= open;
        else if (num == 13)
            door13 = open;

        else if (num == 14)
            door14 = open;
        else if (num == 15)
            door15 = open;

    }
    //---------------Journals-----------------------//
    public void SetJournalActive(int num)
    {
        if (num == 1)
            entry1 = true;
        else if (num == 2)
            entry2 = true;
        else if (num == 3)
            entry3 = true;
        else if (num == 4)
            entry4 = true;

        else if (num == 5)
            entry5 = true;
        else if (num == 6)
            entry6 = true;

        else if (num == 7)
            entry7 = true;
        else if (num == 8)
            entry8 = true;
        else if (num == 9)
            entry9 = true;

        else if (num == 10)
            entry10 = true;
        else if (num == 11)
            entry11 = true;
    }
    public void LoadJournals(int num, bool open)
    {
        if (num == 1)
            entry1 = open;
        else if (num == 2)
            entry2 = open;
        else if (num == 3)
            entry3 = open;
        else if (num == 4)
            entry4 = open;

        else if (num == 5)
            entry5 = open;
        else if (num == 6)
            entry6 = open;

        else if (num == 7)
            entry7 = open;
        else if (num == 8)
            entry8 = open;
        else if (num == 9)
            entry9 = open;

        else if (num == 10)
            entry10 = open;
        else if (num == 11)
            entry11 = open;
     
    }
    //---------------Events-----------------------//
    public void SetEventActive(int num)
    {
        if (num == 1)
            event1 = true;
        if (num == 2)
            event2 = true;
        if (num == 3)
            event3 = true;
        if (num == 4)
            event4 = true;
        if (num == 5)
            event5 = true;
    }
    public void LoadEvents(int num, bool open)
    {
        if (num == 1)
            event1 = open;
        if (num == 2)
            event2 = open;
        if (num == 3)
            event3 = open;
        if (num == 4)
            event4 = open;
        if (num == 5)
            event5 = open;
    }
}
