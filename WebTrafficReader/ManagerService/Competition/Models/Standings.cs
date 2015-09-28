using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerService.Competition.Models
{
    public class Standings
    {
        public Conference AFC { get; set; }
        public Conference NFC { get; set; }
    }

    public class Conference
    {
        public Division East { get; set; }
        public Division North { get; set; }
        public Division South { get; set; }
        public Division West { get; set; }
    }

    public class Division
    {
        public IEnumerable<Team> Teams { get; set; }
    }

    public class Team
    {
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Lost { get; set; }
        public int Tie { get; set; }
    }
}
