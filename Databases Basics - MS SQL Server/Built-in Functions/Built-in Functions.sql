--1
SELECT FirstName,LastName  FROM Employees WHERE FirstName LIKE 'SA%'
--2
SELECT FirstName,LastName  FROM Employees WHERE LastName LIKE '%ei%'
--3
SELECT FirstName FROM Employees
WHERE DepartmentID IN (3,10) AND (HireDate>=1995 OR HireDate<=2005) 
--4
SELECT FirstName,LastName FROM Employees 
 WHERE JobTitle 
 NOT LIKE '%engineer%'
--5
SELECT Name FROM Towns 
 WHERE LEN(Name) = 5 OR LEN(Name) = 6
 ORDER BY Name
--6
SELECT TownId,Name FROM Towns 
 WHERE Name LIKE 'M%' OR Name LIKE 'K%' OR Name LIKE 'B%' OR Name LIKE 'E%'
 ORDER BY Name
--7
SELECT TownId,Name FROM Towns 
 WHERE Name NOT LIKE 'B%' AND Name NOT LIKE 'R%' AND Name NOT LIKE 'D%'
 ORDER BY Name
--8
CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT FirstName,LastName FROM Employees
WHERE HireDate > '2001'
--9
SELECT FirstName,LastName FROM Employees
WHERE LEN(LastName) = 5
--10
SELECT CountryName, IsoCode AS [ISO Code] FROM Countries
WHERE CountryName LIKE '%A%A%A%'
ORDER BY [ISO Code]

--11
SELECT PeakName,RiverName,
LOWER(PeakName + RIGHT(RiverName,LEN(RiverName) -1)) AS [Mix] FROM Rivers,Peaks
WHERE RIGHT(PeakName,1) = LEFT(RiverName,1)
ORDER BY Mix
--12
SELECT TOP(50) Name,FORMAT(Start,'yyyy-MM-dd') AS Start FROM Games
WHERE YEAR(Start) BETWEEN 2011 AND 2012
ORDER BY Start,Name
--13
SELECT UserName,SUBSTRING(Email,CHARINDEX('@',Email,0) + 1,LEN(Email)) AS [Email Provider] FROM Users
ORDER BY [Email Provider],Username
--14
SELECT UserName,IpAddress FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY Username
--15
SELECT Name AS [Game],
CASE
WHEN DATEPART(HOUR,Start) BETWEEN 0 AND 11 THEN 'Morning'
WHEN DATEPART(HOUR,Start) BETWEEN 12 AND 17 THEN 'Afternoon'
WHEN DATEPART(HOUR,Start) BETWEEN 18 AND 23 THEN 'Evening'
END AS [Part of the Day],
CASE
WHEN Duration <= 3 THEN 'Extra Short'
WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
WHEN DURATION > 6 THEN 'Long'
ELSE 'Extra Long'
END AS [Duration]
FROM Games
ORDER BY [Game],[Duration],[Part of the Day]
--16
SELECT ProductName,OrderDate,
DATEADD(DAY,3,OrderDate) AS [Pay Due],
DATEADD(MONTH,1,OrderDate) AS [Deliver Due]
FROM Orders