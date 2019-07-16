using GalaSoft.MvvmLight.Command;
using Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DatabaseClient
{
    public class BaseEntity : NotifyUIBase, INotifyDataErrorInfo
    {
        private RelayCommand<PropertyError> conversionErrorCommand;

        public RelayCommand<PropertyError> ConversionErrorCommand
        {
            get
            {
                return conversionErrorCommand
                    ?? (conversionErrorCommand = new RelayCommand<PropertyError>
                    (PropertyError =>
                    {
                        if (PropertyError.Added)
                        {
                            AddError(PropertyError.PropertyName, PropertyError.Error, ErrorSource.Conversion);
                        }
                        FlattenErrorList();
                    }));
            }
        }

        private RelayCommand<string> sourceUpdatedCommand;
        public RelayCommand<string> SourceUpdatedCommand
        {
            get
            {
                return sourceUpdatedCommand
                    ?? (sourceUpdatedCommand = new RelayCommand<string>
                        (Property =>
                        {
                            ValidateProperty(Property);
                        }));
            }
        }

        private ObservableCollection<PropertyError> errorList = new ObservableCollection<PropertyError>();
        public ObservableCollection<PropertyError> ErrorList
        {
            get
            {
                return errorList;
            }
            set
            {
                errorList = value;
                RaisePropertyChanged();
            }
        }

        protected Dictionary<string, List<AnError>> errors = new Dictionary<string, List<AnError>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                return null;
            }
            if (errors.ContainsKey(property) && errors[property] != null && errors[property].Count > 0)
            {
                return errors[property].Select(x => x.Text).ToList();
            }
            return null;
        }
        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }
        public void NotifyErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
        public bool IsValid()
        {
            RemoveConversionErrorsOnly();

            var vContext = new ValidationContext(this, null, null);
            List<ValidationResult> vResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, vContext, vResults, true);
            TransformErrors(vResults);
            var propNames = errors.Keys.ToList();
            propNames.ForEach(pn => NotifyErrorsChanged(pn));
            NotifyPropertiesInError();
            FlattenErrorList();

            if (propNames.Count > 0)
            {
                return false;
            }
            return true;
        }
        private void RemoveConversionErrorsOnly()
        {
            foreach (KeyValuePair<string, List<AnError>> pair in errors)
            {
                List<AnError> _list = pair.Value;
                _list.RemoveAll(x => x.Source == ErrorSource.Validation);
            }

            var removeProps = errors.Where(x => x.Value.Count == 0)
                .Select(x => x.Key)
                .ToList();
            foreach (string key in removeProps)
            {
                errors.Remove(key);
            }
        }
        public void ValidateProperty(string propertyName)
        {
            errors.Remove(propertyName);

            var vContext = new ValidationContext(this, null, null);
            vContext.MemberName = propertyName;
            List<ValidationResult> vResults = new List<ValidationResult>();
            Validator.TryValidateProperty(this.GetType().GetProperty(propertyName).GetValue(this, null), vContext, vResults);

            TransformErrors(vResults);
            FlattenErrorList();

            NotifyErrorsChanged(propertyName);
        }
        private void TransformErrors(List<ValidationResult> results)
        {
            foreach (ValidationResult r in results)
            {
                foreach (string ppty in r.MemberNames)
                {
                    AddError(ppty, r.ErrorMessage, ErrorSource.Validation);
                }
            }
        }
        private void AddError(string ppty, string err, ErrorSource source)
        {
            List<AnError> _list;
            if (!errors.TryGetValue(ppty, out _list))
            {
                errors.Add(ppty, _list = new List<AnError>());
            }
            if (!_list.Any(x => x.Text == err))
            {
                _list.Add(new AnError { Text = err, Source = source });
            }
        }
        private void FlattenErrorList()
        {
            ObservableCollection<PropertyError> _errorList = new ObservableCollection<PropertyError>();
            foreach (var prop in errors.Keys)
            {
                List<AnError> _errs = errors[prop];
                foreach (AnError err in _errs)
                {
                    _errorList.Add(new PropertyError { PropertyName = prop, Error = err.Text });
                }
            }
            ErrorList = _errorList;
        }
        private void NotifyPropertiesInError()
        {
            foreach (var prop in errors.Keys)
            {
                NotifyErrorsChanged(prop);
            }
        }
        public void ClearErrors()
        {
            List<string> oldErrorProperties = errors.Select(x => x.Key.ToString()).ToList();
            errors.Clear();
            ErrorList.Clear();
            foreach (var p in oldErrorProperties)
            {
                NotifyErrorsChanged(p);
            }
            NotifyErrorsChanged("");
        }
    }
}
