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
        static void WriteEvents(List<Event> events, List<Person> people, Action<Event, List<Person>> writeStyle)
        {
            foreach (var e in events)
            {
                writeStyle(e, people);
            }
        }
        static void WriteStyleActive(Event e, List<Person> people)
        {
            TimeSpan ts = e.EndDateTime - DateTime.UtcNow;
            Spacer();
            Console.WriteLine(
                $"Id: {e.Id}\n" +
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Ends in: {ts.ToString("%hh")}h {ts.ToString("%m")}min\n" +
                $"Invited: "
                );
            foreach (var person in people)
            {
                if (e.CheckIsInvited(person.Email))
                    Console.WriteLine($"\t • {person.FirstName} {person.LastName} : {person.Email}");
            }
        }
        static void WriteStyleUpcoming(Event e, List<Person> people)
        {
            TimeSpan ts = e.EndDateTime - e.EndDateTime;
            Spacer();
            Console.WriteLine(
                $"Id: {e.Id}\n" +
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Starts in {(e.StartDateTime - DateTime.UtcNow).ToString("dd")} days - " +
                $"Length: {ts.ToString("%hh")}h {ts.ToString("%m")}min\n" +
                $"Atendees: "
                );
            foreach (var person in people)
            {
                if (e.CheckIsInvited(person.Email))
                    Console.WriteLine($"\t • {person.FirstName} {person.LastName} : {person.Email}");
            }
        }
        static void WriteStylePast(Event e, List<Person> people)
        {
            TimeSpan tsLength = e.EndDateTime - e.StartDateTime;
            TimeSpan tsAgo = DateTime.UtcNow - e.EndDateTime;
            Spacer();
            Console.WriteLine(
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Ended {tsAgo.ToString("%d")} days ago - " +
                $"Length: {tsLength.ToString("%hh")}h {tsLength.ToString("%m")}min\n" +
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
        }

        static void WaitForUser()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static void WriteWarning(string warning)
        {
            Console.WriteLine(warning);
            WaitForUser();
        }
        static int WriteOptions(string[] options)
        {
            while (true)
            {
                Console.WriteLine("Available options:");
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{i} - {options[i]}");
                }
                Console.Write("Input: ");
                string? userInput = Console.ReadLine();
                if (!int.TryParse(userInput, out int parsedInput))
                {
                    WriteWarning("Please input a valid option!");
                    continue;
                }
                if (parsedInput < 0 || parsedInput > options.Length -1)
                {
                    WriteWarning("Please input a valid option!");
                    continue;
                }
                return parsedInput;
            }
        }
        #endregion

        static void Main()
        {
            #region Predefined data
            List<Event> events = new()
            {
                new Event("Fake event #0",
                "",
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                new List<string>()
                {
                    "eldred.moor2@hotmail.com",
                    "eunice2017@gmail.com",
                    "kennedi_kli@gmail.com",
                }
                ),
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

            while (true)
            {
                string[] options =
                {
                    "Close the program",
                    "Currently active events",
                    "Upcoming events",
                    "Past events",
                    "Create event",
                };

                Console.Clear();
                int option = WriteOptions(options);

                Console.Clear();
                switch (option)
                {
                    case 0:
                        {
                            return;
                        }
                    case 1:
                        {
                            #region Active Events
                            Console.Clear();
                            WriteEvents(ActiveEvents(events), people, WriteStyleActive);
                            string[] activeEventsOptions =
                            {
                                    "Return to main menu",
                                    "Mark as missing",
                                };
                            int submenuOption = WriteOptions(activeEventsOptions);
                            switch (submenuOption)
                            {
                                case 0:
                                    {
                                        break;
                                    }
                                case 1:
                                    {
                                        // TODO Mark as missing submenu option
                                        break;
                                    }
                            }
                        }
                        break;
                    #endregion
                    case 2:
                        {

                            break;
                        }
                    case 3:
                        {

                            break;
                        }
                    case 4:
                        {

                            break;
                        }
                }
            }
        }
    }
}