using System;
using UnityEngine;

public class Shrine : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float _levelCount;

    [SerializeField]
    private bool _fullHeal;

    private Vector2Int _chunk;
    private Action<Vector2Int> _onClaimed;

    public void SetChunk(Vector2Int chunk, Action<Vector2Int> onClaimed)
    {
        _chunk = chunk;
        _onClaimed = onClaimed;
    }

    public void OnInteract(Character character)
    {
        character.ReceiveExperience(character.Stats.ExpToLevelUp * 1.2f * _levelCount);

        if(_fullHeal)
        {
            character.FullHeal();
            character.FullMana();
        } 
        else
        {
            character.Heal(20f);
            character.GainMana(20f);
        }

        _onClaimed?.Invoke(_chunk);
        Destroy(gameObject);
    }
}
