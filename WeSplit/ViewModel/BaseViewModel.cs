using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WeSplit.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
		protected virtual bool OnPropertyChanged<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(backingField, value))
				return false;

			backingField = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}

	class RelayCommand<T> : ICommand
	{
		private readonly Predicate<T> _canExecute;
		private readonly Action<T> _execute;

		public RelayCommand(Predicate<T> canExecute, Action<T> execute)
		{
            _canExecute = canExecute;
			_execute = execute ?? throw new ArgumentNullException("execute");
		}

		public bool CanExecute(object parameter)
		{
			try
			{
				return _canExecute == null || _canExecute((T)parameter);
			}
			catch
			{
				return true;
			}
		}

		public void Execute(object parameter)
		{
			_execute((T)parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}
