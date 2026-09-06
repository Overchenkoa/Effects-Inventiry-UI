using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectStorage : MonoBehaviour
{
    [SerializeField] private GameObject effectUI;
    [SerializeField] private Vector2 borderOffset;
    [SerializeField] private float gap;

    private List<EffectUI> effects = new List<EffectUI>();
    public EventHandler<Effect> effectApplied;
    public EventHandler<Effect> effectExpired;

    private void Start()
    {
        effects = new List<EffectUI>();
    }

    public void ApplyEffect(Effect effect)
    {
        int targetEffect = -1;
        try
        {
            targetEffect =  effects.FindIndex(e => e.effectName == effect.effectName);
        } 
        catch { }

        if (targetEffect >= 0)
        {
            effects[targetEffect].timeLeft = effect.duration > effects[targetEffect].timeLeft 
                ? effect.duration : effects[targetEffect].timeLeft;    
            RepositionEffects();
        }
        else
        {
            GameObject eff = Instantiate(effectUI);
            eff.GetComponent<EffectUI>().AddEffect(effect);
            eff.transform.SetParent(FindAnyObjectByType<Canvas>().transform, false);
            eff.GetComponent<EffectUI>().UIDeleted += OnUIDeleted;
            
            float offsetX = (effectUI.GetComponent<RectTransform>().rect.width + gap)*effects.Count;
            eff.GetComponent<RectTransform>().anchoredPosition = new Vector3(borderOffset.x + offsetX, borderOffset.y);

            effects.Add(eff.GetComponent<EffectUI>());
            effectApplied?.Invoke(this, effect);
        }
    }

    private void OnUIDeleted(object sender, Effect effect)
    {
        ((EffectUI)sender).UIDeleted -= OnUIDeleted;
        int index = effects.FindIndex(e => e.effectName == effect.effectName);
        GameObject eff = effects[index].gameObject;
        effects.RemoveAt(index);
        Destroy(eff);
        effectExpired?.Invoke(this, effect);        
        
        Canvas.ForceUpdateCanvases();
        RepositionEffects();
    }


    private void RepositionEffects()
    {
        effects = effects.OrderBy(e => e.timeLeft).ToList();
        for(int i = 0; i < effects.Count; i++)
        {
            float offsetX = (effectUI.GetComponent<RectTransform>().rect.width + gap)*i;
            effects[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(borderOffset.x + offsetX, borderOffset.y);
        }
    }
}
