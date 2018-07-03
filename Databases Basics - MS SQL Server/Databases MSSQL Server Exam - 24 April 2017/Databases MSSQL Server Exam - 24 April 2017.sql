--1
CREATE TABLE Mechanics(
	MechanicId INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Address NVARCHAR(255) NOT NULL
)

CREATE TABLE Clients(
	ClientId INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Phone VARCHAR(12) 

	CONSTRAINT CK_Clients_Phone CHECK(LEN(Phone) = 12)
)

CREATE TABLE Vendors(
	VendorId INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) UNIQUE NOT NULL
)
CREATE TABLE Parts(
	PartId INT PRIMARY KEY IDENTITY,
	SerialNumber NVARCHAR(50) UNIQUE NOT NULL,
	Description NVARCHAR(255),
	Price DECIMAL(6,2),
	VendorId INT FOREIGN KEY REFERENCES Vendors(VendorId),
	StockQty INT DEFAULT 0,

	CONSTRAINT CK_Parts_Price CHECK(Price > 0 AND Price < 9999.99),
	CONSTRAINT CK_Parts_StockQty CHECK(StockQty >= 0)
)

CREATE TABLE Models(
	ModelId INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Jobs(
	JobId INT PRIMARY KEY IDENTITY,
	ModelId INT FOREIGN KEY REFERENCES Models(ModelId),
	Status NVARCHAR(11) DEFAULT 'Pending' NOT NULL,
	ClientId INT FOREIGN KEY REFERENCES Clients(ClientId),
	MechanicId INT FOREIGN KEY REFERENCES Mechanics(MechanicId),
	IssueDate DATE NOT NULL,
	FinishDate DATE,

	CONSTRAINT CK_Jobs_Status CHECK(Status = 'Pending' OR Status = 'In Progress' OR Status = 'Finished')
)

CREATE TABLE Orders(
	OrderId INT PRIMARY KEY IDENTITY,
	JobId INT FOREIGN KEY REFERENCES Jobs(JobId),
	IssueDate DATE,
	Delivered BIT DEFAULT 0
)

CREATE TABLE PartsNeeded(
	JobId INT FOREIGN KEY REFERENCES Jobs(JobId),
	PartId INT FOREIGN KEY REFERENCES Parts(PartId),
	Quantity INT DEFAULT 1,

	CONSTRAINT CK_PartsNeeded_Quantity CHECK(Quantity > 0),
	CONSTRAINT PK_PartsNeeded PRIMARY KEY (JobId,PartId)
)
CREATE TABLE OrderParts(
	OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
	PartId INT FOREIGN KEY REFERENCES Parts(PartId),
	Quantity INT DEFAULT 1,

	CONSTRAINT CK_OrderParts_Quantity CHECK(Quantity > 0),
	CONSTRAINT PK_OrderParts PRIMARY KEY (OrderId,PartId)
)
--2
INSERT INTO Clients (FirstName,LastName,Phone) VALUES
('Teri','Ennaco','570-889-5187'),
('Merlyn','Lawler','201-588-7810'),
('Georgene','Montezuma','925-615-5185'),
('Jettie','Mconnell','908-802-3564'),
('Lemuel','Latzke','631-748-6479'),
('Melodie','Knipp','805-690-1682'),
('Candida','Corbley','908-275-8357')

INSERT INTO Parts(SerialNumber,Description,Price,VendorId) VALUES
('WP8182119','Door Boot Seal',117.86,2),
('W10780048','Suspension Rod',42.81,1),
('W10841140','Silicone Adhesive',6.77,4),
('WPY055980','High Temperature Adhesive',13.94,3)
--3
UPDATE Jobs
SET Status = 'In Progress',MechanicId = 3
WHERE Status = 'Pending'
--4
DELETE FROM OrderParts
WHERE OrderId = 19
DELETE FROM Orders
WHERE OrderId = 19
--5
SELECT FirstName,LastName,Phone FROM Clients
ORDER BY LastName,ClientId
--6
SELECT Status,IssueDate FROM Jobs
WHERE Status <> 'Finished'
ORDER BY IssueDate,Status
--7
SELECT m.FirstName + ' ' + m.LastName AS Mechanic,j.Status,j.IssueDate FROM Mechanics AS m
JOIN Jobs AS j ON j.MechanicId = m.MechanicId
ORDER BY m.MechanicId,j.IssueDate,j.JobId
--8
SELECT c.FirstName + ' ' + c.LastName AS Client,
	   DATEDIFF(DAY,j.IssueDate,'04-24-2017') AS [Days Going],
	   j.Status 
  FROM Clients AS c
  JOIN Jobs AS j ON j.ClientId = c.ClientId
 WHERE j.Status <> 'Finished'
 ORDER BY [Days Going] DESC,c.ClientId
--9
SELECT Mechanic,AVG(TotalDays) AS [Average Days]
FROM
(SELECT m.MechanicId,m.FirstName + ' ' + m.LastName AS Mechanic,DATEDIFF(DAY,j.IssueDate,j.FinishDate) AS TotalDays FROM Mechanics AS m
JOIN Jobs AS j ON j.MechanicId = m.MechanicId
WHERE Status = 'Finished') AS TotalDays
GROUP BY Mechanic,TotalDays.MechanicId
ORDER BY TotalDays.MechanicId
--10
SELECT TOP 3 m.FirstName + ' ' + m.LastName AS Mechanic,COUNT(m.MechanicId) AS Jobs FROM Mechanics AS m
JOIN Jobs AS j ON j.MechanicId = m.MechanicId
WHERE j.Status <> 'Finished' 
GROUP BY m.MechanicId,m.FirstName,m.LastName
HAVING COUNT(m.MechanicId) > 1
ORDER BY Jobs DESC,m.MechanicId
--11
SELECT Available FROM
(SELECT DISTINCT m.MechanicId, FirstName + ' ' + LastName AS Available FROM Mechanics AS m
FULL JOIN Jobs AS j ON j.MechanicId = m.MechanicId
WHERE j.JobId IS NULL OR j.Status = 'Finished'
)AS Fully
ORDER BY Fully.MechanicId
--12
SELECT ISNULL(SUM(p.Price * op.Quantity),0.00) AS [Parts Total] FROM Orders AS o
FULL JOIN OrderParts AS op ON op.OrderId = o.OrderId
FULL JOIN Parts AS p ON p.PartId = op.PartId
WHERE DATEDIFF(DAY,o.IssueDate,'04-24-2017') <= 21
--13
SELECT j.JobId,ISNULL(SUM(op.Quantity * p.Price),0.00) AS Total FROM Jobs AS j
FULL JOIN Orders AS o ON o.JobId = j.JobId
FULL JOIN OrderParts AS op ON op.OrderId = o.OrderId
FULL JOIN Parts AS p ON p.PartId = op.PartId
WHERE j.Status = 'Finished'
GROUP BY j.JobId
ORDER BY Total DESC,j.JobId

--14
SELECT m.ModelId,m.Name,CAST(ISNULL(AVG(DATEDIFF(DAY,IssueDate,FinishDate)),0.00) AS VARCHAR(10)) + ' days' AS [Average Service Time] FROM Models AS m
JOIN Jobs AS j ON j.ModelId = m.ModelId
GROUP BY m.ModelId,m.Name
ORDER BY AVG(DATEDIFF(DAY,IssueDate,FinishDate))
--15
WITH CTE_Faultiest(ModelId,BreaksAmount)
AS
(
	SELECT Models.ModelId,COUNT(Jobs.JobId) FROM Models
	JOIN Jobs  ON Jobs.ModelId = Models.ModelId
	GROUP BY Models.ModelId
)

SELECT Models.Name AS Model,(SELECT MAX(BreaksAmount) FROM CTE_Faultiest) AS [Times Serviced],ISNULL(SUM(OrderParts.Quantity * Parts.Price),0.00) AS [Parts Total] FROM Orders 
JOIN OrderParts ON OrderParts.OrderId=Orders.OrderId
JOIN Parts ON OrderParts.PartId=Parts.PartId
RIGHT JOIN Jobs ON Orders.JobId=Jobs.JobId
JOIN Models ON Models.ModelId=Jobs.ModelId
WHERE Models.ModelId IN (
			SELECT Models.ModelId FROM Models
			JOIN Jobs ON Models.ModelId=Jobs.ModelId
			GROUP BY Models.ModelId
			HAVING COUNT(JobId)=(SELECT max(BreaksAmount)  
FROM CTE_Faultiest))
GROUP BY Models.Name
--16
SELECT p.PartId,
       p.Description,
       SUM(pn.Quantity) AS Required,
       AVG(p.StockQty) AS [In Stock],
       ISNULL(SUM(op.Quantity), 0) AS Ordered
  FROM Parts AS p
JOIN PartsNeeded pn ON pn.PartId = p.PartId
JOIN Jobs AS j ON j.JobId = pn.JobId
LEFT JOIN Orders AS o ON o.JobId = j.JobId
LEFT JOIN OrderParts AS op ON op.OrderId = o.OrderId
WHERE j.Status <> 'Finished'
GROUP BY p.PartId, p.Description
HAVING AVG(p.StockQty) + ISNULL(SUM(op.Quantity), 0) < SUM(pn.Quantity)
ORDER BY p.PartId
--17
CREATE FUNCTION udf_GetCost(@JobId INT)
RETURNS DECIMAL(15,2)
AS
BEGIN

	DECLARE @TotalMoney DECIMAL(15,2);
	SET @TotalMoney = (SELECT ISNULL(SUM(p.Price * op.Quantity) ,0.00)FROM Jobs AS j
	FULL JOIN Orders AS o ON o.JobId = j.JobId
	FULL JOIN OrderParts AS op ON op.OrderId = o.OrderId
	FULL JOIN Parts AS p ON p.PartId = op.PartId
	WHERE j.JobId = @JobId)
	 
RETURN @TotalMoney

END
--18
CREATE PROC usp_PlaceOrder @JobId INT, @SerialNumber VARCHAR(50), @Quantity INT
AS
BEGIN
    IF (@Quantity <= 0)
    BEGIN
        RAISERROR('Part quantity must be more than zero!', 16, 1);
        RETURN;
    END
    
    DECLARE @JobIdSelected INT = (SELECT JobId FROM Jobs WHERE JobId = @JobId)
    IF (@JobIdSelected IS NULL)
    BEGIN
        RAISERROR('Job not found!', 16, 1);
        RETURN;
    END

    DECLARE @JobStatus VARCHAR(11) = (SELECT Status FROM Jobs WHERE JobId = @JobId)

    IF (@JobStatus = 'Finished')
    BEGIN
        RAISERROR('This job is not active!', 16, 1);
        RETURN;
    END

    DECLARE @PartId INT = (SELECT PartId FROM Parts WHERE SerialNumber = @SerialNumber)
    IF (@PartId IS NULL)
    BEGIN
        RAISERROR('Part not found!', 16, 1);
        RETURN;
    END

    DECLARE @OrderId INT = (SELECT o.OrderId FROM Orders AS o
    JOIN OrderParts AS op ON op.OrderId = o.OrderId
    JOIN Parts AS p ON p.PartId = op.PartId
    WHERE JobId = @JobId AND p.PartId = @PartId AND IssueDate IS NULL)

    -- Order does not exist -> create new order
    IF (@OrderId IS NULL)
    BEGIN
        INSERT INTO Orders (JobId, IssueDate) VALUES
        (@JobId, NULL)

        INSERT INTO OrderParts (OrderId, PartId, Quantity) VALUES
        (IDENT_CURRENT('Orders'), @PartId, @Quantity)
    END
    ELSE
    BEGIN
        DECLARE @PartExistsInOrder INT = (SELECT @@ROWCOUNT FROM OrderParts
                                          WHERE OrderId = @OrderId AND PartId = @PartId)
        
        IF (@PartExistsInOrder IS NULL)
        BEGIN
            -- Order exists, part does not exist in it -> add part to order      
            INSERT INTO OrderParts (OrderId, PartId, Quantity) VALUES
            (@OrderId, @PartId, @Quantity)
        END
        ELSE
        BEGIN
            -- Order exists, part exists -> increase part quantity in order
            UPDATE OrderParts
            SET Quantity += @Quantity
            WHERE OrderId = @OrderId AND PartId = @PartId
        END
    END
END
--19
CREATE TRIGGER tr_Delivery ON Orders FOR UPDATE
AS
BEGIN
	DECLARE @OldStatus INT = (SELECT Delivered FROM deleted)
	DECLARE @NewStatus INT = (SELECT Delivered FROM inserted)

	IF(@OldStatus =0 AND @NewStatus = 1 )
	BEGIN

		UPDATE Parts 
		SET StockQty += op.Quantity
		FROM Parts AS p
		JOIN OrderParts AS op ON op.PartId = op.PartId
		JOIN Orders AS o ON o.OrderId = op.OrderId
		JOIN inserted AS i ON i.OrderId = o.OrderId
		JOIN deleted AS d ON d.OrderId = o.OrderId
		WHERE i.Delivered = 1 AND d.Delivered = 0 
	END
END
--20
WITH CTE_PartCount
AS
(
	SELECT m.MechanicId,v.VendorId,SUM(op.Quantity) AS ItemsVendors FROM Mechanics AS m
	JOIN Jobs AS j ON j.MechanicId = m.MechanicId
	JOIN Orders AS o ON o.JobId = j.JobId
	JOIN OrderParts AS op ON op.OrderId = o.OrderId
	JOIN Parts AS p ON p.PartId = op.PartId
	JOIN Vendors AS v ON v.VendorId = p.VendorId
	GROUP BY m.MechanicId,v.VendorId
)
SELECT m.FirstName + ' ' + m.LastName AS Mechanic,v.Name AS Vendor,c.ItemsVendors AS Parts,
CAST(CAST(CAST(ItemsVendors AS DECIMAL(6,2))/ (SELECT SUM(ItemsVendors) FROM CTE_PartCount WHERE MechanicId = c.MechanicId) * 100 AS INT) AS varchar(20)) + '%' AS Preference
FROM CTE_PartCount AS c
JOIN Mechanics AS m ON m.MechanicId = c.MechanicId
JOIN Vendors AS v ON v.VendorId = c.VendorId
ORDER BY Mechanic,Parts DESC,Vendor

