using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour , IPointerClickHandler  {
    [SerializeField] private Image[] HideImages;
    [SerializeField] private Text[] PrizeInfoText;
    private List<PrivatePrizeInfo> _consumablesList;
    private List<PrivatePrizeInfo> _characterList;
    private PrivatePrizeInfo _empty;

    private void Start()
    {
        CreateDictionaryOfObjects();
        _empty = new PrivatePrizeInfo  
        {
            PrizeTypeID = 999 // Error ID
        };
        CreateEmptyPrizeInfo();
        GenerateRandomPrizeCart();
    }


    private void CreateDictionaryOfObjects()
    {
        _consumablesList = new List<PrivatePrizeInfo>
        {
            new PrivatePrizeInfo
            {
                PrizeTypeID = 0,
                PrizeName = "Coins",
                PrizeQuantityInOne = 50,
                MaxFactorInOne = 10,
                Rarely = 100
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 1,
                PrizeName = "Gems",
                PrizeQuantityInOne = 1,
                MaxFactorInOne = 10,
                Rarely = 50
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 2,
                PrizeName = "Energy",
                PrizeQuantityInOne = 10,
                MaxFactorInOne = 10,
                Rarely = 70
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 3,
                PrizeName = "SpecialMaterial",
                PrizeQuantityInOne = 1,
                MaxFactorInOne = 1,
                Rarely = 25
            }
        };
        _characterList = new List<PrivatePrizeInfo>
        {
            new PrivatePrizeInfo
            {
                PrizeTypeID = 101,
                PrizeName ="SuperHero \nVasia",
                IsCharacter = true,
                Rarely = 10
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 102,
                PrizeName ="SuperHero \nPetia",
                IsCharacter = true,
                Rarely = 20
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 103,
                PrizeName ="SuperHero \nPasha",
                IsCharacter = true,
                Rarely = 50
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 104,
                PrizeName ="SuperHero \nKolia",
                IsCharacter = true,
                Rarely = 75
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 105,
                PrizeName ="SuperHero \nSasha",
                IsCharacter = true,
                Rarely = 100
            },
        };
    }
    private void CreateEmptyPrizeInfo()
    {
        foreach (Text text in PrizeInfoText)
        {
            text.gameObject.transform.parent.gameObject.AddComponent<PrizeInfo>();
        }
    }
    private PrivatePrizeInfo GeneratePrizeCard(List<PrivatePrizeInfo> ListOfPrizeInfo)
    {
        var randomValue = Random.value * 100;
        if (ListOfPrizeInfo.Count <= 0) return _empty;
        var selectPrize = Random.Range(0, ListOfPrizeInfo.Count - 1);
        var generaitedPrize = _empty;
        var i = 0;
        while (true)
        {
            i++; if (i > 50) return _empty; ;
            selectPrize = Random.Range(0, ListOfPrizeInfo.Count - 1);
            if (ListOfPrizeInfo[selectPrize].Rarely < randomValue) continue;
            else
            {
                generaitedPrize = ListOfPrizeInfo[selectPrize];
                break;
            }
        }
       if (generaitedPrize.IsCharacter)
        {
            return generaitedPrize;
        }
       else
        {
            generaitedPrize.CurrentCount = generaitedPrize.PrizeQuantityInOne * Random.Range(1, generaitedPrize.MaxFactorInOne);
            return generaitedPrize;
        }
    }

    public void GenerateRandomPrizeCart()
    {
        var CardTextField = PrizeInfoText;
        var curentCardID = 0;
        var characterRandmazed = GeneratePrizeCard(_characterList);
        if (characterRandmazed.PrizeTypeID == 999) { curentCardID = 0; }
        else
        {
            SetPrivatePrizeInfoToPrizeInfo(characterRandmazed, CardTextField[curentCardID].gameObject.GetComponentInParent<PrizeInfo>());
            CardTextField[curentCardID].text = characterRandmazed.PrizeName;
            curentCardID++;
        }
        for (var i = curentCardID; i < CardTextField.Length; i++)
        {
            var RandomazedConsumable = GeneratePrizeCard(_consumablesList);
            SetPrivatePrizeInfoToPrizeInfo(RandomazedConsumable, CardTextField[i].GetComponentInParent<PrizeInfo>());
            CardTextField[i].text = RandomazedConsumable.CurrentCount +"\n" + RandomazedConsumable.PrizeName;
        }
        SetWhiteColorToAllCards(PrizeInfoText);
    }
    private void SetPrivatePrizeInfoToPrizeInfo(PrivatePrizeInfo From, PrizeInfo To)
    {
        if (From.IsCharacter)
        {
            To.PrizeTypeID = From.PrizeTypeID;
            To.PrizeName = From.PrizeName;
        }
        else
        {
            To.PrizeTypeID = From.PrizeTypeID;
            To.PrizeName = From.PrizeName;
            To.CurrentCount = From.CurrentCount;
        }
    }

    private void HideCartVisability()
    {
        foreach (Image Image in HideImages)
        {
            Image.gameObject.SetActive(true);
        }
    }

    private void SwitchPrizeInfo(PrizeInfo From ,PrizeInfo To)
    {
        var temp = gameObject.AddComponent<PrizeInfo>();
            temp.PrizeTypeID = From.PrizeTypeID;
            temp.PrizeName = From.PrizeName;
            temp.IsCharacter = From.IsCharacter;
            temp.CurrentCount = From.CurrentCount;


        From.PrizeTypeID = To.PrizeTypeID;
        From.PrizeName = To.PrizeName;
        From.IsCharacter = To.IsCharacter;
        From.CurrentCount = To.CurrentCount;


        To.PrizeTypeID = temp.PrizeTypeID;
        To.PrizeName = temp.PrizeName;
        To.IsCharacter = temp.IsCharacter;
        To.CurrentCount = temp.CurrentCount;
        To = temp;
        Destroy(temp);
    }

    public void MixRevardCards()
    {

        for (int i = PrizeInfoText.Length - 1; i >= 0; i--)
{
            int j = Random.Range(0, i + 1);
            var temp = PrizeInfoText[j].text;
            PrizeInfoText[j].text = PrizeInfoText[i].text;
            SwitchPrizeInfo(PrizeInfoText[i].GetComponentInParent<PrizeInfo>(), PrizeInfoText[j].GetComponentInParent<PrizeInfo>());
            PrizeInfoText[i].text = temp;
        }
        SetWhiteColorToAllCards(PrizeInfoText);
        HideCartVisability();
    }
    private void SetWhiteColorToAllCards(Text[] CardTextField)
    {
        foreach(Text Text in CardTextField)
        {
            Text.GetComponentInParent<Image>().color = Color.white;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.tag == "Card")
        {
            foreach (Image Image in HideImages)
            {
                Image.gameObject.SetActive(false);
            }
            eventData.pointerPressRaycast.gameObject.transform.parent.GetComponent<Image>().color = Color.green;
            SaveRevard(eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<PrizeInfo>());
        }
    }
    private void SaveRevard(PrizeInfo PrizeInfo)
    {
        if (PrizeInfo.IsCharacter)
        {
          AddCharacterToCollection();
        }
        switch (PrizeInfo.PrizeTypeID)
        {
            case 0:
                {
                    AddCoints(PrizeInfo.CurrentCount);
                    break;
                }
            case 1:
                {
                    AddGems(PrizeInfo.CurrentCount);
                        break;
                }
            case 2:
                {
                    AddEnergy(PrizeInfo.CurrentCount);
                    break;
                }
            case 3:
                {
                    AddSpecialMaterials(PrizeInfo.CurrentCount);
                    break;
                }
        }

    }

    private void AddCoints(int Coints) { }
    private void AddGems(int Gems) { }
    private void AddEnergy(int Energy) { }
    private void AddSpecialMaterials(int Energy) { }
    private void AddCharacterToCollection() { }
}
public class PrivatePrizeInfo
{
    public ushort PrizeTypeID;
    public string PrizeName;
    public ushort PrizeQuantityInOne;
    public ushort MaxFactorInOne;
    public int CurrentCount;
    public bool IsCharacter;
    public byte Rarely;
}
    public class PrizeInfo : MonoBehaviour {
    public ushort PrizeTypeID;
    public string PrizeName;
    public int CurrentCount;
    public bool IsCharacter;

}
