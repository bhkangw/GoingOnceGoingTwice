using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace csharpbelt.Models
{
    // the following classes are built specifically for insertion into the db. Models for validations are in ViewModels.cs
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public double HeldAmount { get; set; }
        public double Wallet { get; set; }
        // a user can have many auctions & bids therefore:
        public List<Auction> Auction { get; set; } // list to expect multiple auction objects
        public List<Bid> Bid { get; set; } // list to expect multiple bid objects
        // must create an empty list in the single side of a one to many
        public User()
        {
            Auction = new List<Auction>(); // new empty list of auctions
            Bid = new List<Bid>(); // new empty list of bids
        }
    }
}