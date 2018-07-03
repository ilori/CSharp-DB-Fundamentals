--1
CREATE TRIGGER tr_Accounts ON Accounts FOR UPDATE
AS
BEGIN
	INSERT INTO Logs (AccountId,OldSum,NewSum)
	SELECT i.AccountHolderId,d.Balance,i.Balance FROM inserted i
	JOIN deleted AS d ON d.Id = i.Id
END
--2
CREATE TRIGGER tr_SendEmailAfterUpdate ON Accounts AFTER UPDATE
AS
BEGIN
    INSERT INTO NotificationEmails (Recipient, Subject, Body)
         SELECT d.AccountHolderId,
                CONCAT('Balance change for account: ', CAST(d.AccountHolderId AS VARCHAR(5))),
                CONCAT('On ', CAST(GETDATE() AS VARCHAR(50)) ,' your balance was changed from ' , CAST(d.Balance AS VARCHAR(50)) ,' to ' , CAST(i.Balance AS VARCHAR(50)) , '.')
           FROM deleted AS d
     INNER JOIN inserted AS i
             ON d.Id = i.Id
END
--3
CREATE PROC usp_DepositMoney (@AccountId INT, @MoneyAmount DECIMAL(15, 4))
AS
BEGIN
	UPDATE Accounts
	SET Balance += @MoneyAmount
	WHERE Id = @AccountId
END
--4
CREATE PROC usp_WithdrawMoney @AccountId INT,@MoneyAmount DECIMAL(15,4)
AS
BEGIN
	UPDATE Accounts
	SET Balance-=@MoneyAmount
	WHERE Id = @AccountId
END
--5
CREATE PROC usp_TransferMoney @SenderId INT,@ReceiverId INT,@Amount DECIMAL(15,4)
AS
BEGIN
	BEGIN TRAN
		UPDATE Accounts
		SET Balance-=@Amount
		WHERE Id = @SenderId

		IF(@@ROWCOUNT <> 1)
		BEGIN;
			ROLLBACK;
			THROW 50002,'Cannot make transaction',1
			RETURN;
		END
		IF((SELECT Balance FROM Accounts WHERE Id = @SenderId) < 0)
		BEGIN;
			ROLLBACK;
			THROW 50003,'Negative amount',1
		END

		UPDATE Accounts
		SET Balance+=@Amount
		WHERE Id=@ReceiverId
	COMMIT
END
--7
DECLARE @IdUserGame INT = (SELECT Id FROM UsersGames
WHERE UserId = (SELECT Id FROM Users WHERE Username ='Stamat')
AND GameId = (SELECT Id FROM Games WHERE Name = 'Safflower'))

DECLARE @CurrentCash MONEY = (SELECT Cash FROM UsersGames WHERE Id = @IdUserGame)
DECLARE @PlayerLevel INT = (SELECT [Level] FROM UsersGames WHERE Id = @IdUserGame)
DECLARE @TotalCost MONEY
DECLARE @ItemMinLevel INT
DECLARE @ItemMaxLevel INT

SET @ItemMinLevel = 11
SET @ItemMaxLevel = 12
SET @TotalCost = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN @ItemMinLevel AND @ItemMaxLevel)

IF(@TotalCost <= @CurrentCash AND @PlayerLevel >= @ItemMaxLevel)
BEGIN
	BEGIN TRANSACTION
		INSERT INTO UserGameItems (ItemId, UserGameId)
		(SELECT Id, @IdUserGame FROM Items WHERE MinLevel BETWEEN @ItemMinLevel AND @ItemMaxLevel)

		UPDATE UsersGames
		SET Cash -= @TotalCost WHERE UsersGames.Id = @IdUserGame
	COMMIT
END

SET @ItemMinLevel = 19
SET @ItemMaxLevel = 21
SET @TotalCost = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN @ItemMinLevel AND @ItemMaxLevel)

IF(@TotalCost <= @CurrentCash AND @PlayerLevel >= @ItemMaxLevel)
BEGIN
	BEGIN TRANSACTION
		INSERT INTO UserGameItems (ItemId, UserGameId)
		(SELECT Id, @IdUserGame FROM Items WHERE MinLevel BETWEEN @ItemMinLevel AND @ItemMaxLevel)

		UPDATE UsersGames
		SET Cash -= @TotalCost WHERE UsersGames.Id = @IdUserGame
	COMMIT
END

SELECT i.Name AS [Item Name] FROM UserGameItems AS ugi
  JOIN	Items AS i ON i.Id = ugi.ItemId
 WHERE UserGameId = @IdUserGame
 ORDER BY [Item Name]
--8
create proc usp_AssignProject(@emloyeeId INT, @projectID INT)
AS
BEGIN
	BEGIN TRAN
	INSERT INTO EmployeesProjects VALUES
	(@emloyeeId,@projectID)
	DECLARE @CurrentProjects INT;
	SET @CurrentProjects = (SELECT COUNT(ProjectID) FROM EmployeesProjects WHERE EmployeeID = @emloyeeId)
	IF(@CurrentProjects > 3)
	BEGIN
		RAISERROR('The employee has too many projects!', 16, 1)
		ROLLBACK
		RETURN
	END
	COMMIT
END
--9
CREATE TRIGGER tr_DeleteEmployee ON Employees AFTER DELETE
AS
	
	INSERT INTO Deleted_Employees
	SELECT FirstName, 
		   LastName, 
		   MiddleName, 
		   JobTitle, 
		   DepartmentID, 
		   Salary
	  FROM deleted
