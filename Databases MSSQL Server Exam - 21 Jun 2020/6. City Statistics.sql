SELECT c.Name, COUNT(CityId) AS Hotels
FROM Hotels AS h
JOIN Cities AS c ON h.CityId = c.Id
GROUP BY c.Name
ORDER BY Hotels DESC, c.Name

