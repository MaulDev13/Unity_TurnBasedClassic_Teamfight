using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSelection : MonoBehaviour
{
    #region Singleton
    public static ArenaSelection instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void UnitEvent();
    public UnitEvent iAction;

    public List<BattleUnit> selectedUnit = new List<BattleUnit>();
    public int maxSelectedUnitCount = 0;

    BattleUnit unitInAction;
    Skill selectedSkill;

    public void Turn(BattleUnit _unit, Skill _skill)
    {
        unitInAction = _unit;
        selectedSkill = _skill;

        maxSelectedUnitCount = selectedSkill.maxTarget;
    }

    public bool Select(BattleUnit _unit, bool _isSelected)
    {
        /*
        if (unitInAction == null || selectedSkill == null)
            return false;
        */

        if (_isSelected && selectedUnit.Contains(_unit))
        {
            selectedUnit.Remove(_unit);
            return false;
        }

        /*
        if (selectedUnit.Count >= maxSelectedUnitCount)
            return false;
        */

        selectedUnit.Add(_unit);
        return true;

        //LocalManager_Arena.instance.unitTurn;
    }

    public void Clear()
    {
        iAction?.Invoke();

        unitInAction = null;
        selectedSkill = null;

        maxSelectedUnitCount = 1;

        selectedUnit.Clear();
    }
}
