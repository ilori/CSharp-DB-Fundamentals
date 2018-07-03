--1
CREATE TABLE Countries
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Distributors
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(25) NOT NULL UNIQUE,
	AddressText NVARCHAR(30),
	Summary NVARCHAR(200),
	CountryId INT NOT NULL,
	CONSTRAINT FK_Distributors_Countries FOREIGN KEY(CountryId)
	REFERENCES Countries(Id)
)

CREATE TABLE Customers
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(25),
	LastName NVARCHAR(25),
	Gender CHAR(1) CHECK (Gender = 'M' OR Gender = 'F'),
	Age INT,
	PhoneNumber CHAR(10),
	CountryId INT,
	CONSTRAINT FK_Customers_Countries FOREIGN KEY(CountryId)
	REFERENCES Countries(Id)
)

CREATE TABLE Products
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(25) UNIQUE,
	Description NVARCHAR(250),
	Recipe NVARCHAR(MAX),
	Price MONEY CHECK (Price > 0)
)

CREATE TABLE Feedbacks
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Description NVARCHAR(255),
	Rate DECIMAL(10,2),
	ProductId INT,
	CustomerId INT,
	CONSTRAINT FK_Feedbacks_Products FOREIGN KEY(ProductId)
	REFERENCES Products(Id),
	CONSTRAINT FK_Feedbacks_Customers FOREIGN KEY(CustomerId)
	REFERENCES Customers(Id)
)

CREATE TABLE Ingredients
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(30),
	Description NVARCHAR(200),
	OriginCountryId INT,
	DistributorId INT,
	CONSTRAINT FK_Ingredients_Countries FOREIGN KEY(OriginCountryId)
	REFERENCES Countries(Id),
	CONSTRAINT FK_Ingredients_Distributors FOREIGN KEY(DistributorId)
	REFERENCES Distributors(Id)
)

CREATE TABLE ProductsIngredients
(
	ProductId INT NOT NULL,
	IngredientId INT NOT NULL,
	CONSTRAINT PK_ProductsIngredients PRIMARY KEY(ProductId, IngredientId),
	CONSTRAINT FK_ProductsIngredients_Products FOREIGN KEY(ProductId)
	REFERENCES Products(Id),
	CONSTRAINT FK_ProductsIngredients_Ingredients FOREIGN KEY(IngredientId)
	REFERENCES Ingredients(Id)
)

--2
INSERT INTO Distributors (Name,CountryId,AddressText,Summary) VALUES
('Deloitte & Touche',2,'6 Arch St #9757','Customizable neutral traveling'),
('Congress Title',13,'58 Hancock St','Customer loyalty'),
('Kitchen People',1,'3 E 31st St #77','Triple-buffered stable delivery'),
('General Color Co Inc',21,'6185 Bohn St #72','Focus group'),
('Beck Corporation',23,'21 E 64th Ave','Quality-focused 4th generation hardware')


INSERT INTO Customers (FirstName,LastName,Age,Gender,PhoneNumber,CountryId) VALUES
('Francoise','Rautenstrauch',15,'M','0195698399',5),
('Kendra','Loud',22,'F','0063631526',11),
('Lourdes','Bauswell',50,'M','0139037043',8),
('Hannah','Edmison',18,'F','0043343686',1),
('Tom','Loeza',31,'M','0144876096',23),
('Queenie','Kramarczyk',30,'F','0064215793',29),
('Hiu','Portaro',25,'M','0068277755',16),
('Josefa','Opitz',43,'F','0197887645',17)

--3
UPDATE Ingredients
SET DistributorId = 35
WHERE Name IN ('Paprika','Bay Leaf','Poppy')

