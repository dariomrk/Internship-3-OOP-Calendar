namespace Internship_3_OOP_Calendar.Classes
{
    /// <summary>
    /// Defines a person within the calendar.
    /// </summary>
    public class Person
    {
        #region Fields
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        private readonly Dictionary<Guid, bool> _eventAttendance;
        #endregion

        #region Properties
        public string FirstName => _firstName;
        public string LastName => _lastName;
        public string Email => _email;
        // Returns a new Dictionary instance so the _eventAttendance field is not exposed via reference.
        public Dictionary<Guid, bool> EventAttendance => new(_eventAttendance);
        #endregion

        #region Constructors
        public Person(string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException();
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException();
            if (!Classes.Email.Validate(email))
                throw new ArgumentException("Email is not valid!");

            _firstName = firstName;
            _lastName = lastName;
            _email = email;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Marks given event as attended.
        /// </summary>
        /// <param name="eventId">Event id.</param>
        public void Attended(Guid eventId)
        {
            if (!_eventAttendance.ContainsKey(eventId))
            {
                _eventAttendance.Add(eventId, true);
                return;
            }
            _eventAttendance[eventId] = true;
        }
        /// <summary>
        /// Marks given event as unattended.
        /// </summary>
        /// <param name="eventId">Event id.</param>
        public void Missed(Guid eventId)
        {
            if (!_eventAttendance.ContainsKey(eventId))
            {
                _eventAttendance.Add(eventId, false);
                return;
            }
            _eventAttendance[eventId] = false;
        }
        #endregion
    }
}
