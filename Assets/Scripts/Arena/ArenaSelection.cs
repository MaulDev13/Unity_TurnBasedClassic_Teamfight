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

    [SerializeField] private Transform selectionButton;

    public delegate void UnitEvent();
    public UnitEvent iAction;

    public List<BattleUnit> selectedUnits = new List<BattleUnit>();
    public int maxSelectedUnitCount = 0;
    private int currentUnitCount = 0;


    BattleUnit unitInAction;
    Skill selectedSkill;

    public void Init(BattleUnit _unit, Skill _skill)
    {
        Clear();

        unitInAction = _unit;
        selectedSkill = _skill;

        if (!CheckSkillTarget())
        {
            NullTarget();
            return;
        }

        maxSelectedUnitCount = selectedSkill.maxTarget;
    }

    public bool Select(BattleUnit _unit, bool _isSelected)
    {
        Debug.Log("Selection");

        if (unitInAction == null || selectedSkill == null)
            return false;

        if (!CheckValidation(_unit))
            return false;

        if (_isSelected && Contains(selectedUnits, _unit))
        {
            selectedUnits.Remove(_unit);
            currentUnitCount--;
            ButtonViewUpdate();
            return false;
        }

        if (currentUnitCount >= maxSelectedUnitCount)
            return false;

        selectedUnits.Add(_unit);
        currentUnitCount++;

        ButtonViewUpdate();

        return true;

        //LocalManager_Arena.instance.unitTurn;
    }

    private bool CheckValidation(BattleUnit _unit)
    {
        if(selectedSkill.isLivingTarget)
        {
            if (!_unit.isAlive)
                return false;
        }
        else
        {
            if(_unit.isAlive)
                return false;
        }


        switch(selectedSkill.target)
        {
            case SkillTarget.Allies:
                if (Contains(unitInAction.myAllies, _unit))
                    return true;
                else
                    return false;
            case SkillTarget.Enemies:
                if (Contains(unitInAction.myEnemies, _unit))
                    return true;
                else
                    return false;
            case SkillTarget.Oneself:
                return false;
            case SkillTarget.Null:
                return false;
            default:
                Debug.LogWarning($"Skill {selectedSkill.skillName} not have a target");
                return false;
        }
    }

    public void Clear()
    {
        iAction?.Invoke();

        unitInAction = null;
        selectedSkill = null;

        currentUnitCount = 0;
        maxSelectedUnitCount = 0;

        selectedUnits.Clear();

        ButtonViewUpdate();
    }

    public void OnSelectionButton()
    {
        if (!CheckSkillTarget())
        {
            NullTarget();
        }
        else
        {
            if (selectedUnits.Count < selectedSkill.minTarget)
                return;

            LocalManager_ArenaUI.instance.ClearActionButton();
            unitInAction.Action(selectedSkill, selectedUnits);
            Clear();
        }
    }

    private void NullTarget()
    {
        LocalManager_ArenaUI.instance.ClearActionButton();
        unitInAction.Action(selectedSkill);
        Clear();
    }

    public bool AutoSelect(BattleUnit _unit, Skill _skill)
    {
        Clear();

        unitInAction = _unit;
        selectedSkill = _skill;

        if (!CheckSkillTarget())
        {
            NullTarget();
            return true;
        }

        currentUnitCount = 0;
        maxSelectedUnitCount = selectedSkill.maxTarget;

        for(int i = 0; i < maxSelectedUnitCount; i++)
        {
            BattleUnit tmpUnit = null;

            int counter = 0;
            while (tmpUnit == null) {
                counter++;
                if (counter > 100)
                    break;

                if(selectedSkill.target == SkillTarget.Allies)
                {
                    var index = Random.Range(0, unitInAction.myAllies.Count);
                    if (CheckValidation(unitInAction.myAllies[index]))
                        tmpUnit = unitInAction.myAllies[index];
                }
                else
                {
                    var index = Random.Range(0, unitInAction.myEnemies.Count);
                    if (CheckValidation(unitInAction.myEnemies[index]))
                        tmpUnit = unitInAction.myEnemies[index];
                }
            }

            if (tmpUnit != null)
                selectedUnits.Add(tmpUnit);
        }

        if (selectedUnits.Count >= selectedSkill.minTarget)
        {
            OnSelectionButton();
            return true;
        }

        return false;
    }

    private void ButtonViewUpdate()
    {
        selectionButton.gameObject.SetActive(false);

        if (unitInAction == null || selectedSkill == null)
            return;

        if (CheckSkillTarget())
        {
            if (selectedUnits.Count < selectedSkill.minTarget)
                return;
        }

        if (unitInAction.isAuto || !unitInAction.isPlayerUnit)
            return;

        selectionButton.gameObject.SetActive(true);
    }

    public bool Contains(List<BattleUnit> battleUnits, BattleUnit battleUnit)
    {
        foreach (BattleUnit bu in battleUnits)
        {
            if (bu.myUnit.unitName == battleUnit.myUnit.unitName)
                return true;
        }

        return false;
    }

    public bool CheckSkillTarget()
    {
        if (selectedSkill.target == SkillTarget.Oneself || selectedSkill.target == SkillTarget.Null || selectedSkill.target == SkillTarget.AllAllies || selectedSkill.target == SkillTarget.AllEnemies)
            return false;

        return true;
    }
}
