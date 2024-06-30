using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ini adalah scripts yang ada pada Local Manager di scene MainMenu
/// Scripts ini mengatur untuk btn main menu; StartBtn dan ExitBtn
/// Dapat ditambahkan panel pengaturan jika ada
/// </summary>

public class LocalManager_MainMenu : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private List<Unit> customUnitIndex = new List<Unit>();
    [SerializeField] private bool fixedUnitNumber = true;
    [SerializeField] private int unitNumber = 3;

    public void OnBtnStart_Team()
    {
        if (fixedUnitNumber)
            ArenaGameManager.instance.BattleStart_Team(unitNumber);
        else
            ArenaGameManager.instance.BattleStart_Team();
    }

    public void OnBtnStart_Team_CustomUnit()
    {
        ArenaGameManager.instance.BattleStart_Team(customUnitIndex);  
    }

    public void OnBtnStart_Team_Random_CustomUnitIndex()
    {
        if (fixedUnitNumber)
            ArenaGameManager.instance.BattleStart_TeamRandom(customUnitIndex, unitNumber);
        else
            ArenaGameManager.instance.BattleStart_TeamRandom(customUnitIndex);
    }

    // Terpasang pada fungsi Button di StartButton;
    public void OnBtnExit()
    {
        // Auto Save System (jika ada)

#if UNITY_EDITOR
        Debug.Log("Keluar dari aplikasi");
#endif

        Application.Quit();
    }
}