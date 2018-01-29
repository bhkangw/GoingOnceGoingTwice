## Synopsis
This is an auction service web app built in C# using ASP.Net Core that allows users to post items for sale & bid on a community of other items. Once an auction has ended, the dollar credit automatically transfers and is exchanged between seller and highest bidder. 

## Motivation
This app was built exam-style, from-scratch, timed in 4.5 hours in order to test proficiency of Python using the Django framework. Received a perfect score of 10.0 out of 10.0.

## Features
- Appointments & Login app has full validations on all form fields ie. password, name, email, auction dates, product info, bid price, etc.

- Users' passwords encypted using bcrypt.

- Each user has a designated wallet which keeps track of spending and prohibits auction transactions if balance is insufficient.

- Connected to backend database using mySQL for all product, auction, and user data. 

- Utilizes multiple model relationships in order to manage each user's auctions, bids, wins and wallets.

- Handled and tested datetime trigger events for the ending auctions and the preceding actions e.g. fund transfers.

- Used Twitter Bootstrap for styling on both login page and Auction dashboard.

## Skills/Concepts Practiced
- OOP / ORMs
- Client/Server Communication
- ASP.NET Core
- Models/Migration/Relationships
- Password Hashing
- Datetime triggered events
- Backend Database connections
- SQL Queries
- RESTful Routing
- Deployment through AWS
