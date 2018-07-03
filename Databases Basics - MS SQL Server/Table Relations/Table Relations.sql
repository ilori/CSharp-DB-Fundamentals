--1
CREATE TABLE Persons (
	PersonID INT PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	Salary DECIMAL(15,2),
	PassportID INT UNIQUE
)

CREATE TABLE Passports(
	PassportID INT PRIMARY KEY,
	PassportNumber VARCHAR(50) UNIQUE
)
INSERT INTO Passports VALUES 
(101, 'N34FG21B'), 
(102, 'K65LO4R7'), 
(103, 'ZE657QP2')

INSERT INTO Persons VALUES 
(1, 'Roberto', 43330.00, 102),
(2, 'Tom', 56100.00, 103),
(3, 'Yana', 60200.00, 101)

ALTER TABLE Persons
ADD CONSTRAINT PK_Persons_Passports FOREIGN KEY (PassportID) REFERENCES Passports(PassportID)
--2
CREATE TABLE Models(
	ModelID INT PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL,
	ManufacturerID INT
)

CREATE TABLE Manufacturers(
	ManufacturerID INT PRIMARY KEY,
	Name NVARCHAR(50),
	EstablishedOn DATE
)

INSERT INTO Manufacturers VALUES
	(1, 'BMW', '07/03/1916'),
	(2, 'Tesla', '01/01/2003'),
	(3, 'Lada', '01/05/1966')

INSERT INTO Models VALUES
	(101, 'X1', 1),
	(102, 'i6', 1),
	(103, 'Model S', 2),
	(104, 'Model X', 2),
	(105, 'Model 3', 2),
	(106, 'Nova', 3)

ALTER TABLE Models
ADD CONSTRAINT FK_Models_Manufacturers FOREIGN KEY (ManufacturerID)
REFERENCES Manufacturers(ManufacturerID)

--3
 CREATE TABLE Students(
	StudentID INT PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL
 )

 CREATE TABLE Exams (
	ExamID INT PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL
 )

 CREATE TABLE StudentsExams (
	StudentID INT,
	ExamID INT

	CONSTRAINT PK_StudentID_ExamID PRIMARY KEY (StudentID,ExamID) 

	CONSTRAINT FK_StudentID FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID),

	CONSTRAINT FK_ExamID FOREIGN KEY (ExamID)
	REFERENCES Exams(ExamID)
)

INSERT INTO Students (StudentID,Name) VALUES
(1,'Mila'),
(2,'Toni'),
(3,'Ron')


INSERT INTO Exams (ExamID,Name) VALUES
(101,'SpringMVC'),
(102,'Neo4j'),
(103,'Oracle 11g')

INSERT INTO StudentsExams (StudentID,ExamID) VALUES
(1,101),
(1,102),
(2,101),
(3,103),
(2,102),
(2,103)

--4
CREATE TABLE Teachers (
	TeacherID INT,
	Name NVARCHAR(50) NOT NULL,
	ManagerID INT


	CONSTRAINT PK_TeacherID PRIMARY KEY (TeacherID),

	CONSTRAINT FK_ManagerID FOREIGN KEY (ManagerID) REFERENCES Teachers(TeacherID)
)

INSERT INTO Teachers (TeacherID,Name,ManagerID) VALUES
(101,'John',NULL),
(102,'Maya',106),
(103,'Silvia',106),
(104,'Ted',105),
(105,'Mark',101),
(106,'Greta',101)
--5
CREATE TABLE Cities(
	CityID INT PRIMARY KEY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Customers(
	CustomerID INT PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	Birthday DATE,
	CityID INT

	CONSTRAINT FK_Customers_Cities FOREIGN KEY (CityID)
	REFERENCES Cities(CityID)
)

CREATE TABLE Orders(
	OrderID INT PRIMARY KEY,
	CustomerID INT NOT NULL

	CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID)
	REFERENCES Customers(CustomerID)
)

CREATE TABLE ItemTypes(
	ItemTypeID INT PRIMARY KEY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Items (
	ItemID INT PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	ItemTypeID INT

	CONSTRAINT FK_Items_ItemTypes FOREIGN KEY (ItemTypeID)
	REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE OrderItems(
	OrderID INT NOT NULL,
	ItemID INT NOT NULL

	CONSTRAINT PK_OrderItems PRIMARY KEY (OrderID,ItemID),

	CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderID)
	REFERENCES Orders(OrderID),

	CONSTRAINT FK_OrderItems_Items FOREIGN KEY (ItemID)
	REFERENCES Items(ItemID)
)
--6
CREATE TABLE Majors(
	MajorID INT PRIMARY KEY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Students(
	StudentID INT PRIMARY KEY,
	StudentNumber INT NOT NULL,
	StudentName VARCHAR(50) NOT NULL,
	MajorID INT

	CONSTRAINT FK_Students_Majors FOREIGN KEY (MajorID)
	REFERENCES Majors(MajorID)
)
CREATE TABLE Payments(
	PaymentID INT PRIMARY KEY,
	PaymentDate DATE,
	PaymentAmount DECIMAL(15,2),
	StudentID INT

	CONSTRAINT FK_Payments_Students FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID)
)

CREATE TABLE Subjects(
	SubjectID INT PRIMARY KEY,
	SubjectName VARCHAR(50) NOT NULL
)

CREATE TABLE Agenda (
	StudentID INT NOT NULL,
	SubjectID INT NOT NULL,

	CONSTRAINT PK_Agenda PRIMARY KEY (StudentID,SubjectID),

	CONSTRAINT FK_Agenda_Students FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID),

	CONSTRAINT FK_Agenda_Subjects FOREIGN KEY (SubjectID)
	REFERENCES Subjects(SubjectID)
)
--9
SELECT MountainRange,PeakName,Elevation FROM Mountains AS m
JOIN Peaks AS p ON m.Id = p.MountainId
WHERE MountainRange = 'Rila'
ORDER BY Elevation DESC