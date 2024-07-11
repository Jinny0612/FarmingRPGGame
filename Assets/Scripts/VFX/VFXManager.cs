using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : SingletonMonoBehvior<VFXManager>
{
    private WaitForSeconds twoSeconds;
    [SerializeField] private GameObject reapingPrefab = null;

    protected override void Awake()
    {
        base.Awake();
        twoSeconds = new WaitForSeconds(2f);
    }

    private void OnEnable()
    {
        EventHandler.HarvestActionEffectEvent += DisplayHarvestActionEffect;
    }

    private void OnDisable()
    {
        EventHandler.HarvestActionEffectEvent -= DisplayHarvestActionEffect;
    }

    /// <summary>
    /// 将VFX效果对应的游戏物体设置为非活动状态
    /// </summary>
    /// <param name="effectGameObject"></param>
    /// <param name="secondsToWait"></param>
    /// <returns></returns>
    private IEnumerator DisableHarvestActionEffect(GameObject effectGameObject, WaitForSeconds secondsToWait)
    {
        yield return secondsToWait;
        effectGameObject.SetActive(false);
    }

    /// <summary>
    /// 显示收获行为的VFX效果
    /// </summary>
    /// <param name="effectPosition"></param>
    /// <param name="harvestActionEffect"></param>
    private void DisplayHarvestActionEffect(Vector3 effectPosition, HarvestActionEffect harvestActionEffect)
    {
        switch(harvestActionEffect)
        {
            case HarvestActionEffect.reaping:
                // 从对象管理池中取出对应游戏物体  并且设置为活动状态
                GameObject reaping = PoolManager.Instance.ReuseObject(reapingPrefab, effectPosition, Quaternion.identity);
                reaping.SetActive(true);
                // 等待两秒后设置为非活动状态
                StartCoroutine(DisableHarvestActionEffect(reaping, twoSeconds));
                break;

            case HarvestActionEffect.none:
                break;

            default: 
                break;

        }
    }
}
