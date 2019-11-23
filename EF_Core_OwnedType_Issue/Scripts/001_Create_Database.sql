IF (EXISTS (SELECT name FROM master.dbo.sysdatabases where name = 'Candidates'))
BEGIN

ALTER DATABASE [Candidates] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [Candidates]

END

CREATE DATABASE [Candidates]