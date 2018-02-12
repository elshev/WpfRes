using Common.ComponentModel;

namespace ResApp.Entities
{
    public class EntityBase : NotifyPropertyChangedBase
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                RaisePropertyChanged(() => Id);
            }
        }
        
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }
    }
}