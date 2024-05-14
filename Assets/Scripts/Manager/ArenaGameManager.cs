using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaGameManager : MonoBehaviour
{
    #region Singleton
    public static ArenaGameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [Header("Setting")]
    [SerializeField] public string ArenaScene;
    [SerializeField] public string HomeSceneName;

    public List<Unit> unitIndex = new List<Unit>();

    [SerializeField] private int maxHodlerCount = 3;

    [Header("Inspector")]
    public List<Unit> playerUnits = new List<Unit>();
    public List<Unit> enemyUnits = new List<Unit>();

    public void BattleStart_Team()
    {
        playerUnits = RandomUnits(Random.Range(1, maxHodlerCount + 1));
        enemyUnits = RandomUnits(Random.Range(1, maxHodlerCount + 1));

        SceneManager.LoadScene(ArenaScene, LoadSceneMode.Single);
    }

    public void BattleStart_Team(int _unitNumber)
    {
        playerUnits = RandomUnits(Mathf.Clamp(_unitNumber, 1, maxHodlerCount));
        enemyUnits = RandomUnits(Mathf.Clamp(_unitNumber, 1, maxHodlerCount));

        SceneManager.LoadScene(ArenaScene, LoadSceneMode.Single);
    }

    public bool BattleStart_Team(List<Unit> _unitList)
    {
        if (maxHodlerCount > _unitList.Count)
            return false;

        playerUnits = RandomUnits(_unitList);
        enemyUnits = RandomUnits(_unitList);

        SceneManager.LoadScene(ArenaScene, LoadSceneMode.Single);
        return true;
    }

    public void BattleStart_TeamRandom(List<Unit> _unitList)
    {
        playerUnits = RandomUnits(_unitList);
        enemyUnits = RandomUnits(_unitList);

        SceneManager.LoadScene(ArenaScene, LoadSceneMode.Single);
    }

    public void BattleStart_TeamRandom(List<Unit> _unitList, int _unitNumber)
    {
        playerUnits = RandomUnits(_unitList, _unitNumber);
        enemyUnits = RandomUnits(_unitList, _unitNumber);

        SceneManager.LoadScene(ArenaScene, LoadSceneMode.Single);
    }

    public void BattleStart_Team(List<Unit> _unitList, int _unitNumberAllies, int _unitNumberEnemies)
    {
        playerUnits = RandomUnits(_unitList, _unitNumberAllies);
        enemyUnits = RandomUnits(_unitList, _unitNumberEnemies);

        SceneManager.LoadScene(ArenaScene, LoadSceneMode.Single);
    }

    public void BattleResult(bool isWin)
    {
        playerUnits.Clear();
        enemyUnits.Clear();

        Debug.Log("Battle end!");

        if (isWin) // Win
        {
            // What happen when user win the battle

            SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
        }
        else // Lost
        {
            // What happen when user lost the battle

            SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
        }
    }

    #region UnitGenerator
    // Generate random unit and team
    public List<Unit> RandomUnits(int number)
    {
        List<Unit> tmpUnits = new List<Unit>();

        Debug.Log($"Unit count {number}");

        for (int i = 0; i<number; i++)
        {
            var index = Random.Range(0, unitIndex.Count);
            tmpUnits.Add(unitIndex[index]);
        }

        return tmpUnits;
    }

    public List<Unit> RandomUnits(List<Unit> _listUnits, int _number = -99)
    {
        if (_number < 1)
            _number = maxHodlerCount;

        List<Unit> tmpUnits = new List<Unit>();

        for (int i = 0; i < _number; i++)
        {
            var index = Random.Range(0, unitIndex.Count);
            tmpUnits.Add(unitIndex[index]);
        }

        return tmpUnits;
    }
    #endregion
}