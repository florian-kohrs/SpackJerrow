using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimateItem
{

    public static void AnimateItems(List<ItemContainer> items, float delay, MonoBehaviour animatedBy)
    {
        animatedBy.StartCoroutine(AnimateItems(items, delay, animatedBy, animatedBy.transform.position));
    }

    private static IEnumerator AnimateItems(List<ItemContainer> items, float delay, MonoBehaviour animatedBy, Vector3 position)
    {
        List<ItemContainer> animateItems = new List<ItemContainer>();
        animateItems.AddRange(items);

        float delayTimeBetweenContainers = 0.85f;

        yield return new WaitForSeconds(delay);
        foreach (ItemContainer i in animateItems)
        {
            yield return DisplayContainer(i, animatedBy, position);
            yield return new WaitForSeconds(delayTimeBetweenContainers);
        }
    }

    public static void AnimateContainer(ItemContainer item, MonoBehaviour animatedBy, Vector3 spawnPosition)
    {
        animatedBy.StartCoroutine(DisplayContainer(item, animatedBy, spawnPosition));
    }

    public static IEnumerator DisplayContainer(ItemContainer item, MonoBehaviour animatedBy, Vector3 spawnPosition)
    {
        yield return DisplayContainer(item, item.item.RuntimeRef.itemClip, animatedBy, spawnPosition);
    }

    public static IEnumerator DisplayContainer(ItemContainer item, AudioClip sound, MonoBehaviour animatedBy, Vector3 spawnPosition)
    {
        int displayCount = 0;
        float delayTimeForEachItem = 0.2f;
        float displayCountDown = 0;
        float itemFloatHeight = 3.33f;
        float itemLifeTime = 1.25f;

        while (displayCount < item.itemCount)
        {
            if (displayCountDown <= 0)
            {
                displayCountDown += delayTimeForEachItem;
                GameObject i = Object.Instantiate(item.item.RuntimeRef.gameObject);
                Vector3 startScale = i.transform.localScale;
                PlayerGlobalSounds.PlayClip(sound);
                Object.Destroy(i.GetComponent<SaveablePrefabRoot>());
                foreach (ItemBehaviour b in i.GetComponents<ItemBehaviour>())
                {
                    b.enabled = false;
                }
                animatedBy.StartCoroutine(SmoothTransformation<Vector3>.GetStoppable(spawnPosition, spawnPosition + Vector3.up * itemFloatHeight, itemLifeTime,
                    p => { i.transform.localPosition = p; },
                    (s, e, p) =>
                    {
                        i.transform.localScale = startScale * Mathf.Cos((p / 2) * Mathf.PI);
                        return Vector3.Lerp(s, e, p);
                    },
                    () => Object.Destroy(i)
                    ));
                displayCount++;
            }
            yield return new WaitForFixedUpdate();
            displayCountDown -= Time.deltaTime;
        }
    }


}
