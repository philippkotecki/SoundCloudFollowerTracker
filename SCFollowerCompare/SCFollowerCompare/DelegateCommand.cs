﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace SCFollowerCompare
{
    public class DelegateCommand : ICommand
    {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;


        public DelegateCommand(Predicate<object> canExecute, Action<object> execute)
            => (this.canExecute, this.execute) = (canExecute, execute);

        public DelegateCommand(Action<object> execute) : this(null, execute) { }

        //public DelegateCommand(Action<object> execute)
        //    => (this.execute) = (execute);


        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => this.canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => this.execute?.Invoke(parameter);
    }
}
