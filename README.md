1. Task Description

Implement a taxi application.
There are three types of users in this system:

  1. Administrator
  2. User
  3. Driver

2. System Functions

2.1. Displaying Information to Unregistered Users
The first page an unregistered user sees is the application’s home page, where it is possible either to log in if the user is already registered in the system or to go to the registration/login page.

2.2. User Registration and Login
On the registration/login page, a user can log in using their email address and password.
If the user is not yet registered in the system but wants to use the application’s functions, they must first register on the appropriate page. 
Registration can be done in two ways:

  1. Classic registration – by entering personal data, including: email address, password, first name, last name, date of birth, and address. 
  The password must be entered twice to minimize mistakes when choosing a new password.
  After registration, an administrator must approve the registration.

  2. Via a social network – one social network is sufficient for implementation.

Note: Both registration methods must be implemented.
During registration, it is necessary to define:

  - Username
  - Email
  - Password
  - First and last name
  - Date of birth
  - Address
  - User type – Administrator, User, or Driver
  - User picture – Allow image upload

Note: The image must actually be stored on the server and retrieved for display.
Note: A mechanism for user authentication and authorization must be implemented on the server side.

2.3. User Profile
A registered user can update their personal information on the profile page.

2.4. Registration Verification Process
The administrator has the ability to review user data and can either approve or reject a verification request. 
After approval, the profile becomes active. 
Verification is required for drivers. 
Only after being verified can drivers start working, while regular users do not require verification.
On the user’s profile, there should be an indication of the verification process status (request is being processed, request is approved, or request is rejected).
An email notification should be sent upon verification.

2.5. Dashboard
After a successful login, the user is redirected to the Dashboard page. 
It contains the following elements, which will be described in detail in the following sections:

  - Profile (all users)
  - New Ride (User)
  - Previous Rides (User)
  - Verification (Admin)
  - New Rides (Driver)
  - My Rides (Driver)
  - All Rides (Admin)

2.5.1. Profile
Display and edit the user’s profile.

2.5.2. New Ride
Creating a new ride is done by entering the start and end addresses. 
After entering the addresses, the user must click Order. 
The system then predicts the price based on the distance (define it randomly).
In addition to the price, the system should predict the waiting time for the driver to arrive at the user. 
If the user agrees with the ride price and waiting time, they must click a new Confirm button.
All drivers will then see the new ride in the list of rides waiting to be accepted. 
After acceptance, the time required to travel from the start address to the user’s destination is determined (random time).
After a new ride is created and the estimate is accepted, the user cannot use other system functionalities. 
The only features available are:

  - Countdown to the driver’s arrival – real-time countdown must be implemented
  - Countdown to the end of the ride

After the ride is completed, the user can again use all other system functionalities.
The same rules apply to the driver after accepting the ride.
At the end of the ride, the user has the option to rate the driver from 1 to 5 stars. 
The average driver rating can only be viewed by the administrator, who can block drivers based on the rating. 
The administrator can also unblock drivers afterwards.

*** A blocked driver can log in to the system but cannot accept rides.

2.5.3. Verification
The administrator can see the list of drivers and their status, approve or reject their status, and view which drivers are approved.

2.5.4. Previous Rides
The user can view a list of their previous rides.

2.5.5. New Rides
The driver can see a list of new rides waiting to be accepted.

2.5.6. My Rides
The driver can view their completed rides.

2.5.7. All Rides
The administrator has access to all rides and their statuses.

3. System Implementation

3.1. Server Platforms
For the implementation of the project, the following server platform is used:
.NET CORE or Microsoft Service Fabric

3.2. Client Platforms
For the implementation of the project, the following is used:
Single-page interface application in React.

3.3. Sending Emails
There is no dedicated email service provided. You may use your own email account.

3.4. Concurrent Resource Access
It is important that multiple simultaneous users of the application cannot operate on the same element at the same time.
In addition to this restriction, each student must identify at least one more conflicting situation for their part of the requirements and resolve it appropriately.

Note: It is not enough to protect the client side; the server must also be secured! For example, test with Postman/Swagger whether it is possible to delete/modify an entity that does not exist. 
Handle exceptions on both the front and back ends. 
Create a model on the front end so that if the model changes on the back end, the change needs to be made in only one place on the front end.

Note: Git must be used for version control, and the repository must be on GitHub, accessible to the instructors during project development and defense.

3.5. Solution Architecture and Evaluation Criteria
The project must comply with solution quality criteria and best practices for web application development demonstrated in exercises.
The front end of the application must be divided into components.
URLs of external services called from the front end must be stored in a .env file and read from there; this includes the URL of the back end of the application.
HTTP calls from the front end must be placed in services injected into components, not directly inside components.
Models must exist on the front end.
DTOs and database models must exist as separate models, with proper mapping between them.
Passwords in the database must be hashed.
Token signatures and expiration must be validated.
Configurable data (passwords for external services, URLs) on the back end must be kept in the appsettings.json file and loaded from there.
