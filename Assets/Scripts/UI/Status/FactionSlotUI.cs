using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WinterUniverse
{
    public class FactionSlotUI : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        [SerializeField] private Button _thisButton;
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Color _enemyStateColor = Color.red;
        [SerializeField] private Color _neutralStateColor = Color.yellow;
        [SerializeField] private Color _allyStateColor = Color.green;

        private FactionRelationship _relationship;

        public void Initialize(FactionRelationship relationship)
        {
            _relationship = relationship;
            _infoText.text = $"{_relationship.Faction.DisplayName}: {_relationship.State}";
            switch (_relationship.State)
            {
                case RelationshipState.Enemy:
                    _backgroundImage.color = _enemyStateColor;
                    break;
                case RelationshipState.Neutral:
                    _backgroundImage.color = _neutralStateColor;
                    break;
                case RelationshipState.Ally:
                    _backgroundImage.color = _allyStateColor;
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _thisButton.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            GameManager.StaticInstance.UIManager.StatusBar.FactionsBar.ShowFullInformation(_relationship);
        }
    }
}