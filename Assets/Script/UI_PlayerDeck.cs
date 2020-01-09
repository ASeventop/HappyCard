using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;
using UniRx.Triggers;
using DG.Tweening;
using TMPro;
using System;
public class UI_PlayerDeck : MonoBehaviour
{
    public List<GameObject> cardObjects;
    public List<ObservableEventTrigger> eventTriggers;
    public List<ObservableCollision2DTrigger> collisionTriggers;
    public List<GameObject> objectCollider;
    public Vector3 positionFrom;
    public Vector3 positionTo;
    public GameObject selectObject,swapCard;
    public bool readySwapCard = false;
    [SerializeField] Button b_confirm;
    [Header("Timer")]
    float timer;
    public Image img_timeReduce;
    public TextMeshProUGUI time_txt;
    Deck deck;
    public void ShowCardFormDeck(byte[] _cards)
    {
        deck = new Deck();
        CardManager.Instance.Init();
        for (int i = 0; i < cardObjects.Count; i++)
        {
            cardObjects[i].GetComponent<Image>().sprite = AssetManager.GetSprite(_cards[i].ToString());
            CardManager.Instance.AddToRow(deck.GetCardByID(_cards[i]));
        }
    }
    void CardTrigger(Collision2D coll)
    {
        Debug.Log("Collision2D " + coll.gameObject.name);
    }
    private void Start()
    {
       
        foreach (var item in eventTriggers)
        {
            item.OnPointerDownAsObservable().Subscribe(_ => OnCardSelect(_, item));
            item.OnPointerUpAsObservable().Subscribe(_ => OnCardDeselect(_, item));
            item.OnDragAsObservable().Subscribe(_ => OnCardMove(_, item));
           
        }
        foreach (var item in collisionTriggers)
        {
            item.OnTriggerEnter2DAsObservable().Subscribe(_ => OnCardCollider(_));
            item.OnTriggerExit2DAsObservable().Subscribe(_ => OnCardExitCollider(_));
        }
        b_confirm.onClick.AsObservable().Subscribe(_ =>
        {
            PhotonMessage.DeckUpdateEnd();
        });
    }
    void OnCardCollider(Collider2D collider)
    {
        if (collider.gameObject == selectObject) return;
        objectCollider.Add(collider.gameObject);
        swapCard = collider.gameObject;
        readySwapCard = true;
    }
    void OnCardExitCollider(Collider2D collider)
    {
        objectCollider.Remove(collider.gameObject);
        swapCard = null;
        readySwapCard = false;
    }
    void OnCardSelect(PointerEventData eventData, ObservableEventTrigger trigger)
    {
        selectObject = trigger.gameObject;
        positionFrom = selectObject.transform.position;
        selectObject.transform.SetAsLastSibling();
    }
    void OnCardDeselect(PointerEventData eventData, ObservableEventTrigger trigger)
    {
       if(objectCollider.Count <= 0)
        {
            selectObject.transform.DOMove(positionFrom, 0.2f);
        }
        else
        {
            swapCard = objectCollider[objectCollider.Count - 1];
            positionTo = swapCard.transform.position;
            int indSelect = cardObjects.IndexOf(selectObject);
            int toIndex = cardObjects.IndexOf(swapCard);

            cardObjects[indSelect] = swapCard;
            cardObjects[toIndex] = selectObject;
            CardManager.Instance.SwapCard(indSelect, toIndex);
            swapCard.transform.DOMove(positionFrom, 0.2f);
            selectObject.transform.DOMove(positionTo, 0.2f);
        }
       // swap index of card
        objectCollider.Clear();
        swapCard = null;
        selectObject = null;
    }
    void OnCardMove(PointerEventData eventData, ObservableEventTrigger trigger)
    {
        selectObject.transform.position = eventData.position;
    }
    public void SetTimer(float _timer)
    {
        timer = _timer;
        img_timeReduce.fillAmount = _timer / 60;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        time_txt.text = new DateTime(timeSpan.Ticks).ToString("mm:ss");
    }
    public void DeckConfirm()
    {
        b_confirm.gameObject.SetActive(false);
    }
    public void StartGame(bool isStart)
    {
        b_confirm.gameObject.SetActive(isStart);
    }

}
