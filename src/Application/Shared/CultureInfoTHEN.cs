using System.Globalization;

namespace EXAM_SYSTEM_API.Application.Shared
{
    public class CultureInfoTHEN
    {
        public CultureInfo _cultureEngInfoEN = new("en-US");
        public CultureInfo _cultureEngInfoTH = new("th-Th");

        public string FormatDateTH(DateTime? dt, string format = "dd/MM/yy")
        {
            if (dt is null) return "";

            DateTime date = dt.Value; // Unwrap the nullable safely

            try
            {
                string day = date.Day.ToString("00");
                string month = date.Month.ToString("00");
                string year = date.Year.ToString();
                string time = date.ToString("HH:mm");

                string month_name = "";
                string month_sname = "";
                string month_nameEn = "";

                switch (month)
                {
                    case "01": month_name = "มกราคม"; month_sname = "ม.ค"; month_nameEn = "January"; break;
                    case "02": month_name = "กุมภาพันธ์"; month_sname = "ก.พ"; month_nameEn = "February"; break;
                    case "03": month_name = "มีนาคม"; month_sname = "มี.ค"; month_nameEn = "March"; break;
                    case "04": month_name = "เมษายน"; month_sname = "เม.ย"; month_nameEn = "April"; break;
                    case "05": month_name = "พฤษภาคม"; month_sname = "พ.ค"; month_nameEn = "May"; break;
                    case "06": month_name = "มิถุนายน"; month_sname = "มิ.ย"; month_nameEn = "June"; break;
                    case "07": month_name = "กรกฎาคม"; month_sname = "ก.ค"; month_nameEn = "July"; break;
                    case "08": month_name = "สิงหาคม"; month_sname = "ส.ค"; month_nameEn = "August"; break;
                    case "09": month_name = "กันยายน"; month_sname = "ก.ย"; month_nameEn = "September"; break;
                    case "10": month_name = "ตุลาคม"; month_sname = "ต.ค"; month_nameEn = "October"; break;
                    case "11": month_name = "พฤศจิกายน"; month_sname = "พ.ย"; month_nameEn = "November"; break;
                    case "12": month_name = "ธันวาคม"; month_sname = "ธ.ค"; month_nameEn = "December"; break;
                }

                int buddhistYear = date.Year + 543;
                string buddhistYearShort = buddhistYear.ToString().Substring(2, 2);

                switch (format)
                {
                    case "dd MMMM yyyy HH:mm TH":
                        return $"{day} {month_name} {buddhistYear} {time}";
                    case "dd MMMM yyyy เวลา HH:mm TH":
                        return $"{day} {month_name} {buddhistYear} เวลา {time}";
                    case "dd MMMM yyyy TH":
                        return $"{day} {month_name} {buddhistYear}";
                    case "dd MMMM yyyy EN":
                        return $"{day} {month_nameEn} {year}";
                    case "dd/mm/yyyy TH":
                        return $"{day}/{month}/{buddhistYear}";
                    case "HH:mm":
                        return time;
                    case "dd/mm/yyyy HH:mm TH":
                        return $"{day}/{month}/{buddhistYear} {time}";
                    case "MMMM yyyy TH":
                        return $"{month_name} {buddhistYear}";
                    case "dd MM yyyy TH":
                        return $"{day} {month_sname} {buddhistYear}";
                    case "dd MM yyyy HH:mm TH":
                        return $"{day} {month_sname} {buddhistYear} {time}";
                    case "dd/MM/yy":
                        return $"{day}/{month}/{buddhistYearShort}";
                    case "dd MMMM yy HH:mm TH":
                        return $"{day} {month_sname} {buddhistYearShort} เวลา {time}";
                    case "dd/MM/yyyy TH":
                        return $"{day}/{month_sname}/{buddhistYear}";
                    case "dd/MM/yyyy TH ":
                        return $"{day} {month_sname} {buddhistYear}";
                    default:
                        return $"{day}/{month}/{buddhistYear}";
                }

            }
            catch (Exception ex)
            {
                return date.ToString("dd/MM/yy", _cultureEngInfoTH);
            }
        }
    }
}