UPDATE Ingredients
SET OriginCountryId = 14
WHERE OriginCountryId = 8
--4
DELETE FROM Feedbacks
WHERE CustomerId = 14 OR ProductId = 5
--5
SELECT Name,Price,Description FROM Products
ORDER BY Price DESC,Name
--6
SELECT Name,Description,OriginCountryId FROM Ingredients
WHERE OriginCountryId IN (1,10,20)
ORDER BY Id
--7
SELECT TOP 15 i.Name,i.Description,c.Name AS [CountryName] FROM Ingredients AS i
LEFT JOIN Countries AS c ON c.Id = i.OriginCountryId
WHERE OriginCountryId IN (SELECT Id FROM Countries WHERE Name IN ('Greece','Bulgaria'))
ORDER BY i.Name,CountryName
--8
SELECT TOP 10 p.Name,p.Description,AVG(f.Rate) AS AverageRate,COUNT(f.Id) AS FeedbacksAmount FROM Feedbacks AS f
JOIN Products AS p ON p.Id = f.ProductId
GROUP BY p.Name,p.Description
ORDER BY AverageRate DESC,FeedbacksAmount DESC
--9
SELECT f.ProductId AS ProductId,CAST(AVG(f.Rate) AS decimal(4,2)) AS Rate,f.Description,c.Id,c.Age,c.Gender FROM Feedbacks AS f
JOIN Customers AS c ON c.Id = f.CustomerId
GROUP BY f.ProductId,f.Description,c.Id,c.Age,c.Gender
HAVING AVG(f.Rate) < 5
ORDER BY f.ProductId DESC,Rate
--10
SELECT CONCAT(c.FirstName,' ',c.LastName) AS CustomerName,c.PhoneNumber,c.Gender FROM Feedbacks AS f
RIGHT JOIN Customers AS c ON c.id = f.CustomerId
WHERE f.CustomerId IS NULL
ORDER BY c.Id
--11
SELECT f.ProductId,CONCAT(c.FirstName,' ',c.LastName) AS CustomerName,f.Description AS FeedbackDescription FROM Customers AS c
JOIN Feedbacks AS f ON f.CustomerId=c.Id
WHERE c.Id IN(SELECT c.Id FROM Customers AS c JOIN Feedbacks AS f ON f.CustomerId=c.Id GROUP BY c.Id HAVING COUNT(f.Id)>=3)
ORDER BY ProductId,CustomerName,f.Id
--12
SELECT cu.FirstName,cu.Age,cu.PhoneNumber FROM Customers AS cu
FULL JOIN Countries AS co ON co.Id = cu.CountryId
WHERE (Age >= 21 AND FirstName LIKE '%an%') OR (SUBSTRING(PhoneNumber,LEN(PhoneNumber) -1,2) = '38' AND co.Name IN (SELECT Name FROM Customers AS cu
FULL JOIN Countries AS co ON co.Id = cu.CountryId
WHERE co.Name <> 'Greece'))
ORDER BY cu.FirstName,cu.Age DESC

--13
SELECT d.Name,i.Name,p.Name,AVG(f.Rate) FROM Ingredients AS i
JOIN Distributors AS d ON d.Id = i.DistributorId
JOIN ProductsIngredients AS pii ON pii.IngredientId = i.Id
JOIN Products AS p ON p.Id = pii.ProductId
JOIN Feedbacks AS f ON f.ProductId = p.Id
GROUP BY d.Name,i.Name,p.Name
HAVING AVG(f.Rate) BETWEEN 5 AND 8
ORDER BY d.Name,i.Name,p.Name
--14
SELECT TOP 1 WITH TIES co.Name,AVG(f.Rate) AS FeedbackRate FROM Customers AS c
JOIN Feedbacks AS f ON f.CustomerId = c.Id
JOIN Countries AS co ON co.Id = c.CountryId
GROUP BY co.Name
ORDER BY FeedbackRate DESC
--15
SELECT res.CountryName AS [CountryName],res.DistributorName AS [DisributorName] FROM (
	SELECT	d.Name AS [DistributorName],c.Name AS [CountryName],DENSE_RANK() OVER (PARTITION BY c.Name ORDER BY COUNT(i.Id) DESC) AS [curRank] FROM Countries AS c
	LEFT JOIN Distributors AS d ON c.Id = d.CountryId 
	LEFT JOIN Ingredients AS i ON d.Id = i.DistributorId
	GROUP BY d.Name, c.Name
) AS res
WHERE res.curRank = 1 AND res.DistributorName IS NOT NULL
--16
CREATE VIEW v_UserWithCountries 
AS
SELECT FirstName + ' ' + LastName AS CustomerName,Age,Gender,co.Name FROM Customers AS c
JOIN Countries AS co ON co.Id = c.CountryId 

