--1
SELECT TOP(5) EmployeeID,JobTitle,a.AddressID,AddressText FROM Addresses as a
INNER JOIN Employees AS e
ON a.AddressID = e.AddressID
ORDER BY AddressID
--2
SELECT TOP(50) FirstName,LastName,(SELECT Name AS Town FROM Towns AS t WHERE t.TownID = a.TownID) AS Towns,a.AddressText FROM Addresses AS a
INNER JOIN Employees AS e
ON a.AddressID = e.AddressID
ORDER BY FirstName,LastName
--3
SELECT EmployeeID,FirstName,LastName,Name FROM Departments AS d
INNER JOIN Employees AS e
ON d.DepartmentID = e.DepartmentID
WHERE d.DepartmentID = 3
ORDER BY EmployeeID
--4
SELECT TOP(5) EmployeeID,FirstName,Salary,Name AS DepartmentName FROM Employees AS e
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE Salary > 15000
ORDER BY e.DepartmentID
--5
SELECT TOP(3) e.EmployeeID,FirstName FROM EmployeesProjects AS ep
RIGHT JOIN Employees  as e
ON ep.EmployeeID = e.EmployeeID
WHERE ep.ProjectID IS NULL
ORDER BY EmployeeID
--6
SELECT FirstName,LastName,HireDate,Name AS DeptName FROM Employees AS e
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE e.HireDate > '1999' AND d.Name = 'Sales' OR d.Name = 'Finance'
ORDER BY HireDate
--7
SELECT TOP 5 e.EmployeeID,FirstName,p.Name AS ProjectName FROM Employees AS e
JOIN EmployeesProjects AS ep
ON e.EmployeeID = ep.EmployeeID
JOIN Projects AS p
ON ep.ProjectID = p.ProjectID
WHERE p.StartDate > '08.13.2002' AND p.EndDate IS NULL
ORDER BY e.EmployeeID
--8
SELECT e.EmployeeID,FirstName,
     IIF(StartDate > '2005',NULL,Name) 
      AS PojectName FROM Employees AS e
	JOIN EmployeesProjects AS ep
	ON e.EmployeeID=ep.EmployeeID
	JOIN Projects AS p
	  ON ep.ProjectID = p.ProjectID
   WHERE e.EmployeeID = 24
--9
SELECT e.EmployeeID,e.FirstName,e.ManagerID,e1.FirstName FROM Employees AS e
JOIN Employees AS e1
ON e1.EmployeeID = e.ManagerID
WHERE e.ManagerID IN (3,7)
ORDER BY e.EmployeeID

--10
SELECT TOP 50 e.EmployeeID,e.FirstName + ' ' + e.LastName AS EmployeeName,ep.FirstName + ' ' + ep.LastName AS ManagerName,Name AS DepartmentName FROM Employees AS e
JOIN Employees AS ep
ON ep.EmployeeID = e.ManagerID
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID
--11
SELECT TOP 1 AVG(Salary) AS MinAverageSalary FROM Employees
GROUP BY DepartmentID
ORDER BY MinAverageSalary
--12
SELECT mc.CountryCode,m.MountainRange,p.PeakName,p.Elevation FROM MountainsCountries AS mc
JOIN Mountains AS m
ON mc.MountainId = m.Id
JOIN Peaks AS p
ON p.MountainId = mc.MountainId
WHERE mc.CountryCode = 'BG' AND p.Elevation > 2835
ORDER BY p.Elevation DESC
--13
SELECT CountryCode,COUNT(*) AS MountainsRanges FROM MountainsCountries
WHERE CountryCode IN ('BG','US','RU')
GROUP BY CountryCode
--14
SELECT TOP 5 c.CountryName,r.RiverName FROM Countries AS c
JOIN Continents AS cont
ON c.ContinentCode = cont.ContinentCode
LEFT JOIN CountriesRivers AS cr
ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r
ON r.Id = cr.RiverId
WHERE cont.ContinentName = 'Africa'
ORDER BY c.CountryName
--15
WITH CCYContUsage_CTE (ContinentCode, CurrencyCode, CurrencyUsage) AS (
  SELECT ContinentCode, CurrencyCode, COUNT(CountryCode) AS CurrencyUsage
  FROM Countries 
  GROUP BY ContinentCode, CurrencyCode
  HAVING COUNT(CountryCode) > 1  
)
SELECT ContMax.ContinentCode, ccy.CurrencyCode, ContMax.CurrencyUsage 
  FROM
  (SELECT ContinentCode, MAX(CurrencyUsage) AS CurrencyUsage
   FROM CCYContUsage_CTE 
   GROUP BY ContinentCode) AS ContMax
JOIN CCYContUsage_CTE AS ccy 
ON (ContMax.ContinentCode = ccy.ContinentCode AND ContMax.CurrencyUsage = ccy.CurrencyUsage)
ORDER BY ContMax.ContinentCode

--16
SELECT  COUNT(*) AS CountryCode FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
WHERE MountainId IS NULL
GROUP BY MountainId
--17
SELECT TOP 5 c.CountryName,MAX(p.Elevation) AS HighestPeakElevattion,MAX(r.Length) AS LongestRiverLenght FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
LEFT JOIN Peaks AS p ON p.MountainId = m.Id
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
GROUP BY c.CountryName
ORDER BY HighestPeakElevattion DESC,LongestRiverLenght DESC,c.CountryName
--18
WITH PeaksMountains_CTE(Country, PeakName, Elevation, Mountain) AS (
  SELECT c.CountryName, p.PeakName, p.Elevation, m.MountainRange
  FROM Countries AS c
  LEFT JOIN MountainsCountries as mc ON c.CountryCode = mc.CountryCode
  LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
  LEFT JOIN Peaks AS p ON p.MountainId = m.Id
)
SELECT TOP 5
  TopElevations.Country AS Country,
  ISNULL(pm.PeakName, '(no highest peak)') AS HighestPeakName,
  ISNULL(TopElevations.HighestElevation, 0) AS HighestPeakElevation,	
  ISNULL(pm.Mountain, '(no mountain)') AS Mountain
FROM 
  (SELECT Country, MAX(Elevation) AS HighestElevation
   FROM PeaksMountains_CTE 
   GROUP BY Country) AS TopElevations
LEFT JOIN PeaksMountains_CTE AS pm 
ON (TopElevations.Country = pm.Country AND TopElevations.HighestElevation = pm.Elevation)
ORDER BY Country, HighestPeakName
