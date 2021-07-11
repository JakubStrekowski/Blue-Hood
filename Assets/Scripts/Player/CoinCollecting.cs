using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollecting : MonoBehaviour
{
    private int _coinAmount;
    public int CoinAmount { get => _coinAmount; }

    public delegate void OnCoinChangeDelegate();
    public event OnCoinChangeDelegate OnCoinChange;

    public void CollectedCoin()
    {
        _coinAmount++;
        OnCoinChange?.Invoke();
    }
}
