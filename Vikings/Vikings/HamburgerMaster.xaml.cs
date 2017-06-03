using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vikings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMaster : ContentPage
    {
        public ListView ListView => ListViewMenuItems;

        public HamburgerMaster()
        {
            InitializeComponent();
            BindingContext = new HamburgerMasterViewModel();
        }

        class HamburgerMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HamburgerMenuItem> MenuItems { get; }
            public HamburgerMasterViewModel()
            {
                MenuItems = new ObservableCollection<HamburgerMenuItem>(new[]
                {
                    new HamburgerMenuItem { Id = 0, Title = "My Team", TargetType = typeof(MyTeamPage)},              
                    new HamburgerMenuItem { Id = 1, Title = "Position My Team", TargetType = typeof(PositionPage)},
                    new HamburgerMenuItem { Id = 2, Title = "Recaps" , TargetType = typeof (RecapPage)},
                    new HamburgerMenuItem { Id = 3, Title = "About Page", TargetType = typeof(AboutPage)},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            #endregion
        }
    }
}