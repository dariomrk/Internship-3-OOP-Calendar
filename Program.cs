using Internship_3_OOP_Calendar.Classes;
namespace Internship_3_OOP_Calendar
{
    public class Program
    {
        static void Main()
        {
            #region Predefined data
            List<Event> events = new()
            {
                new Event("DUMP DESIGN #1",
                "FESB",
                DateTime.Parse("22 Nov 2022 14:00:00 GMT"),
                DateTime.Parse("22 Nov 2022 16:00:00 GMT")
                ),

                new Event("DUMP DEV #1",
                "FESB",
                DateTime.Parse("22 Nov 2022 17:00:00 GMT"),
                DateTime.Parse("22 Nov 2022 19:00:00 GMT")
                ),

                new Event("DUMP DEV #2",
                "Dom mladih",
                DateTime.Parse("26 Nov 2022 17:00:00 GMT"),
                DateTime.Parse("26 Nov 2022 19:00:00 GMT")
                ),

                new Event("DUMP DESIGN #2",
                "Dom mladih",
                DateTime.Parse("26 Nov 2022 19:00:00 GMT"),
                DateTime.Parse("26 Nov 2022 21:00:00 GMT")
                ),

                new Event("DUMP DEV #3",
                "Dom mladih",
                DateTime.Parse("10 Dec 2022 17:00:00 GMT"),
                DateTime.Parse("10 Dec 2022 19:00:00 GMT")
                ),

                new Event("DUMP DESIGN #3",
                "FESB",
                DateTime.Parse("11 Dec 2022 11:00:00 GMT"),
                DateTime.Parse("11 Dec 2022 13:00:00 GMT")
                ),
            };
            List<Person> people = new()
            {
                new Person("Mary","Leroy","eldred.moor2@hotmail.com"),
                new Person("Steven","Wick","eunice2017@gmail.com"),
                new Person("Mario","Ehrhardt","kennedi_kli@gmail.com"),
                new Person("Charles","Lean","leann1977@yahoo.com"),
                new Person("Karen","Moss","moss_k@gmail.com"),
                new Person("Kimberly","Woodward","kayleigh1991@gmail.com"),
                new Person("Clifton","Cook","cliftoncook@yahoo.com"),
                new Person("Gladys","Rodriguez","mattie_watsi@gmail.com"),
                new Person("Miguel","Booher","kurt_ko1971@hotmail.com"),
                new Person("Terry","Martin","margarett9@yahoo.com")
            };
            #endregion


        }
    }
}