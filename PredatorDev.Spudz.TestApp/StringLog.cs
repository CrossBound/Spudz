using System;
using System.ComponentModel;
using System.Text;

namespace PredatorDev.Spudz.TestApp
{
    public class StringLog : INotifyPropertyChanged
    {
        /*
            REFERENCE CREDITS: 
            https://msdn.microsoft.com/en-us/library/ms743643(v=vs.100).aspx#classes
            https://msdn.microsoft.com/en-us/library/ms743695(v=vs.100).aspx
        */
        private StringBuilder _builder = new StringBuilder();
        public string Content => _builder.ToString();
        public event PropertyChangedEventHandler PropertyChanged;

        public void Append(string Text)
        {
            _builder.Insert(0, Text);
            OnPropertyChanged("Content");
        }

        public void AppendLine(string Text)
        {
            _builder.Insert(0, Environment.NewLine);
            _builder.Insert(0, Text);
            OnPropertyChanged("Content");
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
