using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.Krachylo.Lab2.Models
{
    internal class Person
    {
        private static readonly string[] animals = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }


        public bool IsAdult { get; private set; }
        public string SunSign { get; private set; }
        public string ChineseSign { get; private set; }
        public bool IsBirthday { get; private set; }

        public Person(string name, string surname, string email, DateTime birthDate)
        {
            Name = name;
            Surname = surname;
            Email = email;
            BirthDate = birthDate;

            CalculateProperties();
        }

        public Person(string name, string surname, string email) : this(name, surname, email, DateTime.MinValue) { }

        public Person(string name, string surname, DateTime birthDate) : this(name, surname, string.Empty, birthDate) { }

        private void CalculateProperties()
        {
            var dateNow = DateTime.Now;
            int age = dateNow.Year - BirthDate.Year;
            if (dateNow.DayOfYear - BirthDate.DayOfYear < 0)
                age--;

            IsAdult = age > 18;
            IsBirthday = dateNow.Month == BirthDate.Month && dateNow.Day == BirthDate.Day;
            SunSign = GetSunSign();
            ChineseSign = GetChineseSign();
        }

        private string GetSunSign()
        {
            int day = BirthDate.Day;
            switch (BirthDate.Month)
            {
                case 1:
                    if (day <= 19)
                        return "Capricorn";
                    return "Aquarius";
                case 2:
                    if (day <= 18)
                        return "Aquarius";
                    return "Pisces";
                case 3:
                    if (day <= 20)
                        return "Pisces";
                    return "Aries";
                case 4:
                    if (day <= 19)
                        return "Aries";
                    return "Taurus";
                case 5:
                    if (day <= 20)
                        return "Taurus";
                    return "Gemini";
                case 6:
                    if (day <= 20)
                        return "Gemini";
                    return "Cancer";
                case 7:
                    if (day <= 22)
                        return "Cancer";
                    return "Leo";
                case 8:
                    if (day <= 22)
                        return "Leo";
                    return "Virgo";
                case 9:
                    if (day <= 22)
                        return "Virgo";
                    return "Libra";
                case 10:
                    if (day <= 22)
                        return "Libra";
                    return "Scorpio";
                case 11:
                    if (day <= 21)
                        return "Scorpio";
                    return "Saggitarius";
                case 12:
                    if (day <= 21)
                        return "Saggitarius";
                    return "Capricorn";
                default:
                    throw new ArgumentOutOfRangeException($"Invalid month {BirthDate.Month} provided");
            }
        }

        private string GetChineseSign()
        {
            return animals[(BirthDate.Year - 4) % 12];
        }
    }
}
