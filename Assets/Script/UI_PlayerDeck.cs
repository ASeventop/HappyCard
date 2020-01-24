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
    List<Vector2> locations = new List<Vector2>();
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
    Deck deck = new Deck();
    [Header("Rule")]
    [SerializeField]Image img_rule;
    [SerializeField] Sprite sprite_rule,sprite_notrule;
    bool isLock =false;

    public void ShowCardFormDeck(byte[] _cards)
    {
        isLock = false;
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
        foreach (var item in cardObjects)
        {
            locations.Add(item.transform.position);
        }
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
    public void DeckUpdate(CT_PlayerDeckUpdate deckUpdate){
        UpdateRule(deckUpdate.isRule);
        var swapCard = deckUpdate.swapCard;
        var indexFrom = swapCard[0];
        var indexTo = swapCard[1];

        var objectFrom = cardObjects[indexFrom];
        var objectTo = cardObjects[indexTo];

        objectFrom.transform.DOKill();
        objectTo.transform.DOKill();

        objectFrom.transform.DOMove(locations[indexTo],0.2f);
        objectTo.transform.DOMove(locations[indexFrom],0.2f);

        var buffer = cardObjects[indexFrom];
        cardObjects[indexFrom] = cardObjects[indexTo];
        cardObjects[indexTo] = buffer;
    }
    void OnCardCollider(Collider2D collider)
    {
        if(isLock)return;
        if (collider.gameObject == selectObject) return;
        objectCollider.Add(collider.gameObject);
        swapCard = collider.gameObject;
        readySwapCard = true;
    }
    void OnCardExitCollider(Collider2D collider)
    {
        if(isLock)return;
        objectCollider.Remove(collider.gameObject);
        swapCard = null;
        readySwapCard = false;
    }
    void OnCardSelect(PointerEventData eventData, ObservableEventTrigger trigger)
    {
        if(isLock)return;
        selectObject = trigger.gameObject;
        var indexObject = cardObjects.IndexOf(selectObject);
        positionFrom = locations[indexObject];
        selectObject.transform.SetAsLastSibling();
    }
    void OnCardDeselect(PointerEventData eventData, ObservableEventTrigger trigger)
    {
        if(isLock)return;
        if(objectCollider.Count <= 0)
        {
            selectObject.transform.DOMove(positionFrom, 0.2f);
        }
        else
        {
            swapCard = objectCollider[objectCollider.Count - 1];
            //positionTo = swapCard.transform.position;
            int fromIndex = cardObjects.IndexOf(selectObject);
            int toIndex = cardObjects.IndexOf(swapCard);
            CardManager.Instance.SwapCard(fromIndex, toIndex);
            selectObject.transform.DOMove(positionFrom, 0.2f).SetDelay(3);
        }
       // swap index of card
        objectCollider.Clear();
        swapCard = null;
        selectObject = null;
    }
    void OnCardMove(PointerEventData eventData, ObservableEventTrigger trigger)
    {
        if(isLock)return;
        selectObject.transform.position = eventData.position;
    }
    public void SetTimer(float _timer)
    {
        if(_timer<=0)return;
        timer = _timer;
        img_timeReduce.fillAmount = _timer / 60;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        time_txt.text = new DateTime(timeSpan.Ticks).ToString("mm:ss");
    }
    public void DeckConfirm()
    {
        isLock = true;
        b_confirm.gameObject.SetActive(false);
    }
    public void StartGame(bool isStart)
    {
      //  b_confirm.gameObject.SetActive(isStart);
    }
    public void UpdateRule(bool rule){
        img_rule.sprite = rule ? sprite_rule : sprite_notrule;
        b_confirm.gameObject.SetActive(rule);
    }

}
