using UnityEngine;
using UnityEngine.UI;

public class ArenaMovePoint : MonoBehaviour
{
    private RectTransform unitPoint;
    private RectTransform bar;

    private Image image;
    private float unitMovementThreshold;

    [SerializeField] private float y = 10f;
    private float range;

    private BattleUnit myBattleUnit;

    private void OnDisable()
    {
        myBattleUnit.iMove -= UpdateView;
    }

    public void Init(BattleUnit battleUnit, GameObject parent)
    {
        unitPoint = GetComponent<RectTransform>();
        bar = parent.GetComponent<RectTransform>();
        range = bar.rect.width;

        unitMovementThreshold = LocalManager_Arena.instance.MovementThreshold;

        myBattleUnit = battleUnit;

        if (!myBattleUnit.isPlayerUnit)
        {
            y = -y;
        }

        image = GetComponent<Image>();
        image.sprite = myBattleUnit.myUnit.art;

        myBattleUnit.iMove += UpdateView;
        myBattleUnit.iDead += OnDead;

        UpdateView();
    }

    private void UpdateView()
    {
        var post = Mathf.Clamp(myBattleUnit.GetMove(), 0, unitMovementThreshold);
        var x = (post / unitMovementThreshold * range) - (range / 2);

        unitPoint.localPosition = new Vector3(x, y, 0);

        //Debug.Log($"{myBattleUnit.unit.unitName} is moving! x-{x} = {myBattleUnit.GetMove()} / {unitMovementThreshold} * {range} - {range} / 2");
    }

    private void OnDead()
    {
        this.gameObject.SetActive(false);
    }
}
