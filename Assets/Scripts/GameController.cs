using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameController : MonoBehaviour , IPointerClickHandler  {
    [SerializeField] private Image[] HideImages;
    [SerializeField] private Text[] PrizeInfoText;
    private List<PrivatePrizeInfo> _ObjectsCollection;
    private List<PrivatePrizeInfo> _characterList;
    private PrivatePrizeInfo _empty;
    private List<ushort> _gottenCharters;

    private void Start()
    {
        CreateObjectsCollection();
        _empty = new PrivatePrizeInfo  
        {
            PrizeTypeID = 999 // Error ID
        };
        _gottenCharters = new List<ushort>();
        CreateEmptyPrizeInfo();
        GenerateRandomPrizeCardsPool();
    }


    private void CreateObjectsCollection()
    {
        _ObjectsCollection = new List<PrivatePrizeInfo>
        {
            new PrivatePrizeInfo
            {
                PrizeTypeID = 0,
                PrizeName = "Coins",
                CountChance = new List<Vector2>()
                {
                    new Vector2 (50, 100),
                    new Vector2 (100, 90),
                    new Vector2 (150, 80),
                    new Vector2 (200, 70),
                    new Vector2 (250, 60),
                    new Vector2 (300, 50),
                    new Vector2 (350, 40),
                    new Vector2 (400, 30),
                    new Vector2 (450, 20),
                    new Vector2 (500, 10),
                },
                Rarely = 100
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 1,
                PrizeName = "Gems",
                CountChance = new List<Vector2>()
                {
                    new Vector2 (1, 100),
                    new Vector2 (2, 90),
                    new Vector2 (3, 80),
                    new Vector2 (4, 70),
                    new Vector2 (5, 60),
                    new Vector2 (6, 50),
                    new Vector2 (7, 40),
                    new Vector2 (8, 30),
                    new Vector2 (9, 20),
                    new Vector2 (10, 10),
                },
                Rarely = 50
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 2,
                PrizeName = "Energy",
                CountChance = new List<Vector2>()
                {
                    new Vector2 (10, 100),
                    new Vector2 (20, 90),
                    new Vector2 (30, 80),
                    new Vector2 (40, 70),
                    new Vector2 (50, 60),
                    new Vector2 (60, 50),
                    new Vector2 (70, 40),
                    new Vector2 (80, 30),
                    new Vector2 (90, 20),
                    new Vector2 (100, 10),
                },
                Rarely = 70
            },
            new PrivatePrizeInfo
            {
                PrizeTypeID = 3,
                PrizeName = "SpecialMaterial",
                CountChance = new List<Vector2>()
                {
                    new Vector2(1,100)
                },
                Rarely = 25,
            },
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
        var randomValue = UnityEngine.Random.value * 100;
        if (ListOfPrizeInfo.Count <= 0) return _empty;
        var selectPrize = UnityEngine.Random.Range(0, ListOfPrizeInfo.Count - 1);
        var generaitedPrize = _empty;
        var i = 0;
        while (true)
        {
            i++; if (i > 50) return _empty; ;
            selectPrize = UnityEngine.Random.Range(0, ListOfPrizeInfo.Count - 1);
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
            GenerateCurrentCount(generaitedPrize);
            return generaitedPrize;
        }
    }
    private void GenerateCurrentCount(PrivatePrizeInfo Prize)
    {

        var DiceResult = UnityEngine.Random.value;
        if (Prize.IsCharacter) return;
        var RandomCount = UnityEngine.Random.Range(0, Prize.CountChance.Count);
        while (true)
        {
            RandomCount = UnityEngine.Random.Range(0, Prize.CountChance.Count);
            if (Prize.CountChance[RandomCount].y < DiceResult) continue;
            else
            {
                Prize.CurrentCount = (int)Prize.CountChance[RandomCount].x;
                return;
            }
            
        }
    }
    public void GenerateRandomPrizeCardsPool()
    {
        var CardTextField = PrizeInfoText;
        var temporaryGotenCharacters = new List<ushort>();
        var i = 0;
        while (i < CardTextField.Length)
        {
            var RandomazedConsumable = GeneratePrizeCard(_ObjectsCollection);
            if (_gottenCharters.Contains(RandomazedConsumable.PrizeTypeID) ||
                temporaryGotenCharacters.Contains(RandomazedConsumable.PrizeTypeID)
                ) continue;
            if (RandomazedConsumable.IsCharacter)
            {
                temporaryGotenCharacters.Add(RandomazedConsumable.PrizeTypeID);
            }
            SetPrivatePrizeInfoToPrizeInfo(RandomazedConsumable, CardTextField[i].GetComponentInParent<PrizeInfo>());
            if (RandomazedConsumable.IsCharacter)
                CardTextField[i].text = RandomazedConsumable.PrizeName;
            else
                CardTextField[i].text = RandomazedConsumable.CurrentCount + "\n" + RandomazedConsumable.PrizeName;
            i++;
        }
        SetWhiteColorToAllCards(PrizeInfoText);
    }
    private void SetPrivatePrizeInfoToPrizeInfo(PrivatePrizeInfo From, PrizeInfo To)
    {
        if (From.IsCharacter)
        {
            To.PrizeTypeID = From.PrizeTypeID;
            To.PrizeName = From.PrizeName;
            To.IsCharacter = From.IsCharacter;
        }
        else
        {
            To.PrizeTypeID = From.PrizeTypeID;
            To.PrizeName = From.PrizeName;
            To.IsCharacter = From.IsCharacter;
            To.CurrentCount = From.CurrentCount;
        }
    }

    public void CardsVisabilityFalse()
    {
        foreach (Image Image in HideImages)
        {
            Image.gameObject.SetActive(true);
        }
    }
    public void CardVisabilityTrue()
    {
        foreach (Image Image in HideImages)
        {
            Image.gameObject.SetActive(false);
        }
    }

    private void SwitchPrizeInfo(PrizeInfo From, PrizeInfo To)
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
            int j = UnityEngine.Random.Range(0, i + 1);
            var temp = PrizeInfoText[j].text;
            PrizeInfoText[j].text = PrizeInfoText[i].text;
            SwitchPrizeInfo(PrizeInfoText[i].GetComponentInParent<PrizeInfo>(), PrizeInfoText[j].GetComponentInParent<PrizeInfo>());
            PrizeInfoText[i].text = temp;
        }
        SetWhiteColorToAllCards(PrizeInfoText);
        CardsVisabilityFalse();
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
            var prizeInfo = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<PrizeInfo>();
            SaveRevard(prizeInfo);
            if(prizeInfo.IsCharacter == true)
            {
                _gottenCharters.Add(prizeInfo.PrizeTypeID);
            }
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
public class PrivatePrizeInfo : IEquatable<PrivatePrizeInfo>
{
    public ushort PrizeTypeID { get; set; }
    public string PrizeName { get; set; }
    public List<Vector2> CountChance { get; set; }   // CountChance.x = Count CountChance.y = Chance Spawn
    public int CurrentCount { get; set; }
    public bool IsCharacter { get; set; }
    public byte Rarely { get; set; }

    public override string ToString()
    {
        return "ID: " + PrizeTypeID + "   Name: " + PrizeName + "   IsCharacter: "
            + IsCharacter + "   CurrentCount: " + CurrentCount;
    }
    public bool Equals(PrivatePrizeInfo other)
    {
        if (other == null)
            return false;

        if (PrizeName == other.PrizeName && Rarely == other.Rarely
            && CurrentCount == other.CurrentCount && IsCharacter == other.IsCharacter )
            return true;

        else
            return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        PrivatePrizeInfo personObj = obj as PrivatePrizeInfo;
        if (personObj == null)
            return false;
        else
            return Equals(personObj);
    }

    public override int GetHashCode()
    {
        return PrizeTypeID;
    }
}
    public class PrizeInfo : MonoBehaviour {
    public ushort PrizeTypeID;
    public string PrizeName;
    public int CurrentCount;
    public bool IsCharacter;

}
