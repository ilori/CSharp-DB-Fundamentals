--1
SELECT SUBSTRING(Email,CHARINDEX('@',Email,0) + 1,LEN(Email) - CHARINDEX('@',Email,0)) AS [Email Provider],COUNT(SUBSTRING(Email,CHARINDEX('@',Email,0) + 1,LEN(Email) - CHARINDEX('@',Email,0))) AS [Number Of Users] FROM Users
GROUP BY SUBSTRING(Email,CHARINDEX('@',Email,0) + 1,LEN(Email) - CHARINDEX('@',Email,0))
ORDER BY [Number Of Users] DESC,[Email Provider]
--2
SELECT g.Name,gt.Name,u.Username,ug.Level,ug.Cash,c.Name FROM Users AS u
JOIN UsersGames AS ug ON ug.UserId = u.Id
JOIN Games AS g ON g.Id = ug.GameId
JOIN GameTypes AS gt ON gt.Id = g.GameTypeId
JOIN Characters AS c ON c.Id = ug.CharacterId
ORDER BY ug.Level DESC,u.Username,g.Name
--3
SELECT u.Username [Username], 
	   g.Name [Game], 
	   COUNT(i.Id) [Items Count], 
	   SUM(i.Price) [Items Price]
  FROM UsersGames ug
  JOIN Users u ON u.Id = ug.UserId
  JOIN Games g ON g.Id = ug.GameId
  JOIN UserGameItems ugi ON ugi.UserGameId = ug.Id
  JOIN Items i ON i.Id = ugi.ItemId
 GROUP BY u.Username, g.Name
HAVING COUNT(i.Id) >= 10
 ORDER BY [Items Count] DESC, [Items Price] DESC, Username
--4
SELECT 
  u.Username, g.Name AS Game, MAX(c.Name) AS Character, 
  MAX(s.Strength) + MAX(st.Strength) + SUM(sta.Strength) AS Strength, 
  MAX(s.Defence) + MAX(st.Defence) + SUM(sta.Defence) AS Defence, 
  MAX(s.Speed) + MAX(st.Speed) + SUM(sta.Speed) AS Speed, 
  MAX(s.Mind) + MAX(st.Mind) + SUM(sta.Mind) AS Mind, 
  MAX(s.Luck) + MAX(st.Luck) + SUM(sta.Luck) AS Luck
FROM UsersGames AS ug
JOIN Users AS u ON ug.UserId = u.Id
JOIN Games AS g ON ug.GameId = g.Id
JOIN Characters AS c ON ug.CharacterId = c.Id
JOIN [Statistics] AS s ON c.StatisticId = s.Id
JOIN GameTypes AS gt ON gt.Id = g.GameTypeId
JOIN [Statistics] AS st ON st.Id = gt.BonusStatsId
JOIN UserGameItems AS ugi ON ugi.UserGameId = ug.Id
JOIN Items AS i ON i.Id = ugi.ItemId
JOIN [Statistics] AS sta ON sta.Id = i.StatisticId
GROUP BY u.Username, g.Name
ORDER BY Strength DESC, Defence DESC, Speed DESC, Mind DESC, Luck DESC
--5
DECLARE @AvgSpeed INT = (SELECT AVG(s.Speed) FROM Items AS i JOIN [Statistics] AS s ON s.Id = i.StatisticId)
DECLARE @AvgMind INT = (SELECT AVG(s.Mind) FROM Items AS i JOIN [Statistics] AS s ON s.Id = i.StatisticId)
DECLARE @AvgLuck INT = (SELECT AVG(s.Luck) FROM Items AS i JOIN [Statistics] AS s ON s.Id = i.StatisticId)

SELECT i.Name,i.Price,i.MinLevel,s.Strength,s.Defence,s.Speed,s.Luck,s.Mind FROM Items AS i
JOIN [Statistics] AS s ON s.Id = i.StatisticId
WHERE s.Speed > @AvgSpeed AND s.Mind > @AvgMind AND s.Luck > @AvgLuck
ORDER BY i.Name
--6
SELECT i.Name AS Item,i.Price,i.MinLevel,gt.Name AS [Forbidden Game Type]  FROM Items AS i
LEFT JOIN GameTypeForbiddenItems AS gti ON gti.ItemId = i.Id
LEFT JOIN GameTypes AS gt ON gt.Id = gti.GameTypeId
ORDER BY [Forbidden Game Type] DESC,Item
--7
DECLARE @UserId INT = (SELECT Id FROM Users WHERE Username = 'Alex')
DECLARE @UserGameId INT = (SELECT Id FROM UsersGames 
							WHERE UserId = @UserId AND  GameId = (SELECT Id FROM Games 
																	WHERE Name = 'Edinburgh'))	--235
