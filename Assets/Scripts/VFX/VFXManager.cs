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
    /// ��VFXЧ����Ӧ����Ϸ��������Ϊ�ǻ״̬
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
    /// ��ʾ�ջ���Ϊ��VFXЧ��
    /// </summary>
    /// <param name="effectPosition"></param>
    /// <param name="harvestActionEffect"></param>
    private void DisplayHarvestActionEffect(Vector3 effectPosition, HarvestActionEffect harvestActionEffect)
    {
        switch(harvestActionEffect)
        {
            case HarvestActionEffect.reaping:
                // �Ӷ���������ȡ����Ӧ��Ϸ����  ��������Ϊ�״̬
                GameObject reaping = PoolManager.Instance.ReuseObject(reapingPrefab, effectPosition, Quaternion.identity);
                reaping.SetActive(true);
                // �ȴ����������Ϊ�ǻ״̬
                StartCoroutine(DisableHarvestActionEffect(reaping, twoSeconds));
                break;

            case HarvestActionEffect.none:
                break;

            default: 
                break;

        }
    }
}
