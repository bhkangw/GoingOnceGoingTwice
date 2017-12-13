using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using csharpbelt.Models;
using Microsoft.AspNetCore.Identity;

namespace csharpbelt.Controllers
{
    public class AuctionController : Controller
    {
        private Context _context;

        public AuctionController(Context context)
        {
            _context = context;
        }

        private User ActiveUser // creates a new User instance using the id of the logged in user
        {
            get { return _context.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).FirstOrDefault(); } // returns one user object where UserId matches session's
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Index() // once the user successfully logs in
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            User thisUser = _context.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).FirstOrDefault();
            ViewBag.UserInfo = thisUser;

            // sort auctions by time remaining
            ViewBag.auctions = _context.auctions.OrderBy(a => a.EndDate).Include(a => a.User).ToList();
            IEnumerable<Auction> auctions = _context.auctions.OrderBy(a => a.EndDate).Include(a => a.User).ToList();
            
            // if(_context.bids.Where(b => b.AuctionId == b.AuctionId).OrderBy(b => b.Amount).FirstOrDefault() != null){
            //     // displaying the highest bid for each auction on the index page
            //     var highestbids = new Dictionary<int, double>();
            //     foreach (var auction in auctions)
            //     {
            //         var highestBid = _context.bids.Where(b => b.AuctionId == auction.AuctionId).OrderBy(b => b.Amount).FirstOrDefault();
            //         highestbids.Add(auction.AuctionId, highestBid.Amount);
            //     }

            //     ViewBag.highestbids = highestbids;
            // }

            return View(auctions);

        }

        [HttpGet]
        [Route("newauction")]
        public IActionResult NewAuction() // once the user successfully logs in
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            return View();

        }

        [HttpPost]
        [Route("addauction")]
        public IActionResult AddAuction(AddAuction newauction)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            if(ModelState.IsValid)
            {
                Auction auction = new Auction
                {
                    UserId = ActiveUser.UserId,
                    ProductName = newauction.ProductName,
                    Description = newauction.Description,
                    StartingBid = newauction.StartingBid,
                    EndDate = newauction.EndDate,
                    Resolved = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                _context.auctions.Add(auction);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.UserInfo = ActiveUser;
            return View("NewAuction");
        }

        [HttpGet]
        [Route("auction/{AuctionId}")]
        public IActionResult ShowAuction(int AuctionId) // once the user successfully logs in
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            Auction thisauction = _context.auctions.Where(a => a.AuctionId == AuctionId).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();
            ViewBag.auction = _context.auctions.Where(a => a.AuctionId == AuctionId).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();
            
            Bid highestbid = _context.bids.Where(b => b.AuctionId == AuctionId).Where(b => b.HighestBid == true).Include(b => b.User).FirstOrDefault();
            ViewBag.highestbid = _context.bids.Where(b => b.AuctionId == AuctionId).Where(b => b.HighestBid == true).Include(b => b.User).FirstOrDefault();
            // Auction auction = _context.auctions.Where(a => a.AuctionId == AuctionID).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();
            
            // ACTIONS WHEN ENDDATE IS REACHED
            if(DateTime.Now >= thisauction.EndDate) // trigger and actions for when the auction ends
            {
                highestbid.User.Wallet -= highestbid.Amount; // remove the $ from the higher bigger
                thisauction.User.Wallet += highestbid.Amount; // give the $ to the creator of the auction
                thisauction.Resolved = true; // changed status of the auction to resolved
                _context.SaveChanges(); // lolololol I forgot this line for the longest time !!! gahhh
            }
            
            return View();

        }

        [HttpPost]
        [Route("auction/{AuctionId}/newbid")]
        public IActionResult NewBid(int AuctionId, int Amount)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            var highestBid = _context.bids.Where(b => b.AuctionId == AuctionId).OrderBy(b => b.Amount).FirstOrDefault();
            var thisAuction = _context.auctions.Where(a => a.AuctionId == AuctionId).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();
            ViewBag.auction = _context.auctions.Where(a => a.AuctionId == AuctionId).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();

            if(highestBid != null){
                if (Amount < highestBid.Amount || highestBid.Amount < thisAuction.StartingBid) // prohibit bids when it's not the highest
                {
                    ViewBag.error = "Bid must be higher than current highest or starting bid!"; 
                    ViewBag.highestbid = _context.bids.Where(b => b.AuctionId == AuctionId).Where(b => b.HighestBid == true).SingleOrDefault();
                    ViewBag.auction = _context.auctions.Where(a => a.AuctionId == AuctionId).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();
                    return View("ShowAuction");
                }
                if (Amount > ActiveUser.Wallet - ActiveUser.HeldAmount) // prohibit bids when all money is tied up
                {
                    ViewBag.error = "All of your money is tied up in other auctions!";
                    ViewBag.highestbid = _context.bids.Where(b => b.AuctionId == AuctionId).Where(b => b.HighestBid == true).SingleOrDefault();
                    ViewBag.auction = _context.auctions.Where(a => a.AuctionId == AuctionId).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();
                    return View("ShowAuction");
                }
                if (Amount > ActiveUser.Wallet)
                {
                    ViewBag.error = "You don't have enough money in your wallet!"; // prohibit bids when not enough money left in wallet
                    ViewBag.highestbid = _context.bids.Where(b => b.AuctionId == AuctionId).Where(b => b.HighestBid == true).SingleOrDefault();
                    ViewBag.auction = _context.auctions.Where(a => a.AuctionId == AuctionId).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();
                    return View("ShowAuction");
                }
            }

            if(ModelState.IsValid)
            {
                Bid bid = new Bid
                {
                    AuctionId = AuctionId,
                    UserId = ActiveUser.UserId,
                    Amount = Amount,
                    HighestBid = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                ActiveUser.HeldAmount += Amount;

                List<Bid> bids = _context.bids.Where(b => b.AuctionId == AuctionId).ToList();
                foreach (var b in bids) // changing all other bids to not highest
                {
                    b.HighestBid = false;
                }

                _context.bids.Add(bid);
                _context.SaveChanges();



                return RedirectToAction("ShowAuction");
            }

            ViewBag.highestbid = _context.bids.Where(b => b.AuctionId == AuctionId).Where(b => b.HighestBid == true).SingleOrDefault();

            ViewBag.auction = _context.auctions.Where(a => a.AuctionId == AuctionId).Include(a => a.User).Include(a => a.Bid).ThenInclude(b => b.User).SingleOrDefault();
            return View("ShowAuction");
        }

        [HttpPost]
        [Route("deleteauction")]
        public IActionResult Delete(int AuctionId)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");

            Auction ToDelete = _context.auctions.SingleOrDefault(a => a.AuctionId == AuctionId);
            _context.auctions.Remove(ToDelete);
            _context.SaveChanges();

            User thisUser = _context.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).FirstOrDefault();
            ViewBag.UserInfo = thisUser;

            // sort auctions by time remaining
            ViewBag.auctions = _context.auctions.OrderBy(a => a.EndDate).Include(a => a.User).ToList();
            return View("Index");
        }
    }
}