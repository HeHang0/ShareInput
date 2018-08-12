using System.ComponentModel;

namespace SharePoint.Input
{
    public class MouseEvent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int MoveToLeft { get; set; }
        public int MoveToRight { get; set; }
        public int MoveToTop { get; set; }
        public int MoveToButtom { get; set; }
        public int MoveToLeftTop { get; set; }
        public int MoveToRightTop { get; set; }
        public int MoveToLeftButtom { get; set; }
        public int MoveToRightButtom { get; set; }
        public int MouseDown { get; set; }
        public string MouseEventStr { get; set; }
    }
}
