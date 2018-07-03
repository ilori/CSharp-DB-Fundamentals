--1
SELECT COUNT(*) AS Count FROM WizzardDeposits

--2
SELECT MAX(MagicWandSize) AS [LongestMagicWand] FROM WizzardDeposits

--3
SELECT DepositGroup,MAX(MagicWandSize) AS [LongestMagicWand] 
FROM WizzardDeposits
GROUP BY DepositGroup

--4
SELECT  DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
HAVING AVG(MagicWandSize) < 21

--5
SELECT DepositGroup,SUM(DepositAmount) AS [TotalSum] 
FROM WizzardDeposits
GROUP BY DepositGroup

--6
SELECT DepositGroup,SUM(DepositAmount) AS [TotalSum] 
FROM WizzardDeposits
WHERE MagicWandCreator LIKE 'Ollivander family'
GROUP BY DepositGroup

--7
SELECT DepositGroup,SUM(DepositAmount) AS [TotalSum] 
FROM WizzardDeposits
WHERE MagicWandCreator LIKE 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) <= 150000
ORDER BY TotalSum DESC

--8
SELECT DepositGroup,MagicWandCreator,MIN(DepositCharge) AS MinDepositCharge
FROM WizzardDeposits
GROUP BY DepositGroup,MagicWandCreator
ORDER BY MagicWandCreator,DepositGroup

--9
SELECT ageGroups.AgeGroup, COUNT(*) FROM
(
SELECT 
CASE
WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
WHEN Age >= 61 THEN '[61+]'
END AS AgeGroup
FROM WizzardDeposits
) AS ageGroups
GROUP BY ageGroups.AgeGroup

--10
SELECT DISTINCT SUBSTRING(FirstName,1,1) AS FirstLetter FROM WizzardDeposits
WHERE DepositGroup LIKE 'Troll Chest'
ORDER BY FirstLetter

--11
SELECT DepositGroup,IsDepositExpired,AVG(DepositInterest) FROM WizzardDeposits
WHERE DepositStartDate > '1985'
GROUP BY DepositGroup,IsDepositExpired
ORDER BY DepositGroup DESC, IsDepositExpired

--12
SELECT SUM(wizardDeposit.Difference) AS SumDifference FROM
(
SELECT FirstName, DepositAmount,
LEAD(FirstName) OVER (ORDER BY Id) AS GuestWizard,
LEAD(DepositAmount) OVER (ORDER BY Id) AS GuestDeposit,
DepositAmount - LEAD(DepositAmount) OVER (ORDER BY Id) AS Difference 
FROM WizzardDeposits
) AS wizardDeposit

--13
SELECT DepartmentID,SUM(Salary) AS TotalSalary FROM Employees
GROUP BY DepartmentID
ORDER BY DepartmentID

--14
SELECT DepartmentID,MIN(Salary) AS MinSalary FROM Employees
WHERE (DepartmentID = 2 OR DepartmentID = 5 OR DepartmentID = 7)
AND HireDate > '2000'
GROUP BY DepartmentID

--15
SELECT * INTO NewTable FROM Employees
WHERE Salary > 30000

DELETE FROM NewTable
WHERE ManagerId = 42

UPDATE NewTable
SET Salary += 5000
WHERE DepartmentID = 1
SELECT DepartmentID, AVG(Salary) AS AverageSalary FROM NewTable
GROUP BY DepartmentID


--16
SELECT DepartmentID,MAX(Salary) AS MaxSalary FROM Employees
GROUP BY DepartmentID
HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000

--17
SELECT COUNT(EmployeeID) FROM Employees
WHERE ManagerID IS NULL

--18
SELECT Salaries.DepartmentID, Salaries.Salary FROM
(
SELECT DepartmentId,
MAX(Salary) AS Salary,
DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS Rank
FROM Employees
GROUP BY DepartmentID, Salary
)AS Salaries 
WHERE Rank=3

--19
SELECT TOP(10) FirstName,LastName,DepartmentID FROM Employees AS e1
WHERE e1.Salary > (
SELECT AVG(Salary) FROM Employees AS e2
WHERE e1.DepartmentID= e2.DepartmentID
GROUP BY e2.DepartmentID
)
ORDER BY DepartmentID
