using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit/New Unit", order = 0)]
[Serializable]
public class Unit : ScriptableObject
{
    [Header("Stat")]
    [SerializeField] public string unitName;

    [SerializeField] public Sprite art;

    [SerializeField]
    public float healthPoint = 10000; // current health point
    [HideInInspector] public float maxHealtPoint = 10000;
    [SerializeField] public float shieldPoint = 0;

    public float attackPoint = 100; // current attack point
    [HideInInspector] public float baseAttackPoint = 100;

    public float movement = 8f;
    [HideInInspector] public float currentMovement = 0f;

    [Tooltip("Skill Set on Unit")]
    [SerializeField] public List<Skill> skillSet = new List<Skill>();

    public void Init(Unit _unit)
    {
        unitName = _unit.unitName;

        art = _unit.art;

        healthPoint = _unit.healthPoint;
        maxHealtPoint = healthPoint;
        shieldPoint = _unit.shieldPoint;

        attackPoint = _unit.attackPoint;
        baseAttackPoint = _unit.baseAttackPoint;

        movement = _unit.movement;
        currentMovement = 0f;

        skillSet.Clear();

        foreach (Skill s in _unit.skillSet)
        {
            s.CreateDesc();

            var newSkill = s.Clone();

            if (newSkill.isStartWithZeroCD)
                newSkill.currentCd = 0;
            else
                newSkill.currentCd = newSkill.cd;

            newSkill.CreateDesc();
            skillSet.Add(newSkill);


            //s.currentCd = 0;
            //s.CreateDesc();
            //skillSet.Add(s.Clone());
        }
    }

    public void Reset(Unit _unit)
    {
        healthPoint = _unit.healthPoint;
        maxHealtPoint = _unit.maxHealtPoint;
        shieldPoint = _unit.shieldPoint;

        foreach (Skill s in skillSet)
        {
            foreach (Skill s2 in _unit.skillSet)
            {
                if(s.skillName == s2.skillName)
                {
                    s.currentCd = s2.currentCd;
                    break;
                }
            }
        }
    }

    public float LostHealth()
    {
        return maxHealtPoint - healthPoint;
    }

    public bool Contains(List<Unit> units, Unit unit)
    {
        foreach (Unit n in units)
        {
            if (n.unitName == unit.unitName)
                return true;
        }

        return false;
    }
}
