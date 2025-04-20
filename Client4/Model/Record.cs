using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client1.Model
{
    public class Record : INotifyPropertyChanged
    {
        private string _vehicleNum;
        public string VehicleNum
        {
            get => _vehicleNum;
            set
            {
                _vehicleNum = value;
                OnPropertyChanged(nameof(VehicleNum));
            }
        }
        private string _entryDate;
        public string EntryDate
        {
            get => _entryDate;
            set
            {
                _entryDate = value;
                OnPropertyChanged(nameof(EntryDate));
            }
        }
        private string _exitDate;
        public string ExitDate
        {
            get => _exitDate;
            set
            {
                _exitDate = value;
                OnPropertyChanged(nameof(ExitDate));
            }
        }
        private string _parkingTime;
        public string ParkingTime
        {
            get => _parkingTime;
            set
            {
                _parkingTime = value;
                OnPropertyChanged(nameof(ParkingTime));
            }
        }
        private string _totalFee;
        public string TotalFee
        {
            get => _totalFee;
            set
            {
                _totalFee = value;
                OnPropertyChanged(nameof(TotalFee));
            }
        }
        private byte _classification;
        public byte Classification
        {
            get => _classification;
            set
            {
                _classification = value;
                OnPropertyChanged(nameof(Classification));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
