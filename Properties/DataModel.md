## Data Model

### Explaination

  The data model is a set of data structures that lay the base for the businnes logic of an application. In typical object-oriented application, the data model is built of client-side classes and collections. Typically, it is somehow stored into a Database Management System, however how the data is exactly stored is not a concern of [MVC].  In fact, other than speed in it's performance, models stored within the DBMS is not as efficient as an ORM based one.
  
  There are many references for this and it only makes sense.   Injection efforts and security risks are not of a concern as old myths die hard dictate.   Dynamic and parametric polymorphic efforts show that RT constructs perform with much better flexibility and tightly coupled integration within a projects design.
  
  Future efforts for extension and modification can be reduced and even eliminated when used in conjunction with a reflective based programming language.

  So you have to ask yourself, why would we use deprecated features, such as stored procedures, to construct a data model integrating in all the business logic which is much more difficult to implement and maintain, when there are options to construct a more dynamic, flexible, easier to maintain, and understand model?

  There are alternatives which are far superior; such as using an ORM sub-system.
  
  Similarly, T-SQL is generated automatically by the ORM content generators.  If you used EF, you would know this.  You should not need to modify this file.

---

### Disadvantages

  1. A DBA will be required for performance tuning
  2. All developers will have to be very well versed in your particular SQL dialect(T-SQL, Pl/SQL, etc)
  3. SQL code isn't as expressive and thus harder to write when covering higher level concepts that aren't really related to data
  4. A lot more unnecessary load on the database

  Practically, only a fool would have all business logic in the database.

  1. Very few developers will be able to create a consistent stored procedure interface that works easily across applications. Usually this is because certain assumptions are made of that calling application
  2. Same goes for documenting all of those stored procedures
  3. Database servers are generally bottlenecked enough as it is. Putting the unnecessary load on them just further narrows this bottleneck. Intricate load balancing and beefy hardware will be required for anything with a decent amount of traffic
  4. SQL is just barely a programming language. I once had the pleasure of maintaining a scripting engine written as a T-SQL stored procedure. It was slow, nearly impossible to understand, and took days to implement what would have been a trivial extension in most languages
  5. What happens when you have a client that needs their database to run a different SQL server? You'll basically have to start from scratch -- You're very tied to your database. Same goes for when Microsoft decides to deprecate a few functions you use a couple hundred times across your stored procedures
  6. Source control is extremely difficult to do properly with stored procedures, more so when you have a lot of them
  7. Databases are hard to keep in sync. What about when you have a conflict of some sort between 2 developers that are working in the database at the same time? They'll be overwriting each others code not really aware of it, depending on your "development database" setup
  8. The tools are definitely less easy to work with, no matter which database engine you use.