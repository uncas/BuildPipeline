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
    , SourceUrlBase    nvarchar(256)    NOT NULL
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
    , @SourceUrlBase nvarchar(256)
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
        INSERT INTO Project (ProjectName, SourceUrlBase) VALUES (@ProjectName, @SourceUrlBase)
        
        SET @projectId = @@IDENTITY
    END
    
    DECLARE @buildId int
    SELECT @buildId = BuildId FROM Build WHERE ProjectId = @projectId
        AND SourceRevision = @SourceRevision AND SourceUrl = @SourceUrl
    IF @buildId IS NULL
    BEGIN
        INSERT INTO Build (ProjectId, SourceUrl, SourceRevision)
        VALUES (@projectId, @SourceUrl, @SourceRevision)
        
        SET @buildId = @@IDENTITY
    END
    
    INSERT INTO BuildStep (BuildId, BuildNumber, IsSuccessful, StepName)
    VALUES (@buildId, @BuildNumber, @IsSuccessful, @StepName)
END

GO

EXEC stp_Build_Add
    @ProjectName = 'BuildPipeline'
    , @SourceUrlBase = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/'
    , @SourceUrl = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/trunk'
    , @SourceRevision = 952
    , @IsSuccessFul = 1
    , @StepName = 'Unit'
    , @BuildNumber = 3

EXEC stp_Build_Add
    @ProjectName = 'BuildPipeline'
    , @SourceUrlBase = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/'
    , @SourceUrl = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/trunk'
    , @SourceRevision = 952
    , @IsSuccessFul = 0
    , @StepName = 'Integration'
    , @BuildNumber = 3

EXEC stp_Build_Add
    @ProjectName = 'BuildPipeline'
    , @SourceUrlBase = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/'
    , @SourceUrl = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/trunk'
    , @SourceRevision = 954
    , @IsSuccessFul = 1
    , @StepName = 'Unit'
    , @BuildNumber = 4

EXEC stp_Build_Add
    @ProjectName = 'BuildPipeline'
    , @SourceUrlBase = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/'
    , @SourceUrl = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/branches/Experiment1'
    , @SourceRevision = 959
    , @IsSuccessFul = 0
    , @StepName = 'Unit'
    , @BuildNumber = 5
GO

SELECT * FROM Project
SELECT B.BuildId, B.ProjectId, REPLACE(B.SourceUrl, P.SourceUrlBase, '') AS SourceUrlRelative, B.SourceRevision, B.Created, B.Modified
FROM Build AS B
JOIN Project AS P
    ON B.ProjectId = P.ProjectId
SELECT * FROM BuildStep