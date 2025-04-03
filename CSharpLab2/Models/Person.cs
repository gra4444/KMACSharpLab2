using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KMA.Krachylo.Lab2.Exceptions;

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
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidNameException("Name cannot be empty.");
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                throw new InvalidNameException("Name can only contain letters.");

            if (string.IsNullOrWhiteSpace(surname))
                throw new InvalidNameException("Surname cannot be empty.");
            if (!Regex.IsMatch(surname, @"^[a-zA-Z]+$"))
                throw new InvalidNameException("Surname can only contain letters.");

            if (string.IsNullOrWhiteSpace(email))
                throw new WrongEmailException("Email cannot be empty.");
            if (!IsValidEmail(email))
                throw new WrongEmailException("Invalid email format.");

            var now = DateTime.Now;
            if (birthDate > now)
                throw new FutureBirthDateException("Birth date cannot be in the future.");
            if (now.Year - birthDate.Year >= 135)
                throw new TooOldBirthDateException("Birth date cannot be more than 135 years ago.");


            Name = name;
            Surname = surname;
            Email = email;
            BirthDate = birthDate;

            CalculateProperties();
        }

        public Person(string name, string surname, string email) : this(name, surname, email, DateTime.MinValue) { }

        public Person(string name, string surname, DateTime birthDate) : this(name, surname, string.Empty, birthDate) { }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith("."))
                return false;
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void CalculateProperties()
        {
            var dateNow = DateTime.Now;
            int age = dateNow.Year - BirthDate.Year;
            if (dateNow < BirthDate.AddYears(age))
                age--;
            IsAdult = age >= 18;
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
                    return day <= 19 ? "Capricorn" : "Aquarius";
                case 2:
                    return day <= 18 ? "Aquarius" : "Pisces";
                case 3:
                    return day <= 20 ? "Pisces" : "Aries";
                case 4:
                    return day <= 19 ? "Aries" : "Taurus";
                case 5:
                    return day <= 20 ? "Taurus" : "Gemini";
                case 6:
                    return day <= 20 ? "Gemini" : "Cancer";
                case 7:
                    return day <= 22 ? "Cancer" : "Leo";
                case 8:
                    return day <= 22 ? "Leo" : "Virgo";
                case 9:
                    return day <= 22 ? "Virgo" : "Libra";
                case 10:
                    return day <= 22 ? "Libra" : "Scorpio";
                case 11:
                    return day <= 21 ? "Scorpio" : "Sagittarius";
                case 12:
                    return day <= 21 ? "Sagittarius" : "Capricorn";
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
