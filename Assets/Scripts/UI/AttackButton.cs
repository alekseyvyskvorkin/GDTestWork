using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

namespace TestWork.UI
{
    [Serializable]
    public class AttackButton
    {
        public Button Button => _button;

        [SerializeField] private Button _button;
        [SerializeField] private Image _fillingImage;
        [SerializeField] private TMP_Text _timerText;

        private double _timer;

        public async void OnClick(Attack attack)
        {
            float time = attack.AttackSettings.Cooldown;

            _fillingImage.DOKill();
            _fillingImage.fillAmount = 0;
            _fillingImage.DOFillAmount(1, time).SetEase(Ease.Linear);

            _timerText.text = time.ToString();
            _timer = time;

            while (_timer >= 0)
            {
                _timerText.text = Math.Round(_timer, 1).ToString();
                _timer -= Time.deltaTime;
                await UniTask.Yield();
            }

            if (!Button.interactable)
            {
                _fillingImage.DOKill();
                _fillingImage.fillAmount = 0;
            }

            _timerText.text = "";
        }

        public void ActivateButton(bool value)
        {
            if (_timer <= 0)
            {
                _fillingImage.fillAmount = value ? 1 : 0;
            }

            Button.interactable = value;
        }
    }
}

