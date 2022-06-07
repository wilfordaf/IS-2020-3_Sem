using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Models
{
    public class TimeManager
    {
        private static TimeManager _timeManager;
        private readonly ICentralBank _centralBank;
        private int _daysPassed;

        private TimeManager(ICentralBank centralBank)
        {
            _centralBank = centralBank;
        }

        public static TimeManager GetTimeManager(ICentralBank centralBank)
        {
            return _timeManager ??= new TimeManager(centralBank);
        }

        public void DissipateDays(int numberOfDays)
        {
            for (int i = 0; i < numberOfDays; i++)
            {
                _daysPassed++;
                _centralBank.NotifyBanksDayPassed();

                if (_daysPassed % Config.DaysInMonth == 0)
                {
                    _centralBank.NotifyBanksMonthPassed();
                }
            }
        }

        public void DissipateMonths(int numberOfMonths)
        {
            DissipateDays(numberOfMonths * Config.DaysInMonth);
        }
    }
}