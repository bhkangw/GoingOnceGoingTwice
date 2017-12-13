using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace csharpbelt.Models
{
    // the following classes are built specifically for insertion into the db. Models for validations are in ViewModels.cs
    public class Auction : BaseEntity
    {
        [Key]
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double StartingBid { get; set; }
        public DateTime EndDate { get; set; }
        // public int BidId { get; set; } // foreign key in a one-to-one
        // public Bid HighestBid { get; set; } // bid object with the foreign key
        public Boolean Resolved { get; set; }

        // an auction can have many bids therefore:
        public List<Bid> Bid { get; set; } // list to expect multiple bid objects
        // must create an empty list in the single side of a one to many
        public Auction()
        {
            Bid = new List<Bid>(); // new empty list of bids
        }
    }
}