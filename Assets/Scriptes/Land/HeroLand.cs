using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshObstacle))]
public class HeroLand : Land, IClickable
{
    [SerializeField]
    private NavMeshObstacle navObstacle;

    [SerializeField]
    private Vector3 heroPosOffset = Vector3.zero;
    // head child�� ������ �������� ����
    [SerializeField]
    private Transform head;

    public void SetHero(int _idx)
    {
        Hero hero = CharacterManager.Instance.CreateHero(_idx);
        hero.transform.SetParent(head);
        // head�� ��... land���� ���� �־�� �ҵ�...
        hero.transform.localPosition = Vector3.zero + heroPosOffset;
        hero.gameObject.layer = gameObject.layer;
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
            // 
        }
    }
}

