// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyPropertyChanged.cs" company="Samsyn">
//   Samsyn
// </copyright>
// <summary>
//   The NotifyPropertyChanged.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SiteWatchApiDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The notify property changed.
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The on property change.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void OnPropertyChange([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The on another property change.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void OnAnotherPropertyChange(string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Update Field and update property change
        /// </summary>
        /// <param name="field">value to update</param>
        /// <param name="value">input value</param>
        /// <param name="propertyName">propert name</param>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>was updated</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            this.OnAnotherPropertyChange(propertyName);
            return true;
        }

        #endregion
    }
}
