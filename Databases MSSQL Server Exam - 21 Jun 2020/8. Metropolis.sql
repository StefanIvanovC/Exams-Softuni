
SELECT TOP(10) c.Id, c.Name, c.CountryCode, COUNT(*) AS accounts
	FROM Accounts AS a
	JOIN Cities AS c ON a.CityId = c.Id
	GROUP BY c.Id, c.Name,c.CountryCode 
	ORDER BY accounts DESC
