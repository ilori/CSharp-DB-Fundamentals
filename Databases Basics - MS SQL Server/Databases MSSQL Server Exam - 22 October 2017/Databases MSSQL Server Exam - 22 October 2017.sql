CREATE DATABASE ReportService
GO
USE ReportService
GO

--1
CREATE TABLE Users(
	Id INT PRIMARY KEY IDENTITY,
	Username NVARCHAR(30) UNIQUE NOT NULL,
	Password NVARCHAR(50) NOT NULL,
	Name NVARCHAR(50),
	Gender CHAR(1) CHECK(Gender = 'M' OR Gender = 'F'),
	BirthDate DATETIME,
	Age INT,
	Email NVARCHAR(50) NOT NULL
)

CREATE TABLE Departments(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL
)
CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(25),
	LastName NVARCHAR(25),
	Gender CHAR(1) CHECK(Gender = 'M' OR Gender = 'F'),
	BirthDate DATETIME,
	Age INT,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL
)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE Status(
	Id INT PRIMARY KEY IDENTITY,
	Label NVARCHAR(30) NOT NULL
)

CREATE TABLE Reports(
	Id INT PRIMARY KEY IDENTITY,
	CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
	StatusId INT NOT NULL FOREIGN KEY REFERENCES Status(Id),
	OpenDate DATETIME NOT NULL,
	CloseDate DATETIME,
	Description NVARCHAR(200),
	UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id)
)
--2
INSERT INTO Employees (FirstName,LastName,Gender,BirthDate,DepartmentId) VALUES
('Marlo','O’Malley','M','9/21/1958',1),
('Niki','Stanaghan','F','11/26/1969',4),
('Ayrton','Senna','M','03/21/1960 ',9),
('Ronnie','Peterson','M','02/14/1944',9),
('Giovanna','Amati','F','07/20/1959',5)


INSERT INTO Reports (CategoryId,StatusId,OpenDate,CloseDate,Description,UserId,EmployeeId) VALUES
(1,1,'04/13/2017',NULL,'Stuck Road on Str.133',6,2),
(6,3,'09/05/2015','12/06/2015','Charity trail running',3,5),
(14,2,'09/07/2015',NULL,'Falling bricks on Str.58',5,2),
(4,3,'07/03/2017','07/03/2017','Cut off streetlight on Str.11',1,1)
--3
UPDATE Reports
SET StatusId = 2
WHERE StatusId = 1 AND CategoryId = 4
--4
DELETE FROM Reports
WHERE StatusId = 4
--5
SELECT Username,Age FROM Users
ORDER BY Age,Username DESC
--6
SELECT Description,OpenDate FROM Reports
WHERE EmployeeId IS NULL
ORDER BY OpenDate,Description
--7
SELECT FirstName,LastName,Description,FORMAT(OpenDate,'yyyy-MM-dd') AS OpenDate FROM Reports AS r
JOIN Employees AS e ON e.Id = r.EmployeeId
ORDER BY EmployeeId,OpenDate,r.Id
--8
SELECT c.Name,COUNT(r.CategoryId) AS ReportsNumber FROM Categories AS c
JOIN Reports AS r ON r.CategoryId = c.Id
GROUP BY c.Name
ORDER BY ReportsNumber DESC,c.Name
--9
SELECT c.Name,
		(SELECT COUNT(e1.DepartmentId) FROM Departments AS d1
		 JOIN Employees AS e1 ON e1.DepartmentId = d1.Id
		 WHERE d1.Id = c.DepartmentId
		 GROUP BY d1.Id
		) AS [Employees Number]
 FROM Categories AS c
 ORDER BY c.Name
