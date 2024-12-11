IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ClinicaslTrial')
BEGIN
    CREATE DATABASE ClinicaslTrial;
END
GO

USE ClinicalTrial;
GO

IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'clinical_user')
BEGIN
    CREATE LOGIN clinical_user WITH PASSWORD = 'Password123!';
END
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'clinical_user')
BEGIN
    CREATE USER clinical_user FOR LOGIN clinical_user;
    ALTER ROLE db_owner ADD MEMBER clinical_user;
END
GO