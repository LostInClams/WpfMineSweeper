using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MineSweeper
{
    class ActionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        readonly Action m_action;

        public ActionCommand(Action action)
        {
            m_action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            m_action?.Invoke();
        }
    }
}
