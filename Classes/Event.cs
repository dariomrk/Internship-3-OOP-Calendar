namespace Internship_3_OOP_Calendar.Classes
{
    /// <summary>
    /// Defines a calendar event.
    /// </summary>
    public class Event
    {
        #region Fields
        public readonly Guid Id = Guid.NewGuid();
        private readonly List<string> _invited;
        private string _title;
        private string _location;
        private DateTime _startDateTime = DateTime.MinValue;
        private DateTime _endDateTime = DateTime.MaxValue;
        #endregion

        #region Properties
        // Returns a new List instance so the _invited field is not exposed via reference.
        public List<string> Invited { get => new List<string>(_invited); }
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Event title cannot be empty!");
                }
                _title = value;
            }
        }
        public string Location
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_location))
                    return "No location set";
                return _location;
            } 
            set => _location = value;
        }
        public DateTime StartDateTime
        {
            get => _startDateTime;
            set
            {
                if (value > _endDateTime)
                {
                    throw new InvalidOperationException("Start DateTime must be before end DateTime");
                }
                _startDateTime = value;
            }
        }
        public DateTime EndDateTime
        {
            get => _endDateTime;
            set
            {
                if (value < _startDateTime)
                {
                    throw new InvalidOperationException("End DateTime must be after start DateTime");
                }
                _endDateTime = value;
            }
        }
        #endregion

        #region Constructors
        public Event(string title, string location, DateTime startAt, DateTime endAt)
        {
            Title = title;
            Location = location;
            StartDateTime = startAt;
            EndDateTime = endAt;
            _invited = new List<string>();
        }
        public Event(string title, string location, DateTime startAt, DateTime endAt, List<string> invited)
        {
            Title = title;
            Location = location;
            StartDateTime = startAt;
            EndDateTime = endAt;
            _invited = invited;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check wether the person is invited to the event.
        /// </summary>
        /// <param name="email">Email to check.</param>
        /// <returns>Boolean indicating wether the person is invited.</returns>
        public bool CheckIsInvited(string email)
        {
            var query = from invite in _invited
                        where email == invite
                        select invite;
            return query.Any();
        }
        /// <summary>
        /// Adds a person to the list of invited people.
        /// Also checks wether the email address is valid.
        /// </summary>
        /// <param name="email">Email to add.</param>
        /// <exception cref="ArgumentException">Thrown if email is not valid.</exception>
        public void AddInvited(string email)
        {
            if (!Email.Validate(email))
                throw new ArgumentException("Email is not valid!");

            if (CheckIsInvited(email))
                return;
            _invited.Add(email);
        }
        /// <summary>
        /// Removes the person from the list of invited people.
        /// </summary>
        /// <param name="email">Email to remove.</param>
        /// <returns>Bool indicating wether the operation was successful.</returns>
        public bool RemoveInvited(string email)
        {
            return _invited.Remove(email);
        }
        #endregion
    }
}
