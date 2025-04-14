using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client3.ViewModel.Commands
{
    public class Command : ICommand
    {
        private Action _execute;

        public Command(Action ViewModel_Method)
        {
            _execute = ViewModel_Method;
        }


        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) //컨트롤 활성화 여부, true지정
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute.Invoke();
        }
    }
}