DECLARE @BlackguardItemId INT = (SELECT Id FROM Items WHERE [Name] = 'Blackguard')	--51
DECLARE @PotionItemId INT = (SELECT Id FROM Items WHERE [Name] = 'Bottomless Potion of Amplification')
DECLARE @EyeItemId INT = (SELECT Id FROM Items WHERE [Name] = 'Eye of Etlich (Diablo III)')
DECLARE @GemItemId INT = (SELECT Id FROM Items WHERE [Name] = 'Gem of Efficacious Toxin')
DECLARE @GorgetItemId INT = (SELECT Id FROM Items WHERE [Name] = 'Golden Gorget of Leoric')
DECLARE @AmuletItemId INT = (SELECT Id FROM Items WHERE [Name] = 'Hellfire Amulet')
DECLARE @CurrentCash MONEY
DECLARE @CurrentItemPrice MONEY

BEGIN TRANSACTION
	SET @CurrentCash = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)
	SET @CurrentItemPrice = (SELECT Price FROM Items WHERE Id = @BlackguardItemId)
	INSERT INTO	UserGameItems (ItemId, UserGameId)
	VALUES (@BlackguardItemId, @UserGameId)
	UPDATE UsersGames
	SET Cash -= @CurrentItemPrice
	WHERE UsersGames.Id = @UserGameId
	IF (@CurrentCash - @CurrentItemPrice < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Not enough money to buy this item', 16, 1)
		RETURN
	END
COMMIT

BEGIN TRANSACTION
	SET @CurrentCash = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)
	SET @CurrentItemPrice = (SELECT Price FROM Items WHERE Id = @PotionItemId)
	INSERT INTO	UserGameItems (ItemId, UserGameId)
	VALUES (@PotionItemId, @UserGameId)
	UPDATE UsersGames
	SET Cash -= @CurrentItemPrice
	WHERE UsersGames.Id = @UserGameId
	IF (@CurrentCash - @CurrentItemPrice < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Not enough money to buy this item', 16, 1)
		RETURN
	END
COMMIT

BEGIN TRANSACTION
	SET @CurrentCash = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)
	SET @CurrentItemPrice = (SELECT Price FROM Items WHERE Id = @EyeItemId)
	INSERT INTO	UserGameItems (ItemId, UserGameId)
	VALUES (@EyeItemId, @UserGameId)
	UPDATE UsersGames
	SET Cash -= @CurrentItemPrice
	WHERE UsersGames.Id = @UserGameId
	IF (@CurrentCash - @CurrentItemPrice < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Not enough money to buy this item', 16, 1)
		RETURN
	END
COMMIT

BEGIN TRANSACTION
	SET @CurrentCash = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)
	SET @CurrentItemPrice = (SELECT Price FROM Items WHERE Id = @GemItemId)
	INSERT INTO	UserGameItems (ItemId, UserGameId)
	VALUES (@GemItemId, @UserGameId)
	UPDATE UsersGames
	SET Cash -= @CurrentItemPrice
	WHERE UsersGames.Id = @UserGameId
	IF (@CurrentCash - @CurrentItemPrice < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Not enough money to buy this item', 16, 1)
		RETURN
	END
COMMIT

BEGIN TRANSACTION
	SET @CurrentCash = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)
	SET @CurrentItemPrice = (SELECT Price FROM Items WHERE Id = @GorgetItemId)
	INSERT INTO	UserGameItems (ItemId, UserGameId)
	VALUES (@GorgetItemId, @UserGameId)
	UPDATE UsersGames
	SET Cash -= @CurrentItemPrice
	WHERE UsersGames.Id = @UserGameId
	IF (@CurrentCash - @CurrentItemPrice < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Not enough money to buy this item', 16, 1)
		RETURN
	END
COMMIT

BEGIN TRANSACTION
	SET @CurrentCash = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)
	SET @CurrentItemPrice = (SELECT Price FROM Items WHERE Id = @AmuletItemId)
	INSERT INTO	UserGameItems (ItemId, UserGameId)
	VALUES (@AmuletItemId, @UserGameId)
	UPDATE UsersGames
	SET Cash -= @CurrentItemPrice
	WHERE UsersGames.Id = @UserGameId
	IF (@CurrentCash - @CurrentItemPrice < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Not enough money to buy this item', 16, 1)
		RETURN
	END
COMMIT

SELECT u.Username, g.Name, ug.Cash, i.Name [Item Name]
  FROM Users u
  LEFT JOIN UsersGames ug ON ug.UserId = u.Id
  LEFT JOIN Games g ON g.Id = ug.GameId
  LEFT JOIN UserGameItems ugi ON ugi.UserGameId = ug.Id
  LEFT JOIN Items i ON i.Id = ugi.ItemId
 WHERE g.Name = 'Edinburgh'
 ORDER BY [Item Name]

--8
SELECT p.PeakName,m.MountainRange,p.Elevation FROM Mountains AS m
JOIN Peaks AS p ON p.MountainId = m.Id
ORDER BY p.Elevation DESC,p.PeakName
--9
SELECT p.PeakName,m.MountainRange,cr.CountryName,c.ContinentName FROM Continents AS c
JOIN Countries AS cr ON cr.ContinentCode = c.ContinentCode
JOIN MountainsCountries AS mc ON mc.CountryCode = cr.CountryCode
JOIN Mountains AS m ON m.Id = mc.MountainId
JOIN Peaks AS p ON p.MountainId = m.Id
ORDER BY p.PeakName,cr.CountryName
--10
SELECT cr.CountryName,c.ContinentName,ISNULL(COUNT(r.Id),0) AS [RiverCount],IIF(COUNT(r.Id) = 0,0,SUM(r.Length)) AS [TotalLenght] FROM Continents AS c
LEFT JOIN Countries AS cr ON cr.ContinentCode = c.ContinentCode
LEFT JOIN CountriesRivers AS crr ON cr.CountryCode = crr.CountryCode
LEFT JOIN Rivers AS r ON r.Id = crr.RiverId
GROUP BY cr.CountryName,c.ContinentName
ORDER BY RiverCount DESC,TotalLenght DESC,cr.CountryName

--11
SELECT c.CurrencyCode,c.Description,ISNULL(COUNT(cr.CurrencyCode),0) AS [NumberOfContries] FROM Currencies AS c
LEFT JOIN Countries AS cr ON cr.CurrencyCode = c.CurrencyCode
GROUP BY c.CurrencyCode,c.Description
ORDER BY NumberOfContries DESC,c.Description
--12
SELECT c.ContinentName AS ContinentName,SUM(cr.AreaInSqKm ) [CountriesArea],SUM(CAST(cr.Population AS bigint)) [CountriesPopulation] FROM Continents AS c
JOIN Countries AS cr ON cr.ContinentCode = c.ContinentCode
GROUP BY c.ContinentName
ORDER BY CountriesPopulation DESC
--13
CREATE TABLE Monasteries(
	Id INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL,
	CountryCode CHAR(2) FOREIGN KEY REFERENCES Countries(CountryCode)
)

INSERT INTO Monasteries(Name, CountryCode) VALUES
('Rila Monastery “St. Ivan of Rila”', 'BG'), 
('Bachkovo Monastery “Virgin Mary”', 'BG'),
('Troyan Monastery “Holy Mother''s Assumption”', 'BG'),
('Kopan Monastery', 'NP'),
('Thrangu Tashi Yangtse Monastery', 'NP'),
('Shechen Tennyi Dargyeling Monastery', 'NP'),
('Benchen Monastery', 'NP'),
('Southern Shaolin Monastery', 'CN'),
('Dabei Monastery', 'CN'),
('Wa Sau Toi', 'CN'),
('Lhunshigyia Monastery', 'CN'),
('Rakya Monastery', 'CN'),
('Monasteries of Meteora', 'GR'),
('The Holy Monastery of Stavronikita', 'GR'),
('Taung Kalat Monastery', 'MM'),
('Pa-Auk Forest Monastery', 'MM'),
('Taktsang Palphug Monastery', 'BT'),
('Sümela Monastery', 'TR')

UPDATE Countries
SET IsDeleted = 1
WHERE CountryName IN (SELECT p.CountryName FROM
(SELECT c.CountryName,COUNT(r.Id) AS Rivers FROM Countries AS c JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode JOIN Rivers AS r ON r.Id = cr.RiverId
GROUP BY c.CountryName
HAVING COUNT(r.id) > 3
) AS p)

SELECT m.Name,c.CountryName FROM Countries AS c
RIGHT JOIN Monasteries AS m ON m.CountryCode = c.CountryCode
WHERE IsDeleted <> 1
ORDER BY m.Name
--14
UPDATE Countries
   SET CountryName = 'Burma'
 WHERE CountryName = 'Myanmar'


INSERT INTO Monasteries (Name, CountryCode) VALUES
('Hanga Abbey', (SELECT CountryCode 
				 FROM Countries 
				  WHERE CountryName = 'Tanzania'))

INSERT INTO Monasteries (Name, CountryCode) VALUES
('Myin-Tin-Daik', (SELECT CountryCode 
				 FROM Countries 
				  WHERE CountryName = 'Myanmar'))

SELECT cont.ContinentName, c.CountryName, COUNT(m.Id) [MonasteriesCount]
  FROM Countries c
  JOIN Continents cont ON cont.ContinentCode = c.ContinentCode
  LEFT JOIN Monasteries m ON m.CountryCode = c.CountryCode
 WHERE c.IsDeleted = 0
 GROUP BY cont.ContinentName, c.CountryName
 ORDER BY MonasteriesCount DESC, c.CountryName

