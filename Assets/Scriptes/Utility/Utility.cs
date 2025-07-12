using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public static class Utility
{
    #region Static Variables
    public readonly static int HERO_MAX_LV = 3;
    #endregion
    #region Start Object Pool

    public static PoolingType LandTypeToPoolingType(LandType _type)
    {
        PoolingType type = PoolingType.heroLand;

        switch (_type)
        {
            case LandType.hero:
                type = PoolingType.heroLand;
                break;
            case LandType.enemy:
                type = PoolingType.enemyLand;
                break;
        }

        return type;
    }
    #endregion End Object Pool

    #region Start HeroData
    public static string GetHeroInfo(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        string infoText = string.Empty;

        if (_lv <= DataManager.Instance.GetMaxHeroUpgradeLevel(_heroData.index))
        {
            infoText = $"Lv : {_lv}\n" +
                       $"이름 : {_heroData.name}\n" +
                       $"공격타입 : {_heroData.groundType}\n" +
                       $"영웅타입 : {_heroData.heroType}\n" +
                       $"비용 : {heroUpgradeData.cost}\n";

            switch (_heroData.heroType)
            {
                case HeroType.Attack:
                    {
                        // 공격형 히어로
                        AttackData attackData = DataManager.Instance.GetAttackData(_heroData.heroTypeByIdx, _lv);
                        infoText += $"공격속도 : {attackData.speed}\n" +
                                    $"공격력 : {attackData.damage}\n" +
                                    $"공격범위 : {attackData.radius}\n";
                        break;
                    }
                case HeroType.Buffer:
                    {
                        // 버퍼형 히어로
                        BuffData buffData = DataManager.Instance.GetBuffData(_heroData.heroTypeByIdx, _lv);
                        infoText += $"버프타입 : {buffData.buffType}\n" +
                                    $"버프효과 : {buffData.amount}\n" +
                                    $"버프범위 : {buffData.range}\n" +
                                    $"버프지속 : {buffData.duration}\n";
                        break;
                    }
                case HeroType.Debuffer:
                    {
                        // 디버퍼형 히어로
                        DebuffData debuffData = DataManager.Instance.GetDebuffData(_heroData.heroTypeByIdx, _lv);
                        infoText += $"디버프타입 : {debuffData.debuffType}\n" +
                                    $"디버프효과 : {debuffData.amount}\n" +
                                    $"디버프범위 : {debuffData.range}\n" +
                                    $"디버프지속 : {debuffData.duration}\n";
                        break;
                    }
            }

            infoText += $"판매가격 : {heroUpgradeData.sell}";
        }
        else
        {
            infoText = "Max Upgrade!!";
        }

        return infoText;
    }
    public static bool IsHeroPurchase(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        return GameManager.Instance.gold >= heroUpgradeData.cost;
    }
    public static void PurchaseHero(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        GameManager.Instance.UpdateGold(-heroUpgradeData.cost);
        // 후에 구매 로직 추가
    }
    public static void GetHeroSellPrice(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        GameManager.Instance.UpdateGold(heroUpgradeData.sell);
    }
    public static bool IsHeroUpgrade(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        return GameManager.Instance.gold >= heroUpgradeData.cost && _lv > HERO_MAX_LV;
    }
    public static void UpgradeHero(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        GameManager.Instance.UpdateGold(-heroUpgradeData.cost);
        // 후에 업그레이드 로직 추가
    }

    #endregion End HeroData

    #region UI
    /// <summary>
    /// UICamera 사용으로 RectTransform을 사용하여 월드 좌표를 스크린 좌표로 변환합니다.
    /// </summary>
    /// <param name="_position">목표물</param>
    /// <param name="_canvas">표시하고 싶은 UI canvas</param>
    /// <param name="_offset">오프셋</param>
    /// <returns></returns>
    public static Vector3 WorldToScreenPoint(Vector3 _position, RectTransform _canvas, Vector3 _offset)
    {
        // main camera를 사용하여 월드 좌표를 스크린 좌표로 변환합니다.
        Vector3 screenPosition = GameManager.Instance.MainCamera.WorldToScreenPoint(_position);
        screenPosition += _offset;

        Vector3 screenPos = Vector3.zero;

        // screenPosition을 RectTransform의 월드 좌표로 변환합니다.
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas, screenPosition, GameManager.Instance.UICamera, out screenPos))
        {
            return screenPos;
        }
        else
        {
            Debug.Log($"[Utily] Error RectTransformUtility.ScreenPointToWorldPointInRectangle : false, " +
                $"_position : {_position}, _canvas : {_canvas.position}, screenPosition : {screenPosition}");

            return Vector3.zero;
        }
    }
    public static Vector3 WorldToScreenPoint(Vector3 _position, Vector3 _offset)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_position);
        screenPosition = screenPosition + _offset;
        Ray ray = RectTransformUtility.ScreenPointToRay(GameManager.Instance.UICamera, screenPosition);
        return ray.origin;
    }
    /// <summary>
    /// UICamera의 스크린 좌표를 MainCamera 기준의 월드 좌표로 변환합니다.
    /// UICamera와 MainCamera가 서로 다른 위치/방향일 때, UICamera로 만든 Ray가 MainCamera 기준 월드 좌표를 제대로 표현할 수 있느냐?
    /// 상관없다 왜냐 ScreenPointToRay는 단지 UICamera가 보는 시점에서 화면 픽셀(Screen Point)이 월드 공간상 어디를 향하는지에 대한 Ray를 생성해주는 함수이기 때문
    /// 즉, 스크린 좌표로부터 현재 이 화면상 점에서 바라보는 방향은 어디인가를 알려주는거고 그 이후 월드 좌표를 계산할 땐 UICamera의 위치화 방향 기준으로 ray를 뻗어나가는 것 일 뿐
    /// </summary>
    /// <param name="_screenPosition">터치 등으로부터 얻은 스크린 좌표</param>
    /// <param name="_distance">원하는 깊이 (MainCamera로부터 얼마나 떨어진 위치)</param>
    /// <returns>MainCamera 기준의 월드 좌표</returns>
    public static Vector3 ScreenPointToWorldPoint(Vector3 _screenPosition, float _distance)
    {
        // UICamera 기준으로 Ray 생성
        // Step 1. UICamera 스크린 좌표 -> 동일한 픽셀 좌표를 유지 = _screenPosition

        Ray ray = GameManager.Instance.UICamera.ScreenPointToRay(_screenPosition);
        // Step 2. MainCamera로 ray 생성
        // 그 ray를 MainCamera 공간의 특정 거리 위치까지 쏴서 계산
        return ray.origin + ray.direction * _distance;
    }
    #endregion
}
