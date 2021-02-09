SELECT AccountId, FirstName + ' ' + LastName AS FullName, 
	MAX(DATEDIFF(DAY, ArrivalDate, ReturnDate)) AS LONGESTDAY,
	MIN(DATEDIFF(DAY, ArrivalDate, ReturnDate)) AS SHORTERSTDAY
	FROM AccountsTrips AS at
	JOIN Accounts AS a ON a.Id = at.AccountId
	JOIN Trips AS t ON t.Id = at.TripId
	WHERE MiddleName IS NULL AND t.CancelDate IS NULL	
	GROUP BY AccountId,FirstName,LastName
	ORDER BY LONGESTDAY DESC, SHORTERSTDAY ASC

