using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RssReader
{
    class WpfButton : ICommand
    {
        Action executeMethod = null;
        Func<bool> canExecuteMethod = null;
        public WpfButton(Action executeMethod, Func<bool> canExecuteMethod = null)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (canExecuteMethod != null)
            {
                return canExecuteMethod();
            }
            return true;
        }

        public void Execute(object parameter)
        {
            executeMethod?.Invoke();
        }
    }
}
