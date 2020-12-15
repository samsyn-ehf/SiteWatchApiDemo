
namespace SiteWatchApiDemo
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExcute = null)
        {
            this.execute = execute;
            this.canExecute = canExcute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void DoCanExecuteChanged()
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                this.CanExecuteChanged?.Invoke(this, new EventArgs());
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            this.CanExecuteChanged?.Invoke(this, new EventArgs());

                        }));
            }
        }
    }
}
