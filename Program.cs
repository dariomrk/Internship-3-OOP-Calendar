using Internship_3_OOP_Calendar.Classes;
namespace Internship_3_OOP_Calendar
{
    public class Program
    {
        #region Filter Functions
        static List<Event> FilterEvents(List<Event> events, Predicate<Event> predicate)
        {
            var queried = from e in events
                          where predicate(e)
                          select e;
            return queried.ToList();
        }
        static List<Event> PastEvents(List<Event> events) => FilterEvents(events, (e) => e.EndDateTime < DateTime.UtcNow);
        static List<Event> UpcomingEvents(List<Event> events) => FilterEvents(events, (e) => e.StartDateTime > DateTime.UtcNow);
        static List<Event> ActiveEvents(List<Event> events) => FilterEvents(events, (e) => 
        {
            return e.StartDateTime < DateTime.UtcNow && e.EndDateTime > DateTime.UtcNow;
        }
        );
        #endregion

        #region UI utility functions
        static void Spacer()
        {
            Console.WriteLine(
                "-----------------------------------------------------------------------------------"
                );
        }
        static void WriteEvents(List<Event> events, List<Person> people, Action<Event,List<Person>> writeStyle)
        {
            foreach (var e in events)
            {
                writeStyle(e,people);
            }
        }
        static void WriteStyleActive(Event e, List<Person> people)
        {
            Spacer();
            Console.WriteLine(
                $"Id: {e.Id}\n" +
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Ends in: {(e.EndDateTime - DateTime.UtcNow).ToString("HH:mm")}\n" +
                $"Atendees: "
                );
            foreach(var person in people)
            {
                if (e.CheckIsInvited(person.Email))
                    Console.WriteLine($"\t • {person.FirstName} {person.LastName} : {person.Email}");
            }
            Spacer();
        }
        static void WriteStyleUpcoming(Event e, List<Person> people)
        {
            Spacer();
            Console.WriteLine(
                $"Id: {e.Id}\n" +
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Starts in {(e.StartDateTime - DateTime.UtcNow).ToString("dd")} days - " +
                $"Length: {(e.EndDateTime - e.EndDateTime).ToString("HH:mm")}\n" +
                $"Atendees: "
                );
            foreach (var person in people)
            {
                if (e.CheckIsInvited(person.Email))
                    Console.WriteLine($"\t • {person.FirstName} {person.LastName} : {person.Email}");
            }
            Spacer();
        }
        static void WriteStylePast(Event e, List<Person> people)
        {
            Spacer();
            Console.WriteLine(
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Ended {(DateTime.UtcNow - e.EndDateTime).ToString("dd")} days ago - " +
                $"Length: {(e.EndDateTime - e.EndDateTime).ToString("HH:mm")}\n" +
                $"Atendees:"
                );
            foreach (var person in people)
            {
                if (!e.CheckIsInvited(person.Email))
                    continue;
                if (person.CheckAttended(e.Id))
                    Console.WriteLine($"\t ✔ {person.FirstName} {person.LastName} : {person.Email}");
            }
            Console.WriteLine("Non atendees:");
            foreach (var person in people)
            {
                if (!e.CheckIsInvited(person.Email))
                    continue;
                if (!person.CheckAttended(e.Id))
                    Console.WriteLine($"\t ❌ {person.FirstName} {person.LastName} : {person.Email}");
            }
            Spacer();
        }
        #endregion

        static void Main()
        {
            #region Predefined data
            List<Event> events = new()
            {
                new Event("DUMP DESIGN #1",
                "FESB",
                DateTime.Parse("22 Nov 2022 14:00:00 GMT"),
                DateTime.Parse("22 Nov 2022 16:00:00 GMT"),
                new List<string>()
                {
                    "eldred.moor2@hotmail.com",
                    "eunice2017@gmail.com",
                    "kennedi_kli@gmail.com",
                }
                ),

                new Event("DUMP DEV #1",
                "FESB",
                DateTime.Parse("22 Nov 2022 17:00:00 GMT"),
                DateTime.Parse("22 Nov 2022 19:00:00 GMT"),
                new List<string>()
                {
                    "margarett9@yahoo.com",
                    "mattie_watsi@gmail.com",
                    "leann1977@yahoo.com",
                    "kennedi_kli@gmail.com",
                    "eldred.moor2@hotmail.com",
                }
                ),

                new Event("DUMP DEV #2",
                "Dom mladih",
                DateTime.Parse("26 Nov 2022 17:00:00 GMT"),
                DateTime.Parse("26 Nov 2022 19:00:00 GMT"),
                new List<string>()
                {
                    "margarett9@yahoo.com",
                    "mattie_watsi@gmail.com",
                    "leann1977@yahoo.com",
                    "kennedi_kli@gmail.com",
                    "eldred.moor2@hotmail.com",
                }
                ),

                new Event("DUMP DESIGN #2",
                "Dom mladih",
                DateTime.Parse("26 Nov 2022 19:00:00 GMT"),
                DateTime.Parse("26 Nov 2022 21:00:00 GMT"),
                new List<string>()
                {
                    "eldred.moor2@hotmail.com",
                    "eunice2017@gmail.com",
                    "kennedi_kli@gmail.com",
                }
                ),

                new Event("DUMP DEV #3",
                "Dom mladih",
                DateTime.Parse("10 Dec 2022 17:00:00 GMT"),
                DateTime.Parse("10 Dec 2022 19:00:00 GMT"),
                new List<string>()
                {
                    "margarett9@yahoo.com",
                    "mattie_watsi@gmail.com",
                    "leann1977@yahoo.com",
                    "kennedi_kli@gmail.com",
                    "eldred.moor2@hotmail.com",
                }
                ),

                new Event("DUMP DESIGN #3",
                "FESB",
                DateTime.Parse("11 Dec 2022 11:00:00 GMT"),
                DateTime.Parse("11 Dec 2022 13:00:00 GMT"),
                new List<string>()
                {
                    "eldred.moor2@hotmail.com",
                    "eunice2017@gmail.com",
                    "kennedi_kli@gmail.com",
                }
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