using System;
using System.Windows.Automation;
using static MinesweeperFramework.MinesweeperWindow.ApplicationMenuBar;

namespace MinesweeperFramework
{
    public partial class MinesweeperWindow
    {
        public GameMenu GameMenu { get; private set; }

        public HelpMenu HelpMenu { get; private set; }

        private void GetApplicationMenuBar(AutomationElement element)
        {
            GameMenu = new GameMenu(element);
        }

        public class ApplicationMenuBar
        {
            public class GameMenu
            {
                private AutomationElement _gameMenuElement;
                private ExpandCollapsePattern _gameMenuPattern;

                public GameMenu(AutomationElement element)
                {
                    _gameMenuElement = getFirstDescendant(element, "Game");
                    _gameMenuPattern = _gameMenuElement.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
                }

                public void NewGame()
                {
                    throw new NotImplementedException();
                }

                public void Statistics()
                {
                    throw new NotImplementedException();
                }

                public void Options()
                {
                    if(_gameMenuPattern.Current.ExpandCollapseState == ExpandCollapseState.Collapsed) _gameMenuPattern.Expand();
                    InvokePattern tmp = getFirstDescendant(_gameMenuElement, "Options").GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                    tmp.Invoke();
                }

                public void ChangeAppearance()
                {
                    throw new NotImplementedException();
                }

                public void Exit()
                {
                    throw new NotImplementedException();
                }
            }

            public class HelpMenu
            {
                private ExpandCollapsePattern _helpMenu;

                public HelpMenu(AutomationElement element)
                {
                    _helpMenu = getFirstDescendant(element, "Help").GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
                }

                public void ViewHelp() { throw new NotImplementedException(); }

                public void AboutMinesweeper() { throw new NotImplementedException(); }

                public void GetMoreGamesOnline() { throw new NotImplementedException(); }
            }
        }
    }
}
