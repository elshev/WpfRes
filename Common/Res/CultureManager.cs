using System;
using System.Globalization;
using System.Threading;
using Common.ComponentModel;

namespace Common.Res
{
    /// <summary>
    /// Wrapper around Thread.CurrentThread.CurrentUICulture.
    /// </summary>
    public class CultureManager : NotifyPropertyChangedBase
    {
        private CultureManager()
        {
        }

        static CultureManager()
        {
        }

        private static readonly CultureManager instance = new CultureManager();
        public static CultureManager Instance { get { return instance; } }

        private CultureInfo uiCulture;

        private bool synchronizeThreadCulture = true;

        public event EventHandler UICultureChanged;

        public CultureInfo UICulture
        {
            get { return uiCulture ?? (uiCulture = Thread.CurrentThread.CurrentUICulture); }
            set
            {
                if (value == UICulture) return;
                uiCulture = value;
                Thread.CurrentThread.CurrentUICulture = value;
                if (SynchronizeThreadCulture)
                {
                    SetThreadCulture(value);
                }
                if (UICultureChanged != null)
                {
                    UICultureChanged(null, EventArgs.Empty);
                }
                RaisePropertyChanged(() => UICulture);
            }
        }

        public bool SynchronizeThreadCulture
        {
            get { return synchronizeThreadCulture; }
            set
            {
                if (synchronizeThreadCulture == value) return;

                synchronizeThreadCulture = value;
                if (value)
                {
                    SetThreadCulture(UICulture);
                }
                RaisePropertyChanged(() => SynchronizeThreadCulture);
            }
        }

        private void SetThreadCulture(CultureInfo value)
        {
            Thread.CurrentThread.CurrentCulture = value.IsNeutralCulture ? CultureInfo.CreateSpecificCulture(value.Name) : value;
        }
    }
}