--17
CREATE FUNCTION udf_GetRating(@Name NVARCHAR(25))
RETURNS NVARCHAR(10)
AS
BEGIN
	
	DECLARE @AvarageRating DECIMAL(15,1);

	SET @AvarageRating=(SELECT AVG(f.Rate) FROM Products AS p
	LEFT JOIN Feedbacks AS f ON f.ProductId = p.Id
	WHERE Name = @Name
	GROUP BY p.Name)

	DECLARE @Result NVARCHAR(10);

	IF(@AvarageRating < 5)
	BEGIN
		SET @Result = 'Bad'
	END
	ELSE IF (@AvarageRating BETWEEN 5 AND 8)
	BEGIN
		SET @Result = 'Average'
	END
	ELSE IF (@AvarageRating > 8)
	BEGIN
		SET @Result = 'Good'
	END
	ELSE
	BEGIN
		SET @Result = 'No rating'
	END

	RETURN @Result
END
--18
CREATE PROC usp_SendFeedback @CustomerId INT,@ProductId INT,@Rate DECIMAL(10,2),@Description NVARCHAR(255)
AS
BEGIN
	BEGIN TRAN
	
		INSERT INTO Feedbacks (CustomerId,ProductId,Rate,Description) VALUES
		(@CustomerId,@ProductId,@Rate,@Description)

		DECLARE @TotalFeedbacks INT;

		SET @TotalFeedbacks = (SELECT COUNT(f.CustomerId) FROM Feedbacks AS f
		WHERE f.CustomerId = @CustomerId AND f.ProductId = @ProductId
		GROUP BY f.ProductId)	

		IF(@TotalFeedbacks >= 3)
		BEGIN
			RAISERROR('You are limited to only 3 feedbacks per product!',16,1)
			ROLLBACK;
			RETURN
		END
	COMMIT
END

--19
CREATE TRIGGER tr_Delete ON Products INSTEAD OF DELETE
AS
BEGIN
		DELETE FROM Feedbacks
		WHERE ProductId = (SELECT Id FROM deleted)

		DELETE FROM ProductsIngredients
		WHERE ProductId = (SELECT Id FROM deleted)

		DELETE FROM Products
		WHERE Id = (SELECT Id FROM deleted)
END
--20
SELECT ProductName, 
	   ProductAverageRate, 
	   DistributorName, 
	   DistributorCountry
  FROM (SELECT p.Name [ProductName], 
			   AVG(f.Rate) [ProductAverageRate], 
			   d.Name [DistributorName], 
			   c.Name [DistributorCountry],
			   p.Id [ProductId]
		  FROM ( SELECT p.Id
						  FROM Products p
						  JOIN ProductsIngredients prin ON prin.ProductId = p.Id
						  JOIN Ingredients i ON i.Id = prin.IngredientId
						  JOIN Distributors d ON d.Id = i.DistributorId
						 GROUP BY p.Id
						HAVING COUNT(DISTINCT(i.DistributorId)) = 1) AS ProdSingleDistr
		  JOIN Products p ON p.Id = ProdSingleDistr.Id
		  JOIN ProductsIngredients prin ON prin.ProductId = p.Id
		  JOIN Ingredients i ON i.Id = prin.IngredientId
		  JOIN Distributors d ON d.Id = i.DistributorId
		  JOIN Feedbacks f ON f.ProductId = p.Id
		  JOIN Countries c ON c.Id = d.CountryId
		 GROUP BY p.Name, d.Name, c.Name, p.Id
		)   AS result
 ORDER BY result.ProductId
