using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateHeroUI : BaseUI
{
    [SerializeField]
    private GameObject itemRes;
    [SerializeField]
    private Transform contentTrf;
    [SerializeField]
    private List<CreateHeroInfoItemUI> itemList = new List<CreateHeroInfoItemUI>();
    [SerializeField]
    private Button exitBtn;
    private void Awake()
    {
        exitBtn.onClick.AddListener(ExitBtnClick);
    }
    public void Open(HeroLand _heroLand, List<HeroData> _heroDataList)
    {
        if(_heroDataList.Count > itemList.Count)
        {
            int diff = _heroDataList.Count - itemList.Count;
            for(int i = 0; i < diff; i++)
            {
                CreateHeroInfoItemUI item = Instantiate(itemRes, contentTrf).GetComponent<CreateHeroInfoItemUI>();
                itemList.Add(item);
            }
        }

        _heroDataList.Sort(IndexOfSort);

        for (int i = 0; i < itemList.Count; i++)
        {
            if (i < _heroDataList.Count)
            {
                itemList[i].Set(_heroDataList[i], (int _idx) => { _heroLand.SetHero(_heroDataList[i].index); });
                itemList[i].gameObject.SetActive(true);
            }
            else
            {
                itemList[i].gameObject.SetActive(false);
            }
        }
    }
    private void ExitBtnClick()
    {
        HideUI();
    }
    /// <summary>
    /// index 순으로 정렬 오름차순 정렬
    /// </summary>
    private int IndexOfSort(HeroData _a, HeroData _b)
    {
        if(_a.index > _b.index)
        {
            return 1;
        }
        else if(_a.index < _b.index)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
    public override void HideUI()
    {
        base.HideUI();
    }
}
