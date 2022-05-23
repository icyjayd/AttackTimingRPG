using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.

public class Submenu : MonoBehaviour
{
    public void OnSelect(BaseEventData eventData)
    {
        BattleController.Instance.DeactivateButtons();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