--10
SELECT e.FirstName + ' ' + e.LastName AS Name,ISNULL(COUNT(DISTINCT(r.UserId)),0) AS [Users Number] FROM Employees AS e
LEFT JOIN Reports AS r ON r.EmployeeId = e.Id
GROUP BY e.FirstName + ' ' + e.LastName
ORDER BY [Users Number] DESC,Name
--11
SELECT OpenDate,Description,u.Email AS [Reporter Email] FROM Reports AS r
JOIN Users AS u ON u.Id = r.UserId
JOIN Categories AS c ON c.Id = r.CategoryId
JOIN Departments AS d ON d.id = c.DepartmentId
WHERE r.CloseDate IS NULL AND (DATALENGTH(r.Description) > 20 AND r.Description LIKE '%str%')  AND c.DepartmentId IN (1,4,5)
ORDER BY OpenDate,[Reporter Email],r.Id
--12
SELECT DISTINCT c.Name FROM Users AS u
JOIN Reports AS r ON r.UserId = u.Id
JOIN Categories AS c ON c.Id = r.CategoryId
WHERE FORMAT(r.OpenDate,'dd-MM') = FORMAT(u.BirthDate,'dd-MM')
ORDER BY c.Name
--13
SELECT DISTINCT u.Username FROM Users AS u
FULL JOIN Reports AS r ON r.UserId = u.Id
FULL JOIN Categories AS c ON c.Id = r.CategoryId
WHERE ((SUBSTRING(Username,1,1)) LIKE '[0-9]%' AND (SUBSTRING(u.Username,1,1) = r.CategoryId))
OR (SUBSTRING(Username, LEN(Username), 1) LIKE '%[0-9]' AND SUBSTRING(Username, LEN(Username), 1) = r.CategoryId)
ORDER BY u.Username
--15
SELECT finished.Name, CASE
                       WHEN finished.[Average Duration] IS NULL THEN 'no info'
					   ELSE finished.[Average Duration] 
					  END					
					 FROM
(
SELECT d.Name,CAST(AVG(DATEDIFF(DAY,OpenDate,CloseDate)) AS varchar(10)) AS [Average Duration] FROM Departments AS d
JOIN Categories AS c ON c.DepartmentId = d.Id
JOIN Reports AS r ON r.CategoryId = c.Id
GROUP BY d.Id,d.Name) as finished
ORDER BY finished.Name
--16
SELECT FinalOrder.[Department Name],FinalOrder.[Category Name],CAST(ROUND(100.00/FinalOrder.TotalPercent * FinalOrder.Pecent,0) AS int) AS [Percentage] FROM
(SELECT d.Name AS [Department Name],c.Name AS [Category Name],COUNT(c.Id) AS Pecent,(SELECT COUNT(c1.Id) AS Pecent FROM Reports AS r1
JOIN Categories AS c1 ON c1.Id = r1.CategoryId
JOIN Departments AS d1 ON d1.Id = c1.DepartmentId
WHERE d1.Name = d.Name
GROUP BY d1.Id) AS TotalPercent FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
JOIN Departments AS d ON d.Id = c.DepartmentId
GROUP BY d.Name,c.Name
) AS FinalOrder
GROUP BY FinalOrder.[Department Name],FinalOrder.[Category Name],CAST(ROUND(100.00/FinalOrder.TotalPercent * FinalOrder.Pecent,0) AS int)
ORDER BY FinalOrder.[Department Name],FinalOrder.[Category Name],Percentage
--17
CREATE FUNCTION udf_GetReportsCount(@employeeId INT, @statusId INT)
RETURNS INT
AS
BEGIN
	DECLARE @Reports INT = (SELECT COUNT(r.EmployeeId) FROM Reports AS r
	WHERE r.EmployeeId= @employeeId AND r.StatusId= @statusId)
	RETURN @Reports
END
--18
CREATE PROC usp_AssignEmployeeToReport @employeeId INT, @reportId INT
AS
BEGIN
	BEGIN TRAN
	DECLARE @UserDepartment INT = (SELECT DepartmentId FROM Employees WHERE Id = @employeeId)
	DECLARE @ReportsDepartment INT = (SELECT c.DepartmentId FROM Reports AS r JOIN Categories AS c ON c.Id = r.CategoryId WHERE r.Id = @reportId)

	UPDATE Reports
	SET EmployeeId = @employeeId
	WHERE Id = @reportId
	
	IF(@UserDepartment <> @ReportsDepartment)
	BEGIN
		RAISERROR('Employee doesn''t belong to the appropriate department!',16,1);
		ROLLBACK
	END
	COMMIT
END
--19
CREATE TRIGGER tr_Reports ON Reports AFTER UPDATE
AS 
BEGIN
	UPDATE Reports
	SET StatusId = 3
	WHERE CloseDate IN (SELECT CloseDate FROM inserted) 
END