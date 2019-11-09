using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.Model
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public event Action Event;

        private bool _isCanExecute;

        public bool isCanExecute
        {
            get => _isCanExecute;
            set
            {
                _isCanExecute = value;
                CanExecuteChanged?.Invoke(value, new EventArgs());
            }
        }


        public bool CanExecute(object parameter)
        {
            return isCanExecute;
        }

        public void Execute(object parameter)
        {
            Event?.Invoke();
        }

        public Command(Action action = null, bool canExecute = true)
        {
            Event = action;
            isCanExecute = canExecute;
        }
    }
}
