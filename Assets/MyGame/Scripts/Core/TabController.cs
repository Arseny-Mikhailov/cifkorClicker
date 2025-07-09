using System.Collections.Generic;
using System.Linq;

namespace MyGame.Scripts
{
    public class TabController
    {
        private readonly Dictionary<TabType, ITab> _tabs;
        private ITab _currentTab;

        public TabController(List<ITab> tabs)
        {
            _tabs = tabs.ToDictionary(t => t.TabType);
        }

        public void SwitchTo(TabType tabType)
        {
            if (_currentTab?.TabType == tabType) return;

            _currentTab?.OnHide();
            _currentTab = _tabs[tabType];
            _currentTab.OnShow();
        }
    }
}