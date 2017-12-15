using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace csharpbelt.Models
{
    // the following classes are built specifically for insertion into the db. Models for validations are in ViewModels.cs
    public class Bid : BaseEntity
    {
        [Key]
        public int BidId { get; set; }
        public int UserId { get; set; } // foreign key goes in the multiple side of a one to many
        public User User { get; set; } // User objects created along with the foreign key
        public int AuctionId { get; set; } // foreign key goes in the multiple side of a one to many
        public Auction Auction { get; set; } // Auction objects created along with the foreign key
        // public Auction Highest { get; set; } // Auction objects created along with the foreign key
        public double Amount { get; set; }
        public Boolean HighestBid { get; set; }
    }
}