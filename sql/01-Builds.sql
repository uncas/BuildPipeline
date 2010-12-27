--CREATE DATABASE BuildPipeline
GO
USE BuildPipeline
GO
DROP TABLE BuildStep
DROP TABLE Build
DROP TABLE Project
GO
CREATE TABLE Project
(
    ProjectId    int    NOT NULL
        IDENTITY(1, 1)
        CONSTRAINT PK_Project PRIMARY KEY CLUSTERED
    , ProjectName    nvarchar(64)    NOT NULL
        CONSTRAINT UK_Project_ProjectName UNIQUE
)
GO
CREATE TABLE Build
(
    BuildId    int    NOT NULL
        IDENTITY(1, 1)
        CONSTRAINT PK_Build PRIMARY KEY CLUSTERED
    , ProjectId    int    NOT NULL
        CONSTRAINT FK_Build_Project FOREIGN KEY REFERENCES Project (ProjectId)
    , SourceUrl    nvarchar(256)    NOT NULL
    , SourceRevision    int    NOT NULL
    , Created    datetime    NOT NULL
        CONSTRAINT DF_Build_Created DEFAULT GETDATE()
    , Modified    datetime    NOT NULL
        CONSTRAINT DF_Build_Modified DEFAULT GETDATE()
    , CONSTRAINT UK_Build_ProjectId_SourceUrl_SourceRevision UNIQUE (ProjectId, SourceUrl, SourceRevision)
)
GO
CREATE TABLE BuildStep
(
    BuildStepId    int    NOT NULL
        IDENTITY(1, 1)
        CONSTRAINT PK_BuildStep PRIMARY KEY CLUSTERED
    , BuildId    int    NOT NULL
        CONSTRAINT FK_BuildStep_Build FOREIGN KEY REFERENCES Build (BuildId)
    , IsSuccessful    bit    NOT NULL
    , StepName    nvarchar(64)    NOT NULL
    , BuildNumber    int    NOT NULL
    , Created    datetime    NOT NULL
        CONSTRAINT DF_BuildStep_Created DEFAULT GETDATE()
)

GO
DROP PROCEDURE stp_Build_Add
GO
CREATE PROCEDURE stp_Build_Add
(
    @ProjectName nvarchar(64)
    , @SourceUrl nvarchar(256)
    , @SourceRevision int
    , @IsSuccessful bit
    , @StepName nvarchar(64)
    , @BuildNumber int
)
AS
BEGIN
    DECLARE @projectId int
    SELECT @projectId = ProjectId FROM Project WHERE ProjectName = @ProjectName
    IF @projectId IS NULL
    BEGIN
        INSERT INTO Project (ProjectName) VALUES (@ProjectName)
        
        SET @projectId = @@IDENTITY
    END
    
    DECLARE @buildId int
    SELECT @buildId = BuildId FROM Build WHERE ProjectId = @projectId
        AND SourceRevision = @SourceRevision AND SourceUrl = @SourceUrl
    IF @buildId IS NULL
    BEGIN
        INSERT INTO Build ...
        SET @buildId = @@IDENTITY
    END
    
    INSERT INTO BuildStep ...
END