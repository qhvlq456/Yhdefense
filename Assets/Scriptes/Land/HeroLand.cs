using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class HeroLand : Land, IClickable
{
    [SerializeField]
    private NavMeshObstacle navObstacle;

    // head child�� ������ �������� ����
    [SerializeField]
    private Transform head;


    public void SetHero(int _idx)
    {
        Hero hero = CharacterManager.Instance.CreateHero(_idx);
        hero.transform.SetParent(head);
        // head�� ��... land���� ���� �־�� �ҵ�...
        hero.transform.localPosition = Vector3.zero;
    }
    public void RemoveHero(HeroData _heroData) 
    { 

    }

    public void OnClick()
    {
        // hero ����â 
        if (head.childCount > 0)
        {
            UIManager.Instance.ShowUI<CharacterInfoUI>(UIPanelType.CharacterInfo);
        }
        // hero ����â
        else
        {
            CreateHeroUI createHeroUI = UIManager.Instance.ShowUI<CreateHeroUI>(UIPanelType.CreateHero);
            createHeroUI.Open(this, DataManager.Instance.HeroDataList);
        }
    }
}
