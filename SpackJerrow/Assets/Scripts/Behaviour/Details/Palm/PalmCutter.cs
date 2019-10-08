using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PalmCutter : MonoBehaviour, IHealthController
{

    private int health = 15;

    public bool IsAlive => health > 0;

    public int CurrentHealth => health;

    public float despawnTime = 4.6f;

    public InventoryItem wood;
    
    public void Kill(float despawnTime)
    {
        this.despawnTime = despawnTime;
        health = 0;
        StartCoroutine(OnDeath());
    }

    public bool Damage(int damage)
    {
        bool result = CurrentHealth > 0;
        if (result)
        {
            health -= damage;
            result = !IsAlive;
            if (result)
            {
                gameObject.AddComponent<Rigidbody>();
                StartCoroutine(OnDeath());
            }
        }
        return result;
    
    }
    
    private IEnumerator OnDeath()
    {
        ///destroy collider
        GameManager.GetPlayerComponent<Inventory>().AddItem(wood);
        PalmCutter child = null;
        if (transform.childCount > 1)
        {
            Destroy(transform.parent.GetChild(1).gameObject);
            child = transform.GetChild(0).GetComponent<PalmCutter>();
            if (child != null)
            {
                child.Kill(despawnTime + 0.2f);
            }
        }
        yield return new WaitForSeconds(despawnTime);
        
        AnimateItem.AnimateContainer(new ItemContainer(wood, 1), GameManager.GetPlayerComponent<MonoBehaviour>(), transform.position);

        if(child != null)
        {
            child.transform.parent = transform.parent;
        }
        Destroy(gameObject);
        //transform.parent = null;
    }

    //private async void OnDeath()
    //{
    //    //transform.SetAsLastSibling();
    //    GameManager.GetPlayerComponent<Inventory>().AddItem(wood);
    //    float waitTimeLeft = despawnTime * 1000;
    //    while(waitTimeLeft > 0)
    //    {
    //        int waitTime = 50;
    //        await Task.Delay(waitTime);
    //        waitTimeLeft -= waitTime * Time.timeScale;
    //    }
    //    if (this != null)
    //    {
    //        AnimateItem.DisplayContainer(new ItemContainer(wood, 1), GameManager.GetPlayerComponent<MonoBehaviour>(), transform.position);
    //        PalmCutter child = transform.GetChild(0).GetComponent<PalmCutter>();
    //        if (child != null)
    //        {
    //            child.Kill(0.2f);
    //        }
    //        Destroy(gameObject);
    //    }
    //}

}
