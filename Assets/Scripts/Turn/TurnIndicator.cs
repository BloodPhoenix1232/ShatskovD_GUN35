using Controllers;
using primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Interfaces
{
    public class TurnIndicator : MonoBehaviour
    {
        private ITurn _turn;        //Injected

        private (Team team, Image icon, Image arrow) _left;
        private (Team team, Image icon, Image arrow) _right;


        [SerializeField]
        private Image _leftIcon;
        [SerializeField]
        private Image _leftArrow;

        [SerializeField, Space(15f)]
        private Image _rightIcon;
        [SerializeField]
        private Image _rightArrow;

        [SerializeField, Space(15f), Range(.1f, 2f)]
        private float _disableScale = .7f;
        [SerializeField, Range(0f, 1f)]
        private float _disableAlpha = .3f;

        private void CallBack(GameStatus status)
        {
            if (status is not GameStatus.Unlock) return;

            _turn.Next();
            var (enable, disable) = _leftIcon == _turn.Current
                ? (_left, _right)
                : (_right, _left);

            enable.arrow.enabled = true;
            enable.icon.transform.localScale = Vector3.one;
            enable.icon.color = new Color(1f, 1f, 1f, 1f);

            disable.arrow.enabled = false;
            disable.icon.transform.localScale = Vector3.one;    
            disable.icon.color = new Color(1f, 1f, 1f, _disableAlpha);
        }

    }
}