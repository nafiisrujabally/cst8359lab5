using EventManagerAPI.Models;

namespace EventManagerAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {

            if (context.Events.Any()) return;

            var events = new List<Event>
        {
            new Event
            {
                Title = "AI Confererence 2026",
                Description = "A conference day about how artificial intelligence is taking over the future of technology in 2026 and how we should prepare to adapt to the changes.",
                Date = new DateTime(2026, 5, 20, 9, 0, 0),
                Location = "Algonquin College, C128, Ottawa",
                Attendees = new List<Attendee>
                {
                    new Attendee { Name="Muhammad Nafiis Rujabally", Email="nafiisrujabally786@email.com" },
            new Attendee {Name="Souhail Chabli", Email="chablishouhail12@email.com" }
                }
            },

            new Event
            {
                Title = "Hackathon Day",
                Description = "A full day of hackathon, coding the best ever project among 10 best software leading experts.",
                Date = new DateTime(2026, 5, 30, 10, 1, 5),
                Location = "University Of Toronto, St George Campus, Toronto",
                
                Attendees = new List<Attendee>
                {
                    new Attendee { Name="John Bakker", Email="johnb@email.com" },
            new Attendee { Name="Zainab Kayid", Email="kaydzainab@email.com" }

                }
            },
            new Event
            {
                Title = "Global Conference on Information Technology",
                Description = "Dive deep into the world of IT and computer science, with engaging talks, interactive workshops, and networking opportunities.",
                Date = new DateTime(2026, 7, 12, 8, 0, 0),
                Location = "McGill New Residence Hall, Montreal",
                Attendees = new List<Attendee>
                {
                    new Attendee { Name="Feiz Sondagur", Email="feizs123@email.com" },
                   new Attendee { Name="Thian Zhuang", Email="Tianzhuang09@gmail.com" }


            }
            }
        };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}
