using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace MinesweeperFramework
{
    public partial class MinesweeperWindow
    {
        private static ValuePattern _timeValue;
        public int Time { get { return int.Parse(_timeValue.Current.Value); } }

        private static ValuePattern _minesRemainingValue;
        public int Mines_Remaining { get { return int.Parse(_minesRemainingValue.Current.Value); } }

        private void GetStatusInformationElements(AutomationElement element)
        {
            AutomationElement tmp = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Time"));
            _timeValue = tmp.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;

            tmp = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Mines Remaining"));
            _minesRemainingValue = tmp.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
        }
    }
}
