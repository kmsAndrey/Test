using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppTest.BusinessLogic
{
    public static class Validator
    {
        private static bool Check;//validation status
        private static string TextErrorMessage;//Text error

        /// <summary>
        /// Start validation
        /// </summary>
        /// <param name="empoyee">update empoyee</param>
        /// <returns>status</returns>
        public static bool MainValidator(Empoyee empoyee)
        {
            Check = true;
            string text ="";
            Empoyee em = empoyee;
            if (!ValidatorObligation(em.FirstName))
                text += "Не корректное имя \n";
            if(!ValidatorObligation(em.SurName))
                text += "Не корректное фамилия \n";
            if(!ValidatorObligation(em.DateOfBirth))
                text += "Не корректное дата рождения \n";
            if(!ValidatorPassport(em.DocSeries+" "+em.DocNumber))
                text += "Не корректные паспортные данные \n";
            if(!ValidatorObligationPosition(em.Position))
                text += "Не корректная должность";
            if (!Check)
                TextErrorMessage = text;
            return Check;
        }

        /// <summary>
        /// Check string data
        /// </summary>
        /// <param name="data">Data of string field empoyee</param>
        /// <returns>Status check</returns>
        private static bool ValidatorTextEmployee(string data)
        {
            if (Regex.IsMatch(data, "[^a-zA-Zа-яА-Я*$]"))
            {
                Check = false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check Day of birthday
        /// </summary>
        /// <param name="dateTime">Data of birthday empoyee</param>
        /// <returns>Status check</returns>
        private static bool ValidatorDataEmployee(DateTime dateTime)
        {
            if(dateTime.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                Check = false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check passport
        /// </summary>
        /// <param name="data">Passport series and number</param>
        /// <returns>Status check</returns>
        private static bool ValidatorPassport(string data)
        {
            if(!String.IsNullOrEmpty(data))
            { 
                if (!Regex.IsMatch(data, @"\d{4} \d{6}"))
                {
                    Check = false;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Status check</returns>
        private static bool ValidatorObligationPosition(object data)
        {
            if (data is string)
            {
                if (String.IsNullOrEmpty(data.ToString()))
                {
                    Check = false;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check obligation field
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Status check</returns>
        private static bool ValidatorObligation(object data)
        {
            if(data is string)
            {
                if(String.IsNullOrEmpty(data.ToString()))
                {
                    Check = false;
                    return false;
                }
                if (!ValidatorTextEmployee(data.ToString()))
                    return false;
            }
            if(data is DateTime)
            {
                if(!ValidatorDataEmployee((DateTime)data))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Send error message
        /// </summary>
        /// <returns>Error</returns>
        public static string SendErrorMessage()
        {
            return TextErrorMessage;
        }
    }
}
