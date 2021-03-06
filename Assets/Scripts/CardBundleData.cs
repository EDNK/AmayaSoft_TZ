using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New CardBundleData", menuName="Card Bundle Data", order=1)]
public class CardBundleData : ScriptableObject
{
   [SerializeField] private CardData[] cardData;
   public CardData[] CardData => cardData;
}
