# CashTrack
![CashTrack](src/ClientApp/public/images/cash-track.png)



A budgeting app

notes:
1. All Entities inherit from IEntity except one that is just a many to many class
2. All Entities have names that are plural (Expenses) and their properties are lower case. This is to easily identify properties when using automapper and to match PGSQL. Makes it easier to write queries.
3. All repositories inherit from IRepository<T>
4. Postgresql Server must be set to CST (it defaults to LA time for some reason)
5. If you see time zone descrepencies in the DB it's only because it's accounting for daylight savings. All time zones are UTC with offset -5 or -6.
6. No repository for Authentication or users as I don't plan on creating, updating, deleting users (just me and sarah using it).
7. Automapper Profile lives in the service class. Fluent Validator class lives in Models folder (view models).