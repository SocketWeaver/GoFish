using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GoFish
{
    [Serializable]
    public class CardSelectedEvent : UnityEvent<Card>
    {
    }

    public class MouseActions : MonoBehaviour
    {
        public CardSelectedEvent OnCardSelected = new CardSelectedEvent();

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Card card = MouseOverCard();

                if (card != null)
                {
                    OnCardSelected.Invoke(card);
                }
            }
        }

        Card MouseOverCard()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                Card card = hit.transform.gameObject.GetComponent<Card>();
                if (card != null)
                {
                    return card;
                }
            }

            return null;
        }
    }
}
