SELECT TOP(5) rc.RepositoryId, r.Name, COUNT(*) AS [count]
	FROM RepositoriesContributors AS rc
	JOIN Repositories AS r ON r.Id = rc.RepositoryId
	JOIN Commits AS c ON c.RepositoryId = r.Id
	GROUP BY rc.RepositoryId, r.Name
	ORDER BY count DESC, rc.RepositoryId ASC , r.Name ASC