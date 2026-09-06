using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Interactions;

public class EffectStorage : MonoBehaviour
{
    private List<Effect> effects = new List<Effect>();
    [SerializeField] private GameObject effectUI;
    private List<GameObject> effectsUI = new List<GameObject>();
    public EventHandler<Effect> effectApplied;
    public EventHandler<Effect> effectExpired;

    private void Start()
    {
        effects = new List<Effect>();
        effectsUI = new List<GameObject>();
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
        }
        else
        {
            effects.Add(effect);
            effectApplied?.Invoke(this, effect);

            effect.timeLeft = effect.duration;

            effectUI.GetComponent<Image>().sprite = effect.Image;
            
            
            GameObject eff = Instantiate(effectUI);
            effectsUI.Add(eff);
            eff.transform.SetParent(FindAnyObjectByType<Canvas>().transform, false);
            eff.GetComponent<RectTransform>().anchoredPosition = new Vector3
            (70 + (effectUI.GetComponent<RectTransform>().rect.width + 5)*(effectsUI.Count-1),-70);

            Debug.Log(effect.timeLeft);
        }
    }

    private void Update()
    {
        for(int i = 0; i<effects.Count; i++)
        {
            effects[i].timeLeft-=Time.deltaTime;
            if (effects[i].timeLeft<=0)
            {
                effectExpired?.Invoke(this, effects[i]);
                Destroy(effectsUI[i].gameObject);
                Canvas.ForceUpdateCanvases();
                effectsUI.RemoveAt(i);
                RepositionEffects();
                effects.RemoveAt(i);
            }
            else ProgressAnimation (effectsUI[i], effects[i]);
        }
    }

    private void ProgressAnimation(GameObject effUI, Effect eff)
    {
        Transform tint = effUI.transform.GetChild(0);
        float value = eff.timeLeft/eff.duration;
        Debug.Log(value);
        tint.GetComponent<Image>().fillAmount = 1 - value;
        Canvas.ForceUpdateCanvases();
    }

    private void RepositionEffects()
    {
        for(int i = 0; i < effectsUI.Count; i++)
        {
            effectsUI[i].GetComponent<RectTransform>().anchoredPosition = new Vector3
            (70 + (effectUI.GetComponent<RectTransform>().rect.width + 10)*i,-70);
        }
    }
    private void OnDestroy()
    {
        effects.Clear();
        effectsUI.Clear();
    }
}
