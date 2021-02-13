CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(MAX))
RETURNS INT
BEGIN
	DECLARE @countOfComits INT = (SELECT Count(*) FROM Users AS u 
										 JOIN Commits AS c ON c.ContributorId = u.Id
										 WHERE u.Username = @username
										GROUP BY u.Username);
										IF @countOfComits IS NULL RETURN 0;
	RETURN @countOfComits;
END