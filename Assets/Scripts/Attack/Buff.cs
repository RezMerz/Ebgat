using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour {
    //Buff attributes
    public CharacterAttributes charStats{get;set;}
    public BuffManager buffManager { get; set; }

    protected Coroutine durationCoroutine;


    public float duration;
    public bool stackable;


    public void FinishBuff()
    {
        DebuffCharacter();
        if(durationCoroutine != null)
        {
            StopCoroutine(durationCoroutine);
        }

        buffManager.RemoveBuffFromList(this);
        Destroy(gameObject);
    }

    public void StartBuff()
    {
        BuffCharacter();
        durationCoroutine = StartCoroutine(Duration());
    }

    public void DurationReset()
    {
        StopCoroutine(durationCoroutine);
        durationCoroutine = StartCoroutine(Duration());
    }


    public abstract void BuffCharacter();

    public abstract void DebuffCharacter();


    protected IEnumerator Duration()
    {
        yield return new WaitForSeconds(duration);
        FinishBuff();
    }
}
