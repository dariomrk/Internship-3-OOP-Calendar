﻿using Internship_3_OOP_Calendar.Classes;
namespace Internship_3_OOP_Calendar
{
    public class Program
    {
        #region Filter functions
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
            Spacer();
        }
        static void WriteStyleActive(Event e, List<Person> people)
        {
            TimeSpan tsLength = e.EndDateTime - DateTime.UtcNow;
            Spacer();
            Console.WriteLine(
                $"Id: {e.Id}\n" +
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Ends in: {tsLength.ToString("%h")}h {tsLength.ToString("%m")}min\n" +
                $"Invited:"
                );
            foreach (var person in people)
            {
                if (!person.AttendanceSet(e.Id))
                    person.Attended(e.Id);
                if (e.CheckIsInvited(person.Email))
                {
                    Console.WriteLine($"\t{person.FirstName} {person.LastName} : {person.Email}");
                }
            }
        }
        static void WriteStyleUpcoming(Event e, List<Person> people)
        {
            TimeSpan tsLength = e.EndDateTime - e.StartDateTime;
            TimeSpan tsUntill = e.StartDateTime - DateTime.UtcNow;
            Spacer();
            Console.WriteLine(
                $"Id: {e.Id}\n" +
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Starts in {tsUntill.ToString("dd")} days {tsUntill.ToString("%h")}h - " +
                $"Length: {tsLength.ToString("%h")}h {tsLength.ToString("%m")}min\n" +
                $"Invited:"
                );
            foreach (var person in people)
            {
                if (e.CheckIsInvited(person.Email))
                    Console.WriteLine($"\t{person.FirstName} {person.LastName} : {person.Email}");
            }
        }
        static void WriteStylePast(Event e, List<Person> people)
        {
            TimeSpan tsLength = e.EndDateTime - e.StartDateTime;
            TimeSpan tsSince = DateTime.UtcNow - e.EndDateTime;
            Spacer();
            Console.WriteLine(
                $"Id: {e.Id}\n" +
                $"Title: {e.Title} - Location: {e.Location} - " +
                $"Ended {tsSince.ToString("%d")} days ago - " +
                $"Length: {tsLength.ToString("%h")}h {tsLength.ToString("%m")}min\n" +
                $"Atended:"
                );
            foreach (var person in people)
            {
                if (!e.CheckIsInvited(person.Email))
                    continue;
                if (person.CheckAttended(e.Id))
                    Console.WriteLine($"\t{person.FirstName} {person.LastName} : {person.Email}");
            }
            Console.WriteLine("Missing:");
            foreach (var person in people)
            {
                if (!e.CheckIsInvited(person.Email))
                    continue;
                if (!person.CheckAttended(e.Id))
                    Console.WriteLine($"\t{person.FirstName} {person.LastName} : {person.Email}");
            }
        }

        static void WaitForUser()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
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

        #region Data handling functions
        /// <summary>
        /// Tries to find an Event object given the id.
        /// </summary>
        /// <param name="id">Event id to search.</param>
        /// <param name="events">Event collection to search from.</param>
        /// <param name="output">Found event.</param>
        /// <returns>Boolean indicating wether the lookup was successful.</returns>
        static bool TryGetEvent(Guid id, List<Event> events, out Event output)
        {
            output = null;
            foreach (var e in events)
            {
                if (e.Id == id)
                {
                    output = e;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Sets the person as missing from an event.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <param name="people">Collection of all people.</param>
        /// <param name="emails">Emails of people that missed a given event.</param>
        static void MarkAsMissing(Guid id,List<Event> events, List<Person> people, string[] emails)
        {
            TryGetEvent(id, events, out Event e);

            var invited = from i in e.Invited
                          where emails.Contains(i)
                          select i;

            foreach (var person in people)
                foreach (var email in emails)
                {
                    if (person.Email == email && invited.Contains(person.Email))
                    {
                        person.Missed(id);
                    }
                }
            foreach(var person in people)
            {
                if(emails.Contains(person.Email) && !invited.Contains(person.Email))
                {
                    Console.WriteLine($"\t{person.Email} is not on the list of invited people!");
                }
            }
        }
        #endregion
        static void Main()
        {
            #region Predefined data
            List<Event> events = new()
            {
                new Event("e794943a-6a10-46a4-8d84-80f906ea9c6c",
                "DUMP DESIGN #1",
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

                new Event("44bd33b8-01b9-45b0-9420-a209236fbcec",
                "DUMP DEV #1",
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
                DateTime.Parse("10 Dec 2022 13:30:00 GMT"),
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
                DateTime.Parse("11 Dec 2022 10:30:00 GMT"),
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
                new Person("Mary","Leroy","eldred.moor2@hotmail.com",
                new()
                {
                    {Guid.Parse("e794943a-6a10-46a4-8d84-80f906ea9c6c"),false },
                    {Guid.Parse("44bd33b8-01b9-45b0-9420-a209236fbcec"),true },
                }),
                new Person("Steven","Wick","eunice2017@gmail.com"),
                new Person("Mario","Ehrhardt","kennedi_kli@gmail.com",
                new()
                {
                    {Guid.Parse("44bd33b8-01b9-45b0-9420-a209236fbcec"),true },
                }),
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
                            active:
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
                                        Console.Write("Input event id: ");
                                        if(!Guid.TryParse(Console.ReadLine(),out Guid id)){
                                            WriteWarning("Invalid id format!");
                                            break;
                                        }
                                        if(!TryGetEvent(id,events,out _))
                                        {
                                            WriteWarning("Event with given id does not exist!");
                                            break;
                                        }
                                        Console.WriteLine("Input emails of people to mark as missing," +
                                            " separated with whitespace.");
                                        Console.Write("Input here: ");
                                        string[] emails = (Console.ReadLine().Split(' '));
                                        MarkAsMissing(id,events,people,emails);
                                        WaitForUser();
                                        break;
                                    }
                            }
                        }
                        break;
                    #endregion
                    case 2:
                        {
                            #region Upcoming Events
                            Console.Clear();
                            WriteEvents(UpcomingEvents(events), people, WriteStyleUpcoming);
                            string[] upcomingEventsOptions =
                            {
                                    "Return to main menu",
                                    "Delete event",
                                    "Remove invited people",
                                };
                            int submenuOption = WriteOptions(upcomingEventsOptions);
                            switch (submenuOption)
                            {
                                case 0:
                                    {
                                        break;
                                    }
                                case 1:
                                    {
                                        // TODO submenu option
                                        break;
                                    }
                                case 2:
                                    {
                                        // TODO submenu option
                                        break;
                                    }
                            }
                            break;
                            #endregion
                        }
                    case 3:
                        {
                            #region Past Events
                            Console.Clear();
                            WriteEvents(PastEvents(events), people, WriteStylePast);
                            WaitForUser();
                            break;
                            #endregion
                        }
                    case 4:
                        {
                            #region Create Event
                            // TODO implement menu option
                            break;
                            #endregion
                        }
                }
            }
        }
    }
}