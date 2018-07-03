--1
INSERT INTO Towns (Id, Name)
VALUES (1, 'Sofia')

INSERT INTO Towns (Id, Name)
VALUES (2, 'Plovdiv')

INSERT INTO Towns (Id, Name)
VALUES (3, 'Varna')

INSERT INTO Minions (Id,Name, Age, TownID)
VALUES (1, 'Kevin', 22, 1)

INSERT INTO Minions (Id,Name, Age, TownID)
VALUES (2, 'Bob', 15, 3)

INSERT INTO Minions (Id,Name, Age, TownID)
VALUES (3, 'Steward', NULL , 2)

--7
CREATE TABLE People(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(20) NOT NULL,
	Picture VARBINARY(2000),
	Height DECIMAL(15,2),
	Weight DECIMAL(15,2),
	Gender CHAR(1) NOT NULL,
	BirthDate DATE NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People(Name,Picture,Height,Weight,Gender,BirthDate,Biography) VALUES
('Gogo',15,5,6,'m','1991-02-02','ASKda'),
('Gogo',15,5,6,'m','1991-02-02','ASKda'),
('Gogo',15,5,6,'m','1991-02-02','ASKda'),
('Gogo',15,5,6,'m','1991-02-02',NULL),
('Gogo',15,5,6,'m','1991-02-02',NULL)

--8

CREATE TABLE Users (
	Id INT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL,
	Password VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX),
	LastLoginTime DATETIME,
	IsDeleted BIT
) 

INSERT INTO Users(Username,Password,ProfilePicture,LastLoginTime,IsDeleted) VALUES
('ASD','asdASD',255,'2013-01-01',1),
('ASD','asdhgfh',255,'2013-01-01',1),
('ASD','asdf',255,'2013-01-01',0),
('ASD','asddfd',255,'2013-01-01',0),
('ASD','asddgf',255,'2013-01-01',1)

--13
CREATE TABLE Directors(
	Id INT PRIMARY KEY IDENTITY,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)
CREATE TABLE Genres(
	Id INT PRIMARY KEY IDENTITY,
	GenreName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)
CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Movies(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(id),
	CopyrightYear DATE NOT NULL,
	Lenght DECIMAL(15,2) NOT NULL,
	GenreId INT FOREIGN KEY REFERENCES Genres(id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(id),
	Rating DECIMAL (15,1),
	Notes NVARCHAR(MAX)
)

INSERT INTO Directors(DirectorName,Notes) VALUES
('John Grisham','ASD'),
('FAS','ASD'),
('BUQ','ASD'),
('ASD Grisham',NULL),
('Pepi Grisham',NULL)

INSERT INTO Genres(GenreName,Notes) VALUES
('Horror',NULL),
('Comedy',NULL),
('Action',NULL),
('Thriller',NULL),
('Fantasy',NULL)

INSERT INTO Categories(CategoryName,Notes) VALUES
('Rental',NULL),
('Not',NULL),
('Rental',NULL),
('Not',NULL),
('Rental',NULL)

INSERT INTO Movies(Title,DirectorId,CopyrightYear,Lenght,GenreId,CategoryId,Rating,Notes) VALUES
('ASD',2,'2001-02-02',2.23,3,1,5.5,'SI EBE MAIKATA'),
('ASD',2,'2001-02-02',2.23,3,1,5.5,'SI EBE MAIKATA'),
('ASD',2,'2001-02-02',2.23,3,1,5.5,'SI EBE MAIKATA'),
('ASD',2,'2001-02-02',2.23,3,1,5.5,'SI EBE MAIKATA'),
('ASD',2,'2001-02-02',2.23,3,1,5.5,'SI EBE MAIKATA')

--14
CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	CategoryName nvarchar(50) NOT NULL,
	DailyRate int,
	WeeklyRate int,
	MonthlyRate int NOT NULL,
	WeekendRate int
)

CREATE TABLE Cars
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Platenumber nvarchar(50) NOT NULL UNIQUE,
	Model nvarchar(255) NOT NULL,
	CarYear int NOT NULL,
	CategoryId nvarchar(255),
	Doors int,
	Picture ntext,
	Condition nvarchar(50) NOT NULL,
	Available INT NOT NULL
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	FirstName nvarchar(50) NOT NULL,
	LastName nvarchar(50) NOT NULL,
	Title nvarchar(255) NOT NULL,
	Notes nvarchar(255)
)

CREATE TABLE Customers
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	DriverLicenceNumber int NOT NULL UNIQUE,
	FullName nvarchar(255) NOT NULL,
	Address nvarchar(255),
	City nvarchar(255) NOT NULL,
	ZIPCode nvarchar(255),
	Notes nvarchar(255)
)

CREATE TABLE RentalOrders
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	EmployeeId INT NOT NULL UNIQUE,
	CustomerId INT NOT NULL UNIQUE,
	CarId INT NOT NULL,
	TankLevel INT,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage INT,
	StartDate DATE,
	EndDate DATE,
	TotalDays INT,
	RateApplied nvarchar(50),
	TaxRate nvarchar(50),
	OrderStatus nvarchar(255),
	Notes nvarchar(255)
)

INSERT INTO Categories(CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
VALUES('Somecategory', NULL, 3, 100, 2)
INSERT INTO Categories(CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
VALUES('SomeanotherCategory', 1, NULL, 900, NULL)
INSERT INTO Categories(CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
VALUES('TheLastCategory', 4, 5, 800, 35)

INSERT INTO Cars(Platenumber,Model,CarYear,CategoryId,Doors,Picture,Condition,Available)
VALUES('СА 2258 АС', 'BMW', 2017, NULL,4,NULL,'New', 10)
INSERT INTO Cars(Platenumber,Model,CarYear,CategoryId,Doors,Picture,Condition,Available)
VALUES('RA 2299 CA', 'AUDI', 2017, NULL,2,NULL,'New', 21)
INSERT INTO Cars(Platenumber,Model,CarYear,CategoryId,Doors,Picture,Condition,Available)
VALUES('EG 8888 GA', 'MERCEDES', 2017, NULL,4,NULL,'New', 9)

INSERT INTO Employees(FirstName,LastName,Title,Notes)
VALUES('Gosho','Peshov','Software Developer',NULL)
INSERT INTO Employees(FirstName,LastName,Title,Notes)
VALUES('Pesho','Goshov','Pilot',NULL)
INSERT INTO Employees(FirstName,LastName,Title,Notes)
VALUES('Mariika','Petrova','Doctor',NULL)

INSERT INTO Customers(DriverLicenceNumber, FullName, Address,City,ZIPCode,Notes)
VALUES(5821596,'Gosho it-to',NULL,'Sofia', NULL, NULL)
INSERT INTO Customers(DriverLicenceNumber, FullName, Address,City,ZIPCode,Notes)
VALUES(123513,'Pesho Peshov Peshov',NULL,'England', 'TN9T4U', NULL)
INSERT INTO Customers(DriverLicenceNumber, FullName, Address,City,ZIPCode,Notes)
VALUES(09834758,'Pesho Goshov Peshov',NULL,'Switzerland', NULL, NULL)

INSERT INTO RentalOrders(EmployeeId,CustomerId,CarId,TankLevel,KilometrageStart,KilometrageEnd,TotalKilometrage,StartDate,EndDate,TotalDays,RateApplied,TaxRate,OrderStatus,Notes)
VALUES(5315351, 1351, 5, NULL, 5000, 2351, 1231245, NULL,NULL,NULL,NULL,NULL,NULL,NULL)
INSERT INTO RentalOrders(EmployeeId,CustomerId,CarId,TankLevel,KilometrageStart,KilometrageEnd,TotalKilometrage,StartDate,EndDate,TotalDays,RateApplied,TaxRate,OrderStatus,Notes)
VALUES(53453, 643, 3, NULL, 567876, 12323, 3453453, NULL,NULL,NULL,NULL,NULL,NULL,NULL)
INSERT INTO RentalOrders(EmployeeId,CustomerId,CarId,TankLevel,KilometrageStart,KilometrageEnd,TotalKilometrage,StartDate,EndDate,TotalDays,RateApplied,TaxRate,OrderStatus,Notes)
VALUES(7859647, 123, 2, NULL, 12312, 543536, 367787, NULL,NULL,NULL,NULL,NULL,'DELIVERED',NULL)

--15

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	FirstName nvarchar(50) NOT NULL,
	LastName nvarchar(50),
	Title nvarchar(50) NOT NULL,
	Notes nvarchar(255)
)

CREATE TABLE Customers
(
	AccountNumber INT PRIMARY KEY IDENTITY(1,1),
	FirstName nvarchar(50) NOT NULL,
	LastName nvarchar(50) NOT NULL,
	PhoneNumber INT,
	EmergencyName nvarchar(255),
	EmergencyNumber INT NOT NULL,
	Notes nvarchar(255)
)

CREATE TABLE RoomStatus
(
	RoomType nvarchar(50) PRIMARY KEY NOT NULL,
	Notes nvarchar(255)
)

CREATE TABLE RoomTypes
(
	RoomType nvarchar(50) PRIMARY KEY NOT NULL,
	Notes nvarchar(255)
)


CREATE TABLE BedTypes
(
	BedType nvarchar(50) PRIMARY KEY NOT NULL,
	Notes nvarchar(255)
)

CREATE TABLE Rooms
(
	RoomNumber INT PRIMARY KEY IDENTITY(1,1),
	RoomType nvarchar(50) NOT NULL,
	BedType nvarchar(50) NOT NULL,
	Rate nvarchar(50),
	RoomStatus nvarchar(50),
	Notes nvarchar(255)
)

CREATE TABLE Payments
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	EmployeeId INT UNIQUE NOT NULL,
	PaymentDate date,
	AccountNumber INT NOT NULL,
	FirstDateOccupied date,
	LastDateOccupied date,
	TotalDays INT NOT NULL,
	AmountCharged INT NOT NULL,
	TaxRate INT,
	TaxAmount INT,
	PaymentTotal INT NOT NULL,
	Notes nvarchar(255)
)

CREATE TABLE Occupancies
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	EmployeeId INT UNIQUE NOT NULL,
	DateOccupied date,
	AccountNumber INT NOT NULL,
	RoomNumber INT NOT NULL,
	RateApplied INT,
	PhoneCharge INT,
	Notes nvarchar(255)
)

