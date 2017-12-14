﻿using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ControlClothing : MonoBehaviour
    {
        public Image[] equipImgs;
        private bool isOpen;

        private void Start()
        {
            isOpen = false;
            ToggleEquipMenu(false);
        }

        public void RolloutEquipmentMenu()
        {
            SoundManager.Play("Click01");

            if (isOpen)
            {
                ToggleEquipMenu(false);
            }
            else
            {
                ToggleEquipMenu(true);
            }
        }

        private void ToggleEquipMenu(bool isOn)
        {
            isOpen = isOn;
            if (isOn)
            {
                //Adjusting the alpha to hide the slots as the enabled state is handled
                //by other components. Raycast target is also adjusted based on on or off
                for (var i = 0; i < equipImgs.Length; i++)
                {
                    var tempCol = equipImgs[i].color;
                    tempCol.a = 1f;
                    equipImgs[i].color = tempCol;
                    equipImgs[i].raycastTarget = true;
                }
            }
            else
            {
                for (var i = 0; i < equipImgs.Length; i++)
                {
                    var tempCol = equipImgs[i].color;
                    tempCol.a = 0f;
                    equipImgs[i].color = tempCol;
                    equipImgs[i].raycastTarget = false;
                }
            }
        }
    }
}