using ManagerService.Competition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerService.Competition
{
    public class StandingsApi
    {
        public Standings GetStandings()
        {
            return new Standings {
                AFC = new Conference {
                    East  = new Division {
                        Teams = new List<Team>
                        {
                            new Team { Name = "Patriots", Wins = 3, Lost = 0, Tie = 0 },
                            new Team { Name = "Bills", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Jets", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Dolphins", Wins = 1, Lost = 2, Tie = 0 }
                        }
                    },
                    North = new Division {
                        Teams = new List<Team>
                        {
                            new Team { Name = "Patriots", Wins = 3, Lost = 0, Tie = 0 },
                            new Team { Name = "Bills", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Jets", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Dolphins", Wins = 1, Lost = 2, Tie = 0 }
                        }
                    },
                    South = new Division {
                        Teams = new List<Team>
                        {
                            new Team { Name = "Patriots", Wins = 3, Lost = 0, Tie = 0 },
                            new Team { Name = "Bills", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Jets", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Dolphins", Wins = 1, Lost = 2, Tie = 0 }
                        }
                    },
                    West  = new Division {
                        Teams = new List<Team>
                        {
                            new Team { Name = "Patriots", Wins = 3, Lost = 0, Tie = 0 },
                            new Team { Name = "Bills", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Jets", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Dolphins", Wins = 1, Lost = 2, Tie = 0 }
                        }
                    }
                },
                NFC = new Conference {
                    East  = new Division {
                        Teams = new List<Team>
                        {
                            new Team { Name = "Patriots", Wins = 3, Lost = 0, Tie = 0 },
                            new Team { Name = "Bills", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Jets", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Dolphins", Wins = 1, Lost = 2, Tie = 0 }
                        }
                    },
                    North = new Division {
                        Teams = new List<Team>
                        {
                            new Team { Name = "Patriots", Wins = 3, Lost = 0, Tie = 0 },
                            new Team { Name = "Bills", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Jets", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Dolphins", Wins = 1, Lost = 2, Tie = 0 }
                        }
                    },
                    South = new Division {
                        Teams = new List<Team>
                        {
                            new Team { Name = "Patriots", Wins = 3, Lost = 0, Tie = 0 },
                            new Team { Name = "Bills", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Jets", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Dolphins", Wins = 1, Lost = 2, Tie = 0 }
                        }
                    },
                    West  = new Division {
                        Teams = new List<Team>
                        {
                            new Team { Name = "Patriots", Wins = 3, Lost = 0, Tie = 0 },
                            new Team { Name = "Bills", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Jets", Wins = 2, Lost = 1, Tie = 0 },
                            new Team { Name = "Dolphins", Wins = 1, Lost = 2, Tie = 0 }
                        }
                    }
                }
            };
        }
    }
}