INSERT INTO Employees(FirstName,LastName,Title,Notes)
VALUES('Pesho', 'Peshov', 'Software Developer',NULL)
INSERT INTO Employees(FirstName,LastName,Title,Notes)
VALUES('Gosho', 'Peshov', 'Pilot',NULL)
INSERT INTO Employees(FirstName,LastName,Title,Notes)
VALUES('Pesho', 'Petrov', 'Engineer',NULL)

INSERT INTO Customers(FirstName,LastName,PhoneNumber,EmergencyName,EmergencyNumber, Notes)
VALUES('Pesho', 'Peshov', NULL, NULL, 112, NULL)
INSERT INTO Customers(FirstName,LastName,PhoneNumber,EmergencyName,EmergencyNumber, Notes)
VALUES('Pesho', 'Petrov', NULL, NULL, 911, NULL)
INSERT INTO Customers(FirstName,LastName,PhoneNumber,EmergencyName,EmergencyNumber, Notes)
VALUES('Pesho', 'Peshov', NULL, NULL, 912, NULL)

INSERT INTO RoomStatus(RoomType, Notes)
VALUES('Free', NULL)
INSERT INTO RoomStatus(RoomType, Notes)
VALUES('Reserved', NULL)
INSERT INTO RoomStatus(RoomType, Notes)
VALUES('Currently not available', NULL)

INSERT INTO RoomTypes(RoomType,Notes)
VALUES('Luxury', NULL)
INSERT INTO RoomTypes(RoomType,Notes)
VALUES('Standard', NULL)
INSERT INTO RoomTypes(RoomType,Notes)
VALUES('Small', NULL)

INSERT INTO BedTypes(BedType,Notes)
VALUES('LARGE', NULL)
INSERT INTO BedTypes(BedType,Notes)
VALUES('Medium', NULL)
INSERT INTO BedTypes(BedType,Notes)
VALUES('Small', NULL)

INSERT INTO Rooms(RoomType, BedType, Rate,RoomStatus,Notes)
VALUES('Luxury', 'Large', 'Perfect for rich people', 'Available', NULL)
INSERT INTO Rooms(RoomType, BedType, Rate,RoomStatus,Notes)
VALUES('Medium', 'Medium', NULL, 'Not available', NULL)
INSERT INTO Rooms(RoomType, BedType, Rate,RoomStatus,Notes)
VALUES('Economy', 'Small', NULL, 'Available', NULL)

INSERT INTO Payments(EmployeeId,PaymentDate,AccountNumber,FirstDateOccupied,LastDateOccupied,TotalDays,AmountCharged,TaxRate,TaxAmount,PaymentTotal,Notes)
VALUES(231, NULL, 2311, NULL,NULL, 7, 5000, 0,0,5000,NULL)
INSERT INTO Payments(EmployeeId,PaymentDate,AccountNumber,FirstDateOccupied,LastDateOccupied,TotalDays,AmountCharged,TaxRate,TaxAmount,PaymentTotal,Notes)
VALUES(321, NULL, 3211, NULL,NULL, 7, 5000, 0,2000,7000,NULL)
INSERT INTO Payments(EmployeeId,PaymentDate,AccountNumber,FirstDateOccupied,LastDateOccupied,TotalDays,AmountCharged,TaxRate,TaxAmount,PaymentTotal,Notes)
VALUES(999, NULL, 9989, NULL,NULL, 7, 5000, 0,50,5500,NULL)

INSERT INTO Occupancies(EmployeeId,DateOccupied,AccountNumber,RoomNumber,RateApplied,PhoneCharge,Notes)
VALUES(991, NULL, 534, 8, NULL,NULL,NULL)
INSERT INTO Occupancies(EmployeeId,DateOccupied,AccountNumber,RoomNumber,RateApplied,PhoneCharge,Notes)
VALUES(561, NULL, 75, 9, NULL,NULL,NULL)
INSERT INTO Occupancies(EmployeeId,DateOccupied,AccountNumber,RoomNumber,RateApplied,PhoneCharge,Notes)
VALUES(135, NULL, 8, 10, NULL,NULL,NULL)

--19
SELECT * FROM Towns
SELECT * FROM Departments
SELECT * FROM Employees

--20
SELECT * FROM Towns ORDER BY Name
SELECT * FROM Departments ORDER BY Name
SELECT * FROM Employees ORDER BY Salary DESC

--21
SELECT Name FROM Towns ORDER BY Name
SELECT Name FROM Departments ORDER BY Name
SELECT FirstName, LastName, JobTitle, Salary FROM Employees ORDER BY Salary DESC

--22
UPDATE Employees
SET Salary+=Salary * 0.1
SELECT Salary FROM Employees

--23
UPDATE Payments
SET TaxRate-=TaxRate*0.03
SELECT TaxRate FROM Payments

--24
TRUNCATE TABLE Occupancies

-25