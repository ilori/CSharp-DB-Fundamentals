--1
CREATE PROC usp_GetEmployeesSalaryAbove35000
AS
SELECT FirstName,LastName FROM Employees
WHERE Salary > 35000
--2
CREATE PROC usp_GetEmployeesSalaryAboveNumber @Salary DECIMAL(18,2)
AS
SELECT FirstName,LastName FROM Employees
WHERE Salary >= @Salary

--3
CREATE PROC usp_GetTownsStartingWith @TownName NVARCHAR(50)
AS
SELECT Name AS Town FROM Towns
WHERE Name LIKE CONCAT(@TownName,'%')
--4
CREATE PROC usp_GetEmployeesFromTown @TownName NVARCHAR(MAX)
AS
SELECT FirstName,LastName FROM Employees AS e
JOIN Addresses AS a ON a.AddressID = e.AddressID
JOIN Towns AS t ON t.TownID = a.TownID
WHERE t.Name = @TownName
--5
CREATE FUNCTION ufn_GetSalaryLevel(@Salary DECIMAL(18,4))
RETURNS VARCHAR(10)
AS
BEGIN
	DECLARE @Message VARCHAR(10) = '';
	IF(@Salary < 30000)
	BEGIN
		SET @Message = 'Low';
	END
	ELSE IF(@Salary BETWEEN 30000 AND 50000)
	BEGIN
		SET @Message = 'Average';
	END
	ELSE
	BEGIN
		SET @Message = 'High' 
	END
	RETURN @Message
END
--6
CREATE PROC usp_EmployeesBySalaryLevel @SalaryLevel NVARCHAR(50)
AS
SELECT FirstName,LastName FROM Employees
WHERE dbo.ufn_GetSalaryLevel(Salary) = @SalaryLevel
--7
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(max), @word VARCHAR(max))
RETURNS BIT
AS
BEGIN
  DECLARE @isComprised BIT = 0;
  DECLARE @currentIndex INT = 1;
  DECLARE @currentChar CHAR;

  WHILE(@currentIndex <= LEN(@word))
  BEGIN

    SET @currentChar = SUBSTRING(@word, @currentIndex, 1);
    IF(CHARINDEX(@currentChar, @setOfLetters) = 0)
      RETURN @isComprised;
    SET @currentIndex += 1;

  END

  RETURN @isComprised + 1;

END
--8
CREATE PROC usp_DeleteEmployeesFromDepartment @departmentID INT
AS
ALTER TABLE Departments
ALTER COLUMN ManagerID INT NULL

DELETE FROM EmployeesProjects
WHERE EmployeeID IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @departmentID)

UPDATE Employees
SET ManagerID = NULL
WHERE ManagerID IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @departmentID)

UPDATE Departments
SET ManagerID = NULL
WHERE ManagerID IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @departmentID)

DELETE FROM Employees
WHERE EmployeeID IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @departmentID)

DELETE FROM Departments
WHERE DepartmentID = @departmentID

DELETE FROM Departments
WHERE DepartmentID = @departmentId
SELECT COUNT(*) AS Count FROM Employees AS e
JOIN Departments AS d
ON d.DepartmentID = e.DepartmentID
WHERE e.DepartmentID = @departmentId
--9
CREATE PROC usp_GetHoldersFullName
AS
SELECT FirstName + ' ' + LastName AS [Full Name] FROM AccountHolders
--10
CREATE PROC usp_GetHoldersWithBalanceHigherThan @Money DECIMAL(18,2)
AS
SELECT FirstName AS [First Name],LastName AS [Last Name]
FROM
(SELECT TOP 1000 FirstName,LastName,SUM(Balance) AS TotalMoney FROM Accounts AS a
JOIN AccountHolders AS ap ON ap.Id = a.AccountHolderId
GROUP BY ap.id,FirstName,LastName
HAVING SUM(Balance) > @Money
ORDER BY ap.LastName,ap.FirstName
) AS FinalResult
--11
CREATE FUNCTION ufn_CalculateFutureValue (@sum DECIMAL(15,2),@yearlyRate FLOAT,@numberOfYears INT)
RETURNS DECIMAL(15,4)
BEGIN
	RETURN @sum * POWER(1 + @yearlyRate, @numberOfYears);
END

--12
CREATE PROC usp_CalculateFutureValueForAccount @accountID INT,@yearlyRate FLOAT
AS

SELECT ah.Id,ah.FirstName,ah.LastName,a.Balance,dbo.ufn_CalculateFutureValue(a.Balance,@yearlyRate,5) 
AS [Balance in 5 years] 
FROM AccountHolders AS ah
JOIN Accounts AS a ON a.AccountHolderId = ah.Id
WHERE a.Id = @accountID
--13
CREATE FUNCTION ufn_CashInUsersGames (@gameName nvarchar(50))
RETURNS table
AS
RETURN (
  WITH CTE_CashInRows (Cash, RowNumber) AS (
    SELECT ug.Cash, ROW_NUMBER() OVER (ORDER BY ug.Cash DESC)
    FROM UsersGames AS ug
    JOIN Games AS g ON ug.GameId = g.Id
    WHERE g.Name = @gameName
  )
  SELECT SUM(Cash) AS SumCash
  FROM CTE_CashInRows
  WHERE RowNumber % 2 = 1
)
