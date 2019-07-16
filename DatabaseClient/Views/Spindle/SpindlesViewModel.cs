using DatabaseClient.EntityData;
using DatabaseClient.Messages;
using GalaSoft.MvvmLight.Messaging;
using Support;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace DatabaseClient
{
    public class SpindlesViewModel : CrudVMBase
    {
        public SpindleVM SelectedSpindle { get; set; }
        public ObservableCollection<SpindleVM> Spindles { get; set; }
        public SpindlesViewModel() : base()
        {

        }
        protected override void CommitUpdates()
        {
            UserMessage msg = new UserMessage();
            var inserted = (from s in Spindles
                            where s.IsNew
                            select s).ToList();
            if (db.ChangeTracker.HasChanges() || inserted.Count > 0)
            {
                foreach (SpindleVM s in inserted)
                {
                    db.spindle.Add(s.TheSpindle);
                }
                try
                {
                    db.SaveChanges();
                    msg.Message = "Database Updated";
                }
                catch (Exception e)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        ErrorMessage = e.InnerException.GetBaseException().ToString();
                    msg.Message = "There was a problem updating the database";
                }
            }
            else
                msg.Message = "No changes to save";
            Messenger.Default.Send<UserMessage>(msg);
        }

        protected override void DeleteCurrent()
        {
            UserMessage msg = new UserMessage();
            if (SelectedSpindle != null)
            {
                db.spindle.Remove(SelectedSpindle.TheSpindle);
                Spindles.Remove(SelectedSpindle);
                RaisePropertyChanged("Spindles");
                msg.Message = "Deleted";
            }
            else
                msg.Message = "No Spindle selected to delete";
            Messenger.Default.Send<UserMessage>(msg);
        }

        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;
            ObservableCollection<SpindleVM> _spindles = new ObservableCollection<SpindleVM>();
            var spindles = await (from s in db.spindle
                                  orderby s.model
                                  select s).ToListAsync();
            foreach (spindle spin in spindles)
            {
                _spindles.Add(new SpindleVM { IsNew = false, TheSpindle = spin });
            }
            Spindles = _spindles;
            RaisePropertyChanged("Spindles");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
