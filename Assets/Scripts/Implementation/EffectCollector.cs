using UnityEngine;

public class EffectCollector : MonoBehaviour
{
    [SerializeField] private Effect effect;
    private EffectStorage effectStorage;

    private void Start()
    {
        effectStorage = FindAnyObjectByType<EffectStorage>().GetComponent<EffectStorage>();
        
    }
    private void OnMouseDown()
    {
        effectStorage.ApplyEffect(effect);
        Destroy(this.gameObject);    
    }
}
