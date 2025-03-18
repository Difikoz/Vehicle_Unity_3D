using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class StatusBarUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _pages = new();

        private int _currentPageIndex;
        private JournalBarUI _journalBar;
        private FactionsBarUI _factionsBar;
        private MapBarUI _mapBar;

        public JournalBarUI JournalBar => _journalBar;
        public FactionsBarUI FactionsBar => _factionsBar;
        public MapBarUI MapBar => _mapBar;

        public void Initialize()
        {
            _journalBar = GetComponentInChildren<JournalBarUI>();
            _factionsBar = GetComponentInChildren<FactionsBarUI>();
            _mapBar = GetComponentInChildren<MapBarUI>();
            _journalBar.Initialize();
            _factionsBar.Initialize();
            _mapBar.Initialize();
        }

        public void Enable()
        {
            _journalBar.Enable();
            _factionsBar.Enable();
            _mapBar.Enable();
            ShowTab(0);
            gameObject.SetActive(false);
        }

        public void Disable()
        {
            _journalBar.Disable();
            _factionsBar.Disable();
            _mapBar.Disable();
        }

        public void PreviousTab()
        {
            if (_currentPageIndex > 0)
            {
                ShowTab(_currentPageIndex - 1);
            }
            else
            {
                ShowTab(_pages.Count - 1);
            }
        }

        public void NextTab()
        {
            if (_currentPageIndex < _pages.Count - 1)
            {
                ShowTab(_currentPageIndex + 1);
            }
            else
            {
                ShowTab(0);
            }
        }

        public void ShowTab(int index)
        {
            _currentPageIndex = index;
            for (int i = 0; i < _pages.Count; i++)
            {
                _pages[i].SetActive(i == _currentPageIndex);
            }
        }
    }
}