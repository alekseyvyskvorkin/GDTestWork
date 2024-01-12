using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using TestWork.Units;

namespace TestWork.UI
{
    public class UIService : MonoBehaviour
    {
        private const string WinText = "You Win";
        private const string LoseText = "You Lose";

        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _startPlayButton;
        [SerializeField] private TMP_Text _endGameText;

        [SerializeField] private GameObject _gamePlayPanel;
        [SerializeField] private AttackButton _baseAttackButton;
        [SerializeField] private AttackButton _doubleAttackButton;

        [SerializeField] private TMP_Text _currentWaveText;
        [SerializeField] private TMP_Text _maxWaveCountText;

        private GameManager _gameManager;

        [Inject]
        private void Construct(GameManager gameManager, Player player, Spawner spawner)
        {
            _gameManager = gameManager;
            _gameManager.OnStartGame += HideMenu;
            _gameManager.OnWinGame += ShowWinText;
            _gameManager.OnLoseGame += ShowLoseText;

            _startPlayButton.onClick.AddListener(() => _gameManager.OnStartGame?.Invoke());
            _restartButton.onClick.AddListener(() => _gameManager.OnStartGame?.Invoke());

            InitAttackButton(_baseAttackButton, player, player.Attacks[0]);
            InitAttackButton(_doubleAttackButton, player, player.Attacks[1]);

            player.Attacks[1].OnDistanceCheck += _doubleAttackButton.ActivateButton;

            spawner.OnSpawn += ChangeWaveCountText;
        }

        private void ChangeWaveCountText(int currentWave, int maxWaveCount)
        {
            _currentWaveText.text = "Current wave :" + currentWave.ToString();
            _maxWaveCountText.text = "Wave count :" + maxWaveCount.ToString();
        }

        private void InitAttackButton(AttackButton abilityButton, Player player, Attack ability)
        {
            abilityButton.Button.onClick.AddListener(() =>
            {
                if (player.IsAbilityAnimationCompleted && ability.IsReadyToUse)
                {
                    abilityButton.OnClick(ability);
                    player.Attack(ability);
                }
            });
        }

        private void HideMenu()
        {
            _gamePlayPanel.SetActive(true);
            _menuPanel.SetActive(false);
        }

        private void ShowMenu()
        {
            _gamePlayPanel.SetActive(false);
            _menuPanel.SetActive(true);
            _endGameText.gameObject.SetActive(true);
        }

        private void ShowWinText()
        {
            ShowMenu();
            _endGameText.text = WinText;
        }

        private void ShowLoseText()
        {
            ShowMenu();
            _endGameText.text = LoseText;
        }
    }
}