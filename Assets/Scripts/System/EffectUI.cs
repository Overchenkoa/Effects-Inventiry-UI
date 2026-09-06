using UnityEngine.UI;
using UnityEngine;
using System;

public class EffectUI : MonoBehaviour
{
    private Transform Tint;
    private Effect effect;

    public float timeLeft {
        get {return effect.timeLeft; }
        set { effect.timeLeft = value;}
    }
    public string effectName {get {return effect.effectName; }}
    
    public EventHandler<Effect> UIDeleted;

    
    public void AddEffect(Effect eff)
    {
        effect = eff;
        GetComponent<Image>().sprite = effect.Image; 
        effect.timeLeft = effect.duration;

    }
    void Start()
    {
        Tint = transform.GetChild(0); 
    }

    private void ProgressAnimation()
    {
        float value = effect.timeLeft/effect.duration;
        Tint.GetComponent<Image>().fillAmount = 1 - value;
        Canvas.ForceUpdateCanvases();
    }
    
    void Update()
    { 
        timeLeft-=Time.deltaTime;
        if(timeLeft <= 0)
        {
            UIDeleted?.Invoke(this, effect);
        }
        else ProgressAnimation();
    }
}
