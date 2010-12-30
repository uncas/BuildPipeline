CREATE TABLE Pipeline
(
    PipelineId    int    NOT NULL
        IDENTITY(1, 1)
        CONSTRAINT PK_Pipeline PRIMARY KEY CLUSTERED
    , ProjectId    int    NOT NULL
        CONSTRAINT FK_Pipeline_Project FOREIGN KEY REFERENCES Project (ProjectId)
    , SourceUrl    nvarchar(256)    NOT NULL
    , SourceRevision    int    NOT NULL
    , Created    datetime    NOT NULL
        CONSTRAINT DF_Pipeline_Created DEFAULT GETDATE()
    , Modified    datetime    NOT NULL
        CONSTRAINT DF_Pipeline_Modified DEFAULT GETDATE()
    , CONSTRAINT UK_Pipeline_ProjectId_SourceUrl_SourceRevision UNIQUE (ProjectId, SourceUrl, SourceRevision)
)

GO

SET IDENTITY_INSERT Pipeline ON

INSERT INTO Pipeline
(PipelineId, ProjectId, SourceUrl, SourceRevision, Created, Modified)
SELECT * FROM Build

SET IDENTITY_INSERT Pipeline OFF

GO

ALTER TABLE BuildStep
    ADD
        PipelineId    int    NULL
            CONSTRAINT FK_BuildStep_Pipeline FOREIGN KEY REFERENCES Pipeline (PipelineId)

GO

UPDATE BuildStep
SET PipelineId = BuildId

GO

ALTER TABLE BuildStep
    ALTER
        COLUMN PipelineId    int    NOT NULL

GO

ALTER TABLE BuildStep
    ALTER
        COLUMN BuildId    int    NULL

GO

ALTER PROCEDURE stp_Build_Add
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
    
    DECLARE @pipelineId int
    SELECT @pipelineId = PipelineId FROM Pipeline WHERE ProjectId = @projectId
        AND SourceRevision = @SourceRevision AND SourceUrl = @SourceUrl
    IF @pipelineId IS NULL
    BEGIN
        INSERT INTO Pipeline (ProjectId, SourceUrl, SourceRevision)
        VALUES (@projectId, @SourceUrl, @SourceRevision)
        
        SET @pipelineId = @@IDENTITY
    END
    
    INSERT INTO BuildStep (PipelineId, BuildNumber, IsSuccessful, StepName)
    VALUES (@pipelineId, @BuildNumber, @IsSuccessful, @StepName)
